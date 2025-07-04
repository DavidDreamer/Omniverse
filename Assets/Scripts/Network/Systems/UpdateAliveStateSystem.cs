using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse.Network
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedFixedStepSimulationSystemGroup))]
	public partial struct UpdateAliveStateSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

			foreach ((var health, var entity) in SystemAPI.Query<RefRO<Health>>().WithAll<Alive>().WithEntityAccess())
			{
				if (health.ValueRO.Current == 0)
				{
					commandBuffer.SetComponentEnabled<Alive>(entity, false);
				}
			}

			commandBuffer.Playback(state.EntityManager);
		}
	}
}
