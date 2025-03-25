using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

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

			foreach (var (unit, movementSpeed, navAgent) in SystemAPI.Query<Unit, RefRW<MovementSpeed>, RefRW <NavAgentComponent>>())
			{
				if (!navAgent.ValueRW.IsActive)
				{
					continue;
				}

				float3 direction = math.normalize(navAgent.ValueRW.targetPosition - unit.DynamicEntity.LocalTransform.ValueRW.Position);
				direction.y = 0;

				unit.DynamicEntity.LocalTransform.ValueRW.Position += direction * movementSpeed.ValueRW.Current * deltaTime;
			}
		}
	}
}
