using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct NavAgentSystem : ISystem
	{
		[BurstCompile]
		private void OnUpdate(ref SystemState state)
		{
			float deltaTime = SystemAPI.Time.DeltaTime;

			foreach (var (navAgent, movementSpeed, localTransform) in SystemAPI.Query<RefRW<NavAgentComponent>, RefRW<MovementSpeed>, RefRW <LocalTransform>>())
			{
				if (!navAgent.ValueRW.IsActive)
				{
					continue;
				}

				float3 direction = math.normalize(navAgent.ValueRW.targetPosition - localTransform.ValueRW.Position);
				direction.y = 0;

				localTransform.ValueRW.Position += direction * movementSpeed.ValueRW.Current * deltaTime;
			}
		}
	}
}
