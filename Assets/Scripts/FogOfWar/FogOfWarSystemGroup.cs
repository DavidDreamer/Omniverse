using Dreambox.Math;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation, WorldSystemFilterFlags.ClientSimulation)]
	public partial class FogOfWarSystemGroup : ComponentSystemGroup
	{
		protected override void OnStartRunning()
		{
			base.OnStartRunning();

			var mapSettings = SystemAPI.GetSingleton<MapSettings>();

			if (mapSettings.FogOfWarMode is FogOfWarMode.Revealed)
			{
				return;
			}

			int2 size = mapSettings.Size / FogOfWar.Multiplier;

			var entity = EntityManager.CreateEntity();
			EntityManager.SetName(entity, "Fog Of War");
			EntityManager.AddComponent<FogOfWar>(entity);
			EntityManager.SetComponentData(entity, new FogOfWar()
			{
				Explored = mapSettings.FogOfWarMode is FogOfWarMode.Explored,
				Size = size,
				Occlusion = new NativeArray<bool>(size.x * size.y, Allocator.Persistent),
				Visibility = new NativeArray<CellVisibilityState>(size.x * size.y, Allocator.Persistent)
			});
		}

		protected override void OnStopRunning()
		{
			base.OnStopRunning();

			if (SystemAPI.HasSingleton<FogOfWar>())
			{
				var fogOfWar = SystemAPI.GetSingleton<FogOfWar>();
				fogOfWar.Occlusion.Dispose();
				fogOfWar.Visibility.Dispose();
			}
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
				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;

				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach (var item in SystemAPI.Query<RefRO<FogOfWarObstacle>, RefRO<LocalTransform>>())
					{
						RefRO<FogOfWarObstacle> obstacle = item.Item1;
						RefRO<LocalTransform> transform = item.Item2;

						int index = FogOfWarUtils.CellIndexFromPosition(transform.ValueRO.Position, mapSize, fogOfWar.ValueRO.Size);
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
				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;

				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach ((var agent, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					{
						float3 position = localTransform.ValueRO.Position;
						agent.ValueRW.CellIndex = FogOfWarUtils.CellIndexFromPosition(position, mapSize, fogOfWar.ValueRO.Size);
					}
				}
			}
		}

		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateVisibilitySystem))]
		public partial struct UpdateUnitsRelevancy : ISystem
		{
			[BurstCompile]
			public void OnCreate(ref SystemState state)
			{
				var ghostRelevancy = SystemAPI.GetSingleton<GhostRelevancy>();
				ghostRelevancy.GhostRelevancyMode = GhostRelevancyMode.SetIsRelevant;
			}

			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				var ghostRelevancy = SystemAPI.GetSingleton<GhostRelevancy>();

				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;

				foreach ((var unit, var localTransform, var entity) in SystemAPI.Query<Unit, LocalTransform>().WithEntityAccess())
				{
					//var a = FogOfWarUtils.CellIndexFromPosition

					//foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
					//{
					//	foreach ((var agent, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					//	{
					//		float3 position = localTransform.ValueRO.Position;
					//		agent.ValueRW.CellIndex = fogOfWar.ValueRW.CellIndexFromPosition(position, mapSize);
					//	}
					//}

					//ghostRelevancy.GhostRelevancySet.Add
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

			public void OnCreate(ref SystemState state)
			{
				state.RequireForUpdate<Player>();
			}

			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				var player = SystemAPI.GetSingleton<Player>();
				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;

				foreach (var fogOfWar in SystemAPI.Query<FogOfWar>())
				{
					foreach ((var agent, var faction, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, Faction, RefRO<LocalTransform>>())
					{
						if (faction.ID != player.FactionID)
						{
							continue;
						}

						float3 position = localTransform.ValueRO.Position;
						int2 coordinates = FogOfWarUtils.CellCoordinatesFromPosition(position, mapSize);
						int radius = (int)(agent.ValueRW.VisionRange) / FogOfWar.Multiplier;

						var circleHandler = new BresenhamCircleHandler(coordinates.x, coordinates.y, fogOfWar.Visibility, fogOfWar.Occlusion, fogOfWar.Size);
						Bresenham.Circle(coordinates.x, coordinates.y, radius, circleHandler);
					}
				}
			}
		}
	}
}
