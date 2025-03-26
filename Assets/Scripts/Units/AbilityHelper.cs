using Omniverse.Abilities;

namespace Omniverse
{
	public static class AbilityHelper
	{
		public static AbilityCastError CanBeCasted(this Ability ability, DynamicEntity entity)
		{
			//if (ability.Cooldown.IsActive)
			//{
			//	return AbilityCastError.IsOnCooldown;
			//}

			//TODO ECS
			//if (ability.Casting.InProcess)
			//{
			//	return AbilityCastError.AlreadyInProcess;
			//}

			//foreach (CostDesc costDesc in ability.Desc.Cost)
			//{
			//	if (!unit.Properties.ContainsKey(costDesc.PropertyID))
			//	{
			//		return AbilityCastError.NotEnoughResources;
			//	}

			//	//TODO processing
			//	if (unit.Properties[costDesc.PropertyID].Amount < costDesc.PropertyModifier.Value)
			//	{
			//		return AbilityCastError.NotEnoughResources;
			//	}
			//}

			return AbilityCastError.None;
		}
	}
}
