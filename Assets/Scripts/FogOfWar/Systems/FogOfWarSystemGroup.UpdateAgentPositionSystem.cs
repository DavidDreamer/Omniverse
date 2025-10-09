using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Omniverse
{
	public partial class FogOfWarSystemGroup
	{
		[BurstCompile]
		[DisableAutoCreation]
		public partial struct UpdateAgentPositionSystem : ISystem
		{
			[BurstCompile]
			public void OnUpdate(ref SystemState state)
			{
				int2 mapSize = SystemAPI.GetSingleton<MapSettings>().Size;
				var fogOfWarSettings = SystemAPI.GetSingleton<FogOfWarSettings>();

				foreach (var fogOfWar in SystemAPI.Query<RefRW<FogOfWar>>())
				{
					foreach ((var agent, var localTransform) in SystemAPI.Query<RefRW<FogOfWarAgent>, RefRO<LocalTransform>>())
					{
						float3 position = localTransform.ValueRO.Position;
						agent.ValueRW.CellIndex = FogOfWarUtils.CellIndexFromPosition(position, mapSize, fogOfWarSettings.Size);
					}
				}
			}
		}
	}
}
