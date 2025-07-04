using Omniverse.Abilities;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Omniverse
{
	[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
	public partial struct AbilityCastingSystem : ISystem
	{
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
					Entity owner = SystemAPI.GetComponent<Owner>(entity).Entity;

					if (SystemAPI.HasComponent<Cooldown>(entity))
					{
						var cooldwon = SystemAPI.GetComponentRW<Cooldown>(entity);
						cooldwon.ValueRW.TimeLeft = cooldwon.ValueRW.Duration;
						SystemAPI.SetComponentEnabled<Cooldown>(entity, true);
					}

					if (SystemAPI.HasComponent<Manacost>(entity))
					{
						var manacost = SystemAPI.GetComponent<Manacost>(entity);
						var mana = SystemAPI.GetComponentRW<Mana>(owner);
						mana.ValueRW.Current -= manacost.Value;
					}

					var abilityTarget = SystemAPI.ManagedAPI.GetComponent<AbilityTarget>(entity);
					var abilityActiveOperation = SystemAPI.ManagedAPI.GetComponent<AbilityActiveOperation>(entity);

					var dynamicEntity = SystemAPI.GetAspect<DynamicEntity>(owner);

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
