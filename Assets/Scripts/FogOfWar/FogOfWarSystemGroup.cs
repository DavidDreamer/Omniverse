using Dreambox.Math;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Omniverse
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	public partial class FogOfWarSystemGroup : ComponentSystemGroup
	{
		private const int Multiplier = 2;

		protected override void OnStartRunning()
		{
			base.OnStartRunning();

			var gameOptions = SystemAPI.ManagedAPI.GetSingleton<GameOptions>();
			if (gameOptions.FogOfWarType is FogOfWarType.None)
			{
				return;
			}

			int2 size = gameOptions.MapSize / Multiplier;

			var entity = EntityManager.CreateEntity();
			EntityManager.SetName(entity, "Fog Of War");
			EntityManager.AddComponent<FogOfWar>(entity);
			EntityManager.SetComponentData(entity, new FogOfWar()
			{
				Explored = gameOptions.FogOfWarType is FogOfWarType.Explored,
				Size = size,
				Occlusion = new NativeArray<bool>(size.x * size.y, Allocator.Persistent),
				Visibility = new NativeArray<CellVisibilityState>(size.x * size.y, Allocator.Persistent)
			});
		}

		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		public partial struct ClearVisibilitySystem : ISystem
		{
			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					if (fogOfWar.ValueRW.Explored)
					{
						for (int i = 0; i < fogOfWar.ValueRW.Visibility.Length; ++i)
						{
							fogOfWar.ValueRW.Visibility[i] = CellVisibilityState.Explored;
						}
					}
					else
					{
						for (int i = 0; i < fogOfWar.ValueRW.Visibility.Length; ++i)
						{
							if (fogOfWar.ValueRW.Visibility[i] is CellVisibilityState.Visible)
							{
								fogOfWar.ValueRW.Visibility[i] = CellVisibilityState.Explored;
							}
						}
					}
				}
			}
		}

		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(ClearVisibilitySystem))]
		public partial struct UpdateObstaclesSystem : ISystem
		{
			[BurstCompile]
			//TODO: Size-dependent algorithm
			public void OnUpdate(ref SystemState state)
			{
				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach (var item in SystemAPI.Query<RefRO<FogOfWarObstacle>, RefRO<LocalTransform>>())
					{
						RefRO<FogOfWarObstacle> obstacle = item.Item1;
						RefRO<LocalTransform> transform = item.Item2;

						int x = (int)(transform.ValueRO.Position.x / Multiplier);
						int y = (int)(transform.ValueRO.Position.z / Multiplier);
						
						int index = x * fogOfWar.ValueRO.Size.y + y;
						fogOfWar.ValueRW.Occlusion[index] = true;
					}
				}
			}
		}

		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateObstaclesSystem))]
		public partial struct UpdateAgentPositionSystem : ISystem
		{
			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach ((var agent, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					{
						float3 position = localTransform.ValueRO.Position;

						int x = (int)(position.x / Multiplier);
						int y = (int)(position.z / Multiplier);

						int index = x * fogOfWar.ValueRO.Size.y + y;

						agent.ValueRW.CellIndex = index;
					}
				}
			}
		}

		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateAgentPositionSystem))]
		public partial struct UpdateVisibilitySystem : ISystem
		{
			private readonly struct BresenhamCircleHandler : IBresenhamCircleHandler
			{
				private int X { get; }

				private int Y { get; }

				private NativeArray<CellVisibilityState> CellVisibilityStates { get; }

				private NativeArray<bool> CellObstacles { get; }

				private int2 Size { get; }

				public BresenhamCircleHandler(int x, int y, NativeArray<CellVisibilityState> cellVisibilityStates, NativeArray<bool> cellObstacles, int2 size)
				{
					X = x;
					Y = y;

					CellVisibilityStates = cellVisibilityStates;
					CellObstacles = cellObstacles;
					Size = size;
				}

				public void HandlePoint(int x, int y)
				{
					x = math.clamp(x, 0, Size.x - 1);
					y = math.clamp(y, 0, Size.y - 1);

					var lineHandler = new BresenhamLineHandler(CellVisibilityStates, CellObstacles, Size);
					Bresenham.Line(X, Y, x, y, lineHandler);
				}
			}

			private struct BresenhamLineHandler : IBresenhamLineHandler
			{
				private NativeArray<CellVisibilityState> CellVisibilityStates;
				private NativeArray<bool> CellObstacles { get; }

				private int2 Size { get; }

				public BresenhamLineHandler(
					NativeArray<CellVisibilityState> cellVisibilityStates,
					NativeArray<bool> cellObstacles,
					int2 size)
				{
					CellVisibilityStates = cellVisibilityStates;
					CellObstacles = cellObstacles;
					Size = size;
				}

				public bool HandlePoint(int x, int y)
				{
					int index = x * Size.y + y;

					CellVisibilityStates[index] = CellVisibilityState.Visible;

					return CellObstacles[index];
				}
			}

			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				if (SystemAPI.HasSingleton<Player>() is false)
				{
					return;
				}

				var player = SystemAPI.GetSingleton<Player>();

				foreach (var fogOfWar in SystemAPI.Query<FogOfWar>())
				{
					foreach ((var agent, var faction, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, Faction, RefRO<LocalTransform>>())
					{
						if (faction.ID != player.FactionID)
						{
							continue;
						}

						float3 position = localTransform.ValueRO.Position;

						int x0 = (int)(position.x / Multiplier);
						int y0 = (int)(position.z / Multiplier);
						int radius = (int)(agent.ValueRW.VisionRange / Multiplier);

						var circleHandler = new BresenhamCircleHandler(x0, y0, fogOfWar.Visibility, fogOfWar.Occlusion, fogOfWar.Size);
						Bresenham.Circle(x0, y0, radius, circleHandler);
					}
				}
			}
		}
	}
}
