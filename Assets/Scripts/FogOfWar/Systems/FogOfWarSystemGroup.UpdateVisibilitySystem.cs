using Dreambox.Math;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Omniverse
{
	public partial class FogOfWarSystemGroup
	{
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

				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

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

						var circleHandler = new BresenhamCircleHandler(coordinates.x, coordinates.y, fogOfWar.Visibility, fogOfWar.Occlusion, fogOfWarSettings.Size);
						Bresenham.Circle(coordinates.x, coordinates.y, radius, circleHandler);
					}
				}
			}
		}
	}
}
