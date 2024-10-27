using Omniverse.Abilities;
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

			if (ability.Desc.Target is NoneTarget)
			{
				if (ability.Desc.Casting.Time == 0)
				{
					var castImmediateAbilityCommand = new CastImmediateAbilityCommand(unit, ability);
					unit.CommandModule.Add(castImmediateAbilityCommand);
				}
				else
				{
					var command = new CastAbilityCommand<None>(unit, ability, None.Instance);
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
			switch (ActiveAbility.Desc.Target)
			{
				case VectorTarget vectorTarget:
				{
					if (!CommonActions.Select.WasPressedThisFrame())
					{
						return;
					}

					if (!cursorWorldPosition.HasValue)
					{
						return;
					}

					if (vectorTarget.Mode is VectorTargetMode.Direction)
					{
						Vector3 direction = cursorWorldPosition.Value - ActiveUnit.transform.position;
						direction.Set(direction.x, 0, direction.z);
						direction.Normalize();

						var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, direction);
						AddCommand(ActiveUnit, castAbilityCommand);
					}
					else
					{
						var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
						AddCommand(ActiveUnit, approachPositionForAbilityCastCommand);
						var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
						ActiveUnit.CommandModule.Add(castAbilityCommand);
					}
					break;
				}
				case UnitTarget:
				case ResourceSourceTarget:
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

					switch (target)
					{
						case Unit unit:
						{
							var castAbilityCommand = new CastAbilityCommand<Unit>(ActiveUnit, ActiveAbility, unit);
							ActiveUnit.CommandModule.Add(castAbilityCommand);
							break;
						}
						case ResourceSource resourceSource:
						{
							var castAbilityCommand = new CastAbilityCommand<ResourceSource>(ActiveUnit, ActiveAbility, resourceSource);
							ActiveUnit.CommandModule.Add(castAbilityCommand);
							break;
						}
					}

					break;
				}
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
