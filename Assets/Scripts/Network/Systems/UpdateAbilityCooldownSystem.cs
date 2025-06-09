using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Omniverse.Network
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

			foreach ((var cooldown, var entity) in SystemAPI.Query<RefRW<Cooldown>>().WithAll<Cooldown>().WithEntityAccess())
			{
				float timeLeft = math.max(0f, cooldown.ValueRW.TimeLeft - deltaTime);
				cooldown.ValueRW.TimeLeft = timeLeft;

				if (timeLeft == 0)
				{
					commandBuffer.SetComponentEnabled<Cooldown>(entity, false);
				}
			}

			commandBuffer.Playback(state.EntityManager);
		}
	}
}
