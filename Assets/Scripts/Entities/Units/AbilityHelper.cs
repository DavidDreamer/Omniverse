using Omniverse.Abilities;

namespace Omniverse.Units
{
	public static class AbilityHelper
	{
		public static AbilityCastError CanBeCasted(this Ability ability, Unit unit)
		{
			if (ability.Cooldown is not null && ability.Cooldown.IsActive)
			{
				return AbilityCastError.IsOnCooldown;
			}

			if (ability.Casting.InProcess)
			{
				return AbilityCastError.AlreadyInProcess;
			}

			foreach (CostDesc costDesc in ability.Desc.Cost)
			{
				if (!unit.Properties.ContainsKey(costDesc.PropertyID))
				{
					return AbilityCastError.NotEnoughResources;
				}

				//TODO processing
				if (unit.Properties[costDesc.PropertyID].Amount.Value < costDesc.PropertyModifier.Value)
				{
					return AbilityCastError.NotEnoughResources;
				}
			}

			return AbilityCastError.None;
		}
	}
}
