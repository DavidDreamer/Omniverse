using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Omniverse
{
	public partial class FogOfWarSystemGroup
	{
		[BurstCompile]
		[UpdateInGroup(typeof(FogOfWarSystemGroup))]
		[UpdateAfter(typeof(RefreshSystem))]
		public partial struct UpdateObstaclesSystem : ISystem
		{
			[BurstCompile]
			//TODO: Size-dependent algorithm
			public void OnUpdate(ref SystemState state)
			{
				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;
				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach (var item in SystemAPI.Query<RefRO<FogOfWarObstacle>, RefRO<LocalTransform>>())
					{
						RefRO<FogOfWarObstacle> obstacle = item.Item1;
						RefRO<LocalTransform> transform = item.Item2;

						int index = FogOfWarUtils.CellIndexFromPosition(transform.ValueRO.Position, mapSize, fogOfWarSettings.Size);
						fogOfWar.ValueRW.Occlusion[index] = true;
					}
				}
			}
		}
	}
}
