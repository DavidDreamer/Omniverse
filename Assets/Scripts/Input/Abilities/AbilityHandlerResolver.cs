using Omniverse.Abilities;
using Omniverse.Units;
using VContainer;

namespace Omniverse.Input
{
	public class AbilityHandlerResolver
	{
		[Inject]
		private AbilityHandler AbilityHandler { get; set; }

		[Inject]
		private PointAbilityHandler PointAbilityHandler { get; set; }

		[Inject]
		private DirectionAbilityHandler DirectionAbilityHandler { get; set; }

		[Inject]
		private EntityTargetHandler EntityTargetHandler { get; set; }

		public void TryCastAbility(Unit unit, Ability ability)
		{
			TargetType targetType = ability.Desc.Target.Type;

			if (targetType == TargetType.None)
			{
				AbilityHandler.TryCastAbility(unit, ability);
			}
			else if (targetType.HasFlag(TargetType.Point))
			{
				if (PointAbilityHandler.InProcess)
				{
					PointAbilityHandler.Cancell();
				}
				else
				{
					PointAbilityHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}
			}
			else if (targetType.HasFlag(TargetType.Direction))
			{
				if (DirectionAbilityHandler.InProcess)
				{
					DirectionAbilityHandler.Cancell();
				}
				else
				{
					DirectionAbilityHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}
			}
			else if (targetType.HasFlag(TargetType.Unit) || targetType.HasFlag(TargetType.ResourceSource))
			{
				if (EntityTargetHandler.InProcess)
				{
					EntityTargetHandler.Cancell();
				}
				else
				{
					EntityTargetHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}
			}
		}
	}
}
