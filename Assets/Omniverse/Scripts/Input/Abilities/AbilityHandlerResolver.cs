using Omniverse.Abilities;
using Omniverse.Entities.Units;
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
		private UnitTargetHandler UnitTargetHandler { get; set; }

		[Inject]
		private TrajectoryAbilityHandler TrajectoryAbilityHandler { get; set; }

		public void TryCastAbility(Unit unit, Ability ability)
		{
			switch (ability.Desc.Target)
			{
				case NonTarget:
				{
					AbilityHandler.TryCastAbility(unit, ability);
					break;
				}
				case PointTarget:
				{
					if (PointAbilityHandler.InProcess)
					{
						PointAbilityHandler.Cancell();
					}
					else
					{
						PointAbilityHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}

					break;
				}
				case TrajectoryTarget:
				{
					if (TrajectoryAbilityHandler.InProcess)
					{
						TrajectoryAbilityHandler.Cancell();
					}
					else
					{
						TrajectoryAbilityHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}

					break;
				}
				case EntityTarget:
				{
					if (UnitTargetHandler.InProcess)
					{
						UnitTargetHandler.Cancell();
					}
					else
					{
						UnitTargetHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}

					break;
				}
			}
		}
	}
}
