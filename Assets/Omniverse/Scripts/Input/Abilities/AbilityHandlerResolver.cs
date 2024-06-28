using Omniverse.Abilities;
using Omniverse.Entities.Units;
using UnityEngine;
using VContainer;

public class AbilityHandlerResolver
{
	[Inject]
	private AbilityController AbilityController { get; set; }
	
	[Inject]
	private PointAbilityController PointAbilityController { get; set; }

	[Inject]
	private UnitTargetController UnitTargetController { get; set; }

	[Inject]
	private TrajectoryAbilityController TrajectoryAbilityController { get; set; }

	public void TryCastAbility(Unit unit, Ability ability)
	{
		switch (ability.Desc.Target)
		{
			case NonTarget:
			{
				AbilityController.TryCastAbility(unit, ability);
				break;
			}
			case PointTarget:
			{
				if (PointAbilityController.InProcess)
				{
					PointAbilityController.Cancell();
				}
				else
				{
					PointAbilityController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}

				break;
			}
			case TrajectoryTarget:
			{
				if (TrajectoryAbilityController.InProcess)
				{
					TrajectoryAbilityController.Cancell();
				}
				else
				{
					TrajectoryAbilityController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}

				break;
			}
			case EntityTarget:
			{
				if (UnitTargetController.InProcess)
				{
					UnitTargetController.Cancell();
				}
				else
				{
					UnitTargetController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
				}

				break;
			}
		}
	}
}
