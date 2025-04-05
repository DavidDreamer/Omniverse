using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	public static class AbilityHelper
	{
		public static AbilityCastError CanBeCasted(this Entity ability, EntityManager entityManager)
		{
			if (entityManager.HasComponent<Cooldown>(ability))
			{
				if (entityManager.IsComponentEnabled<Cooldown>(ability))
				{
					return AbilityCastError.IsOnCooldown;
				}
			}

			if (entityManager.HasComponent<Casting>(ability))
			{
				var casting = entityManager.GetComponentData<Casting>(ability);
				if (casting.InProcess)
				{
					//TODO
					//return AbilityCastError.AlreadyInProcess;
				}
			}

			var owner = entityManager.GetComponentData<Owner>(ability).Entity;

			if (entityManager.HasComponent<Manacost>(ability))
			{
				var manacost = entityManager.GetComponentData<Manacost>(ability);

				if (entityManager.HasComponent<Mana>(owner))
				{
					var mana = entityManager.GetComponentData<Mana>(owner);

					if (mana.Current < manacost.Value)
					{
						return AbilityCastError.NotEnoughMana;
					}
				}
				else
				{
					return AbilityCastError.NotEnoughMana;
				}
			}

			return AbilityCastError.None;
		}
	}
}
