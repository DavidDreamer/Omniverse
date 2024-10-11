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
				if (ability.Desc.Cast.Time == 0)
				{
					var castImmediateAbilityCommand = new CastImmediateAbilityCommand(unit, ability);
					unit.CommandModule.Add(castImmediateAbilityCommand);
				}
				else
				{
					var command = new CastAbilityCommand(unit, ability);
					AddCommand(unit, command);
				}
			}
			else
			{
				ActiveUnit = unit;
				ActiveAbility = ability;
			}
		}

		public void ProcessAbility(Entity target, Vector3? cursorWorldPosition, bool additiveMode)
		{
			TargetType targetType = ActiveAbility.Desc.Target.Type;

			if (targetType is TargetType.Point)
			{
				if (!CommonActions.Select.WasPressedThisFrame())
				{
					return;
				}

				if (!cursorWorldPosition.HasValue)
				{
					return;
				}

				var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
				AddCommand(ActiveUnit, approachPositionForAbilityCastCommand);
				var castPointTargetAbilityCommand = new CastPointTargetAbilityCommand(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
				ActiveUnit.CommandModule.Add(castPointTargetAbilityCommand);
			}
			else if (targetType is TargetType.Direction)
			{
				if (!CommonActions.Select.WasPressedThisFrame())
				{
					return;
				}

				if (!cursorWorldPosition.HasValue)
				{
					return;
				}

				Vector3 direction = cursorWorldPosition.Value - ActiveUnit.transform.position;
				direction.Set(direction.x, 0, direction.z);
				direction.Normalize();

				var castDirectionalAbilityCommand = new CastDirectionalAbilityCommand(ActiveUnit, ActiveAbility, direction);
				AddCommand(ActiveUnit, castDirectionalAbilityCommand);
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

				var approachEntityForAbilityCastCommand = new ApproachEntityForAbilityCastCommand(ActiveUnit, ActiveAbility, target);
				AddCommand(ActiveUnit, approachEntityForAbilityCastCommand);
				var castEntityTargetAbilityCommand = new CastEntityTargetAbilityCommand(ActiveUnit, ActiveAbility, target);
				ActiveUnit.CommandModule.Add(castEntityTargetAbilityCommand);
			}

			Discard();
		}

		private void Discard()
		{
			ActiveAbility = null;
		}

		private void AddCommand(Unit unit, ICommand command)
		{
			if (!CommonActions.AdditiveMode.IsPressed())
			{
				unit.CommandModule.Reset();
			}

			unit.CommandModule.Add(command);
		}
	}
}
