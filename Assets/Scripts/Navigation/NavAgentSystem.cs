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

			foreach ((var unit, var localTransform, var movementSpeed, var navAgent) in SystemAPI.Query<Unit, RefRW<LocalTransform>, MovementSpeed, NavAgentComponent>())
			{
				if (!navAgent.IsActive)
				{
					continue;
				}

				float3 direction = math.normalize(navAgent.targetPosition - localTransform.ValueRW.Position);
				direction.y = 0;

				localTransform.ValueRW.Position += direction * movementSpeed.Current * deltaTime;
			}
		}
	}
}
