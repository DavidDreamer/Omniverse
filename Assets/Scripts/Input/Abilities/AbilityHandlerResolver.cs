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
		private EntityTargetHandler EntityTargetHandler { get; set; }

		// [Inject]
		// private TrajectoryAbilityHandler TrajectoryAbilityHandler { get; set; }

		public void TryCastAbility(Unit unit, Ability ability)
		{
			switch (ability.Target)
			{
				case null:
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
				// case TrajectoryTarget:
				// {
				// 	if (TrajectoryAbilityHandler.InProcess)
				// 	{
				// 		TrajectoryAbilityHandler.Cancell();
				// 	}
				// 	else
				// 	{
				// 		TrajectoryAbilityHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				// 	}
				//
				// 	break;
				// }
				case EntityTarget:
					{
						if (EntityTargetHandler.InProcess)
						{
							EntityTargetHandler.Cancell();
						}
						else
						{
							EntityTargetHandler.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
						}

						break;
					}
			}
		}
	}
}
