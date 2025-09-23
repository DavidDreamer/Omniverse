using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	public static class AbilityHelper
	{
		public static AbilityCastError CanBeCasted(this Ability ability, Entity entity, EntityManager entityManager)
		{
			if (ability.Cooldown.Active)
			{
				return AbilityCastError.IsOnCooldown;
			}

			if (ability.Casting.InProcess)
			{
				return AbilityCastError.AlreadyInProcess;
			}

			if (entityManager.HasComponent<Mana>(entity))
			{
				var mana = entityManager.GetComponentData<Mana>(entity);

				if (mana.Current < ability.Manacost.Value)
				{
					return AbilityCastError.NotEnoughMana;
				}
			}
			else
			{
				return AbilityCastError.NotEnoughMana;
			}

			return AbilityCastError.None;
		}
	}
}
