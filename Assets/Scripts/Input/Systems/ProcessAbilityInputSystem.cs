using Omniverse.Abilities;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using static InputActions;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessAbilityInputSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.EntityManager.CreateSingleton<AbilityInput>();
		}

		public void OnUpdate(ref SystemState state)
		{
			var pointer = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingleton<Selection>();
			var abilityInput = SystemAPI.GetSingletonRW<AbilityInput>();
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

			if (abilityInput.ValueRW.InProcess)
			{
				Ability ability = abilityModule.Abilities[abilityInput.ValueRW.AbilityIndex];

				switch (ability.Target)
				{
					case VectorTarget vectorTarget:
					{

						if (!commonActions.Select.WasPressedThisFrame())
						{
							return;
						}

						if (pointer.TargetType is not PointerTargetType.World)
						{
							return;
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
							return;
						}

						if (pointer.Entity == Entity.Null)
						{
							return;
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
							if (abilityInput.ValueRW.InProcess)
							{
								Discard();

								if (abilityInput.ValueRW.Entity == entity && abilityInput.ValueRW.AbilityIndex == i)
								{
									return;
								}
							}

							AbilityCastError error = ability.CanBeCasted(entity);

							if (error is not AbilityCastError.None)
							{
								return;
							}

							if (ability.Target is NoneTarget)
							{
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
							else
							{
								abilityInput.ValueRW.InProcess = true;
								abilityInput.ValueRW.Entity = entity;
								abilityInput.ValueRW.AbilityIndex = i;
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
				abilityInput.ValueRW.InProcess = false;
			}
		}
	}
}
