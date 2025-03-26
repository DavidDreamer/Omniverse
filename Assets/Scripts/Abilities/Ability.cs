using System.Collections.Generic;
using Omniverse.Abilities;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Omniverse
{
	public partial struct UpdateAbilityCooldownSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (var abilityModule in SystemAPI.Query<AbilityModule>())
			{
				abilityModule.Update(SystemAPI.Time.DeltaTime);
			}
		}
	}

	public class AbilityModule : IComponentData
	{
		public List<Ability> Abilities = new();

		public void Update(float deltaTime)
		{
			foreach (Ability ability in Abilities)
			{
				ability.Update(deltaTime);
			}
		}
	}

	public class Ability
	{
		public MetaData MetaData;

		public Cooldown Cooldown;

		public Casting Casting;

		public ITarget Target;

		public float CastRange;

		public IOperation ActiveOperation;

		public void Update(float deltaTime)
		{
			Cooldown.TimeLeft = math.max(0f, Cooldown.TimeLeft - deltaTime);
		}

		public void Cast<TTarget>(EntityManager entityManager, DynamicEntity entity, TTarget target)
		{
			Cooldown.TimeLeft = Cooldown.Time;

			var operation = (IOperation<TTarget>)ActiveOperation;
			operation.Perform(entityManager, entity, target);
		}
	}
}
