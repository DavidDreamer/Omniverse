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

			foreach ((var abilityBuffer, var input, var entity) in SystemAPI.Query<DynamicBuffer<Ability>, RefRW<AbilityInput>>().WithEntityAccess())
			{
				if (!input.ValueRO.Cast.IsSet)
				{
					continue;
				}

				int index = input.ValueRW.AbilityIndex;
				Ability ability = abilityBuffer.ElementAt(index);
				abilityBuffer.ElementAt(index).Cooldown.TimeLeft = ability.Cooldown.Duration;

				var mana = SystemAPI.GetComponentRW<Mana>(entity);
				mana.ValueRW.Current -= ability.Manacost.Value;

				var abilityActiveOperation = ability.Desc.Value.ActiveOperation;
				var dynamicEntity = SystemAPI.GetAspect<DynamicEntity>(entity);

				switch (ability.Desc.Value.Target)
				{
					case NoneTarget:
					{
						var operation = (IOperation<None>)abilityActiveOperation;
						operation.Perform(state.EntityManager, dynamicEntity, None.Instance);
						break;
					}
					case VectorTarget vectorTarget:
					{
						var operation = (IOperation<Vector3>)abilityActiveOperation;
						operation.Perform(state.EntityManager, dynamicEntity, input.ValueRO.Vector);
						break;
					}
				}
			}
		}
	}
}
