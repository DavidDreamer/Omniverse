using Omniverse.Abilities;
using Unity.Entities;
using Unity.Transforms;
using static InputActions;

namespace Omniverse.Input
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessAbilityInputSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			EntityManager entityManager = state.EntityManager;

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
			var dynamicEntity = state.EntityManager.GetAspect<DynamicEntity>(entity);
			var commandModule = SystemAPI.ManagedAPI.GetComponent<CommandModule>(entity);
			var transform = SystemAPI.GetComponent<LocalTransform>(entity);

			if (abilityInput.ValueRO.InProcess)
			{
				var target = entityManager.GetComponentObject<AbilityTarget>(abilityInput.ValueRO.Ability).Target;

				switch (target)
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

							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(dynamicEntity, abilityInput.ValueRO.Ability, direction);
							AddCommand(ref state, commandModule, castAbilityCommand);
						}
						else
						{
							var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(dynamicEntity, abilityInput.ValueRO.Ability, pointer.WorldPosition);
							AddCommand(ref state, commandModule, approachPositionForAbilityCastCommand);
							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(dynamicEntity, abilityInput.ValueRO.Ability, pointer.WorldPosition);
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

			var children = entityManager.GetBuffer<Child>(selection.Entity);

			int i = 0;

			foreach (Child child in children)
			{
				if (!entityManager.HasComponent<Ability>(child.Value))
				{
					continue;
				}

				if (i >= abilityActions.Count)
				{
					continue;
				}

				if (abilityActions[i].WasPressedThisFrame())
				{
					Entity abilityEntity = child.Value;
					//Ability ability = entityManager.GetComponentData<Ability>(abilityEntity);

					//if (ability.ActiveOperation is not null)
					{
						if (abilityInput.ValueRO.InProcess)
						{
							if (abilityInput.ValueRO.Ability == abilityEntity)
							{
								Discard();
								return;
							}
							else
							{
								Discard();
							}
						}

						AbilityCastError error = AbilityCastError.None; //ability.CanBeCasted(dynamicEntity);

						if (error is not AbilityCastError.None)
						{
							return;
						}

						var abilityTarget = entityManager.GetComponentObject<AbilityTarget>(abilityEntity);

						if (abilityTarget.Target is NoneTarget)
						{
							var casting = entityManager.GetComponentData<Casting>(abilityEntity);

							if (casting.Time == 0)
							{
								var command = new CastImmediateAbilityCommand(dynamicEntity, abilityEntity);
								commandModule.Add(command);
							}
							else
							{
								var command = new CastAbilityCommand<None>(dynamicEntity, abilityEntity, None.Instance);
								AddCommand(ref state, commandModule, command);
							}
						}
						else
						{
							abilityInput.ValueRW.Ability = abilityEntity;
						}
					}
				}

				i++;
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
				abilityInput.ValueRW.Ability = Entity.Null;
			}
		}
	}
}
