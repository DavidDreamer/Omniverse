using Omniverse.Abilities;
using Unity.Entities;
using Unity.Transforms;
using static InputActions;

namespace Omniverse.Input
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
	public partial struct ProcessAbilityInputSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			var pointer = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingleton<Selection>();
			var abilityInput = SystemAPI.ManagedAPI.GetSingleton<AbilityInput>();
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
			var commandModule = SystemAPI.ManagedAPI.GetComponent<CommandModule>(entity);
			var transform = SystemAPI.GetComponent<LocalTransform>(entity);

			if (abilityInput.InProcess)
			{
				Ability ability = abilityInput.Ability;

				switch (ability.Target)
				{
					case VectorTarget vectorTarget:
					{
						if (!commonActions.Select.WasPressedThisFrame())
						{
							break;
						}

						if (pointer.TargetType is not PointerTargetType.World)
						{
							break;
						}

						if (vectorTarget.Mode is VectorTargetMode.Direction)
						{
							UnityEngine.Vector3 direction = pointer.WorldPosition - transform.Position;
							direction.Set(direction.x, 0, direction.z);
							direction.Normalize();

							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(entity, ability, direction);
							AddCommand(ref state, commandModule, castAbilityCommand);
						}
						else
						{
							var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(entity, ability, pointer.WorldPosition);
							AddCommand(ref state, commandModule, approachPositionForAbilityCastCommand);
							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(entity, ability, pointer.WorldPosition);
							AddCommand(ref state, commandModule, castAbilityCommand);
						}

						Discard();
						break;
					}
					case UnitTarget:
					case ResourceSourceTarget:
					{
						if (!commonActions.Select.WasPressedThisFrame())
						{
							break;
						}

						if (pointer.Entity == Entity.Null)
						{
							break;
						}

						//TODO ECS
						//var approachEntityForAbilityCastCommand = new ApproachEntityForAbilityCastCommand(ActiveUnit, ActiveAbility, target);
						//AddCommand(ref state, commandModule, approachEntityForAbilityCastCommand);

						//switch (target)
						//{
						//	case UnitObsolete unit:
						//	{
						//		var castAbilityCommand = new CastAbilityCommand<UnitObsolete>(ActiveUnit, ActiveAbility, unit);
						//		ActiveUnit.CommandModule.Add(castAbilityCommand);
						//		break;
						//	}
						//	case ResourceSource resourceSource:
						//	{
						//		var castAbilityCommand = new CastAbilityCommand<ResourceSource>(ActiveUnit, ActiveAbility, resourceSource);
						//		ActiveUnit.CommandModule.Add(castAbilityCommand);
						//		break;
						//	}
						//}

						break;
					}
				}
			}

			var abilityActions = abilitiesActions.Get().actions;

			for (int i = 0; i < abilityActions.Count; ++i)
			{
				if (abilityActions[i].WasPressedThisFrame())
				{
					if (i < abilityModule.Abilities.Count)
					{
						Ability ability = abilityModule.Abilities[i];

						//if (ability.ActiveOperation is not null)
						{
							if (abilityInput.InProcess)
							{
								if (abilityInput.Entity == entity && abilityInput.Ability == ability)
								{
									Discard();
									return;
								}
								else
								{
									Discard();
								}
							}

							AbilityCastError error = ability.CanBeCasted(entity);

							if (error is not AbilityCastError.None)
							{
								return;
							}

							if (ability.Target is NoneTarget)
							{
								if (ability.Casting.Time == 0)
								{
									var command = new CastImmediateAbilityCommand(entity, ability);
									commandModule.Add(command);
								}
								else
								{
									var command = new CastAbilityCommand<None>(entity, ability, None.Instance);
									AddCommand(ref state, commandModule, command);
								}
							}
							else
							{
								abilityInput.Entity = entity;
								abilityInput.Ability = ability;
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
				abilityInput.Ability = null;
			}
		}
	}
}
