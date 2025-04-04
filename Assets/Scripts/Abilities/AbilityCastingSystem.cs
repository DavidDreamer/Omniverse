using UnityEngine;
using Omniverse.Abilities;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Omniverse
{
	[BurstCompile]
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct AbilityCastingSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var networkTime = SystemAPI.GetSingleton<NetworkTime>();

			if (!networkTime.IsFirstTimeFullyPredictingTick)
			{
				return;
			}

			foreach ((var input, var entity) in SystemAPI.Query<RefRW<AbilityInput>>().WithEntityAccess())
			{
				if (input.ValueRO.Cast.IsSet)
				{
					if (SystemAPI.HasComponent<Cooldown>(entity))
					{
						var cooldwon = SystemAPI.GetComponentRW<Cooldown>(entity);
						cooldwon.ValueRW.TimeLeft = cooldwon.ValueRW.Duration;
						SystemAPI.SetComponentEnabled<Cooldown>(entity, true);
					}

					var abilityTarget = SystemAPI.ManagedAPI.GetComponent<AbilityTarget>(entity);
					var abilityActiveOperation = SystemAPI.ManagedAPI.GetComponent<AbilityActiveOperation>(entity);

					Entity unit = SystemAPI.GetComponent<Owner>(entity).Entity;
					var dynamicEntity = SystemAPI.GetAspect<DynamicEntity>(unit);

					switch (abilityTarget.Target)
					{
						case NoneTarget:
						{
							var operation = (IOperation<None>)abilityActiveOperation.Operation;
							operation.Perform(state.EntityManager, dynamicEntity, None.Instance);
							break;
						}
						case VectorTarget vectorTarget:
						{
							var operation = (IOperation<Vector3>)abilityActiveOperation.Operation;
							operation.Perform(state.EntityManager, dynamicEntity, input.ValueRO.Vector);
							break;
						}
					}
				}
			}
		}
	}
}
