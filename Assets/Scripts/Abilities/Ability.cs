using Unity.Mathematics;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;
using System.Collections.Generic;
using Omniverse.Abilities;

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
		public string Name;

		public WeakObjectReference<Sprite> Icon;

		public Cooldown Cooldown;

		public ITarget Target;

		public float CastRange;

		public IOperation ActiveOperation;

		public void Update(float deltaTime)
		{
			Cooldown.TimeLeft = math.max(0f, Cooldown.TimeLeft - deltaTime);
		}

		public void Cast<TTarget>(Entity entity, TTarget target)
		{
			Cooldown.TimeLeft = Cooldown.Time;

			//var operation = (IOperation<TTarget>)ActiveOperation;
			//operation.Perform(entity, target);
		}
	}
}
