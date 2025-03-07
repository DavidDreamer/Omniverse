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

		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		public partial struct InitializeSystem : ISystem, ISystemStartStop
		{
			public void OnStartRunning(ref SystemState state)
			{
				var entityManager = state.EntityManager;
				var map = SystemAPI.GetSingleton<Map>();
				int2 size = map.Size / Multiplier;

				var entity = entityManager.CreateEntity();
				entityManager.SetName(entity, "Fog Of War");
				entityManager.AddComponent<FogOfWar>(entity);
				entityManager.SetComponentData(entity, new FogOfWar()
				{
					Explored = true,
					Size = size,
					Occlusion = new NativeArray<bool>(size.x * size.y, Allocator.Persistent),
					Visibility = new NativeArray<CellVisibilityState>(size.x * size.y, Allocator.Persistent)
				});
			}

			public void OnStopRunning(ref SystemState state)
			{
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

		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(ClearVisibilitySystem))]
		public partial struct UpdateObstaclesSystem : ISystem
		{
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
;
						int index = x * fogOfWar.ValueRO.Size.y + y;
						fogOfWar.ValueRW.Occlusion[index] = true;
					}
				}
			}
		}

		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateObstaclesSystem))]
		public partial struct UpdateAgentPositionSystem : ISystem
		{
			public void OnUpdate(ref SystemState state)
			{
				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach (var agent in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					{
						float3 position = agent.Item2.ValueRO.Position;

						int x = (int)(position.x / Multiplier);
						int y = (int)(position.z / Multiplier);

						int index = x * fogOfWar.ValueRO.Size.y + y;

						agent.Item1.ValueRW.CellIndex = index;
					}
				}
			}
		}

		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(UpdateAgentPositionSystem))]
		public partial struct UpdateVisibilitySystem : ISystem
		{
			public void OnUpdate(ref SystemState state)
			{
				foreach (var fogOfWar in SystemAPI.Query<FogOfWar>())
				{
					foreach (var item in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					{
						FogOfWarAgent agent = item.Item1.ValueRO;
						float3 position = item.Item2.ValueRO.Position;

						int x0 = (int)(position.x / Multiplier);
						int y0 = (int)(position.z / Multiplier);
						int radius = (int)(agent.VisionRange / Multiplier);
						//int factionId = agent.FactionID;

						var circleHandler = new BresenhamCircleHandler(x0, y0, fogOfWar.Visibility, fogOfWar.Occlusion, fogOfWar.Size);
						Bresenham.Circle(x0, y0, radius, circleHandler);
					}
				}
			}
		}
	}
}
