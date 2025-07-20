using Omniverse.Abilities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static InputActions;

namespace Omniverse.Input
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	[UpdateAfter(typeof(SelectEntitySystem))]
	public partial struct ProcessAbilityInputSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			EntityManager entityManager = state.EntityManager;

			var pointer = SystemAPI.GetSingleton<Pointer>();
			var selection = SystemAPI.GetSingleton<Selection>();
			var selectionRW = SystemAPI.GetSingletonRW<Selection>();
			var builder = SystemAPI.GetSingletonRW<Builder>();
			var inputSystemData = SystemAPI.ManagedAPI.GetSingleton<InputSystemData>();

			CommonActions commonActions = inputSystemData.InputActions.Common;
			AbilitiesActions abilitiesActions = inputSystemData.InputActions.Abilities;
			bool additiveMode = commonActions.AdditiveMode.IsPressed();

			if (!selection.HasSelection)
			{
				return;
			}

			var entity = selection.Entity;

			if (entityManager.HasComponent<Building>(entity))
			{
				return;
			}

			var dynamicEntity = SystemAPI.GetAspect<DynamicEntity>(entity);
			var commandModule = SystemAPI.ManagedAPI.GetComponent<CommandModule>(entity);
			var transform = SystemAPI.GetComponent<LocalTransform>(entity);

			if (selection.AbilityInProcess)
			{
				var target = entityManager.GetComponentObject<AbilityTarget>(selectionRW.ValueRO.Ability).Target;

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

							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(dynamicEntity.Entity, selection.Ability, direction);
							AddCommand(ref state, commandModule, castAbilityCommand);
						}
						else
						{
							var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(dynamicEntity.Entity, selection.Ability, pointer.WorldPosition);
							AddCommand(ref state, commandModule, approachPositionForAbilityCastCommand);
							var castAbilityCommand = new CastAbilityCommand<UnityEngine.Vector3>(dynamicEntity.Entity, selection.Ability, pointer.WorldPosition);
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

			var abilityReferences = entityManager.GetBuffer<AbilityReference>(selection.Entity);

			int i = 0;

			foreach (AbilityReference reference in abilityReferences)
			{
				if (i >= abilityActions.Count)
				{
					continue;
				}

				if (abilityActions[i].WasPressedThisFrame())
				{
					Entity abilityEntity = reference.Entity;

					//if (ability.ActiveOperation is not null)
					{
						if (selection.AbilityInProcess)
						{
							if (selection.Ability == abilityEntity)
							{
								Discard();
								return;
							}
							else
							{
								Discard();
							}
						}

						AbilityCastError error = abilityEntity.CanBeCasted(entityManager);

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
								var input = entityManager.GetComponentData<AbilityInput>(abilityEntity);
								input.Cast.Set();
								entityManager.SetComponentData(abilityEntity, input);
							}
							else
							{
								var command = new CastAbilityCommand<None>(dynamicEntity.Entity, abilityEntity, None.Instance);
								AddCommand(ref state, commandModule, command);
							}
						}
						else
						{
							selectionRW.ValueRW.Ability = abilityEntity;
						}
					}
				}

				i++;
			}

			if (abilitiesActions.Build.WasPressedThisFrame())
			{
				if (builder.ValueRW.BuildingDesc == null)
				{
					var buildAbility = SystemAPI.GetComponent<BuildAbility>(dynamicEntity.Entity);
					builder.ValueRW.BuildingDesc = buildAbility.Desc.Value.Building;
				}
				else
				{
					builder.ValueRW.BuildingDesc = null;
				}
			}

			if (builder.ValueRW.BuildingDesc != null && commonActions.Select.WasPressedThisFrame())
			{
				var commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

				var buildAbility = SystemAPI.GetComponent<BuildAbility>(dynamicEntity.Entity);
				var faction = SystemAPI.GetComponent<Faction>(dynamicEntity.Entity);

				var data = new BuildOperationData
				{
					Desc = buildAbility.Desc.Value.Building,
					Entity = buildAbility.Building,
					LocalTransform = new LocalTransform()
					{
						Position = pointer.CellPosiiton,
						Rotation = quaternion.identity,
						Scale = 1
					},
					Faction = faction
				};

				BuildingUtils.Build(commandBuffer, data);

				builder.ValueRW.BuildingDesc = null;
				commandBuffer.Playback(entityManager);
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
				selectionRW.ValueRW.Ability = Entity.Null;
			}
		}
	}
}
