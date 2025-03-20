using Omniverse.Abilities;
using Unity.Burst;
using Unity.Entities;
using static InputActions;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessAbilityInputSystem : ISystem
	{
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.EntityManager.CreateSingleton<AbilityInput>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var pointer = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingleton<Selection>();
			var abilityInput = SystemAPI.GetSingleton<AbilityInput>();
			var inputSystemData = SystemAPI.ManagedAPI.GetSingleton<InputSystemData>();

			CommonActions commonActions = inputSystemData.InputActions.Common;
			AbilitiesActions abilitiesActions = inputSystemData.InputActions.Abilities;
			bool additiveMode = commonActions.AdditiveMode.IsPressed();

			if (!selection.HasSelection)
			{
				return;
			}
			 
			var entity = selection.Entity;
			var abilityModule = SystemAPI.ManagedAPI.GetComponent<AbilityModule>(entity);

			if (abilityInput.InProcess)
			{
				//switch (ActiveAbility.Desc.Target)
				//{
				//	case VectorTarget vectorTarget:
				//	{

				//		if (!commonActions.Select.WasPressedThisFrame())
				//		{
				//			return;
				//		}

				//		if (pointer.TargetType is not PointerTargetType.World)
				//		{
				//			return;
				//		}

				//		if (vectorTarget.Mode is VectorTargetMode.Direction)
				//		{
				//			Vector3 direction = cursorWorldPosition.Value - ActiveUnit.transform.position;
				//			direction.Set(direction.x, 0, direction.z);
				//			direction.Normalize();

				//			var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, direction);
				//			AddCommand(ActiveUnit, castAbilityCommand);
				//		}
				//		else
				//		{
				//			var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
				//			AddCommand(ActiveUnit, approachPositionForAbilityCastCommand);
				//			var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
				//			ActiveUnit.CommandModule.Add(castAbilityCommand);
				//		}
				//		break;
				//	}
				//	case UnitTarget:
				//	case ResourceSourceTarget:
				//	{
				//		if (!commonActions.Select.WasPressedThisFrame())
				//		{
				//			return;
				//		}

				//		if (target == null)
				//		{
				//			return;
				//		}

				//		var approachEntityForAbilityCastCommand = new ApproachEntityForAbilityCastCommand(ActiveUnit, ActiveAbility, target);
				//		AddCommand(ActiveUnit, approachEntityForAbilityCastCommand);

				//		switch (target)
				//		{
				//			case UnitObsolete unit:
				//			{
				//				var castAbilityCommand = new CastAbilityCommand<UnitObsolete>(ActiveUnit, ActiveAbility, unit);
				//				ActiveUnit.CommandModule.Add(castAbilityCommand);
				//				break;
				//			}
				//			case ResourceSource resourceSource:
				//			{
				//				var castAbilityCommand = new CastAbilityCommand<ResourceSource>(ActiveUnit, ActiveAbility, resourceSource);
				//				ActiveUnit.CommandModule.Add(castAbilityCommand);
				//				break;
				//			}
				//		}

				//		break;
				//	}
				//}
			}
			else
			{
				var abilityActions = abilitiesActions.Get().actions;

				for (int i = 0; i < abilityActions.Count; ++i)
				{
					if (abilityActions[i].WasPressedThisFrame())
					{
						if (i < abilityModule.Abilities.Count)
						{
							Ability ability = abilityModule.Abilities[i];

							//TODO ECS
							//if (ability.Desc.ActiveOperation is not null)
							{
								//if (ActiveAbility == ability)
								//{
								//	Discard();
								//	return;
								//}

								//if (ActiveAbility is not null)
								//{
								//	Discard();
								//}

								AbilityCastError error = ability.CanBeCasted(entity);

								if (error is not AbilityCastError.None)
								{
									return;
								}

								//if (ability.Desc.Target is NoneTarget)
								{
									var commandModule = SystemAPI.ManagedAPI.GetComponent<CommandModule>(entity);
									//if (ability.Desc.Casting.Time == 0)
									//{
									//	var castImmediateAbilityCommand = new CastImmediateAbilityCommand(unit, ability);
									//	unit.CommandModule.Add(castImmediateAbilityCommand);
									//}
									//else
									{
										var command = new CastAbilityCommand<None>(entity, ability, None.Instance);
										AddCommand(ref state, commandModule, command);
									}
								}
								//else
								//{
								//	ActiveUnit = unit;
								//	ActiveAbility = ability;
								//}
							}
						}
					}
				}
			}

			void AddCommand(ref SystemState state, CommandModule commandModule, ICommand command)
			{
				if (!commonActions.AdditiveMode.IsPressed())
				{
					commandModule.Reset(ref state);
				}

				commandModule.Add(command);
			}

			void Discard()
			{
				abilityInput.InProcess = false;
			}
		}
	}
}
