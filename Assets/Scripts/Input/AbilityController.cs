using Omniverse.Abilities;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Input
{
	public class AbilityController
	{
		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		[Inject]
		private ErrorHandler ErrorHandler { get; set; }

		[Inject]
		private Detector EntityDetector { get; set; }

		public Unit ActiveUnit { get; private set; }

		public Ability ActiveAbility { get; private set; }

		public void Process(Unit unit, Ability ability)
		{
			if (ActiveAbility == ability)
			{
				Discard();
				return;
			}

			if (ActiveAbility is not null)
			{
				Discard();
			}

			AbilityCastError error = ability.CanBeCasted(unit);

			if (error is not AbilityCastError.None)
			{
				ErrorHandler.Hadle(error.ToString());
				return;
			}

			TargetType targetType = ability.Desc.Target.Type;

			if (targetType is TargetType.None)
			{
				Cast(unit, ability);
			}
			else
			{
				ActiveUnit = unit;
				ActiveAbility = ability;
			}
		}

		public void ProcessAbility(Entity target, Vector3 cursorWorldPosition)
		{
			TargetType targetType = ActiveAbility.Desc.Target.Type;

			if (targetType is TargetType.Point)
			{
				if (!CommonActions.Select.WasPressedThisFrame())
				{
					return;
				}

				ActiveAbility.OperationContext.Points.Add(cursorWorldPosition);
			}
			else if (targetType is TargetType.Direction)
			{
				if (!CommonActions.Select.WasPressedThisFrame())
				{
					return;
				}

				Vector3 direction = cursorWorldPosition - ActiveUnit.transform.position;
				direction.Set(direction.x, 0, direction.z);
				direction.Normalize();
				ActiveAbility.OperationContext.Directions.Add(direction);
			}
			else if (targetType is TargetType.Unit or TargetType.ResourceSource)
			{
				if (!CommonActions.Select.WasPressedThisFrame())
				{
					return;
				}

				if (target == null)
				{
					return;
				}

				ActiveAbility.OperationContext.Entities.Add(target);
			}

			AbilityCastError error = ActiveAbility.CanBeCasted(ActiveUnit);

			if (error is not AbilityCastError.None)
			{
				ErrorHandler.Hadle(error.ToString());
			}
			else
			{
				Cast(ActiveUnit, ActiveAbility);
			}

			Discard();
		}

		private void Cast(Unit unit, Ability ability) => unit.Cast(ability, default).Forget();

		private void Discard()
		{
			ActiveAbility = null;
		}
	}
}
