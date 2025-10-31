using Omniverse.Abilities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
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

			if (!SystemAPI.HasComponent<GhostOwnerIsLocal>(entity))
			{
				return;
			}

			if (entityManager.HasComponent<Building>(entity))
			{
				return;
			}

			var transform = SystemAPI.GetComponent<LocalTransform>(entity);
			var abilityBuffer = entityManager.GetBuffer<Ability>(selection.Entity);

			if (selection.AbilityInProcess)
			{
				Ability ability = abilityBuffer[selection.AbilityIndex];

				switch (ability.Desc.Value.Target)
				{
					case Target.Vector:
					{
						if (!commonActions.Select.WasPressedThisFrame())
						{
							break;
						}

						if (pointer.TargetType is not PointerTargetType.World)
						{
							break;
						}

						UnityEngine.Vector3 direction = pointer.WorldPosition - transform.Position;
						direction.Set(direction.x, 0, direction.z);
						direction.Normalize();

						//TODO CASTING

						Discard();
						break;
					}
					case Target.Unit:
					case Target.ResourceSource:
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

			for (int i = 0; i < abilityBuffer.Length; i++)
			{
				if (i >= abilityActions.Count)
				{
					continue;
				}

				Ability ability = abilityBuffer[i];

				if (abilityActions[i].WasPressedThisFrame())
				{
					//if (ability.ActiveOperation is not null)
					{
						if (selection.AbilityInProcess)
						{
							if (selection.AbilityIndex == i)
							{
								Discard();
								return;
							}
							else
							{
								Discard();
							}
						}

						AbilityCastError error = ability.CanBeCasted(selection.Entity, entityManager);

						if (error is not AbilityCastError.None)
						{
							return;
						}

						if (ability.Desc.Value.Target is Target.None)
						{
							if (ability.Casting.Time == 0)
							{
								var abilityInput = entityManager.GetComponentData<AbilityInput>(selection.Entity);
								abilityInput.AbilityIndex = i;
								abilityInput.Cast.Set();
								entityManager.SetComponentData(selection.Entity, abilityInput);
							}
							else
							{
								//TODO CASTING
							}
						}
						else
						{
							selectionRW.ValueRW.AbilityInProcess = true;
							selectionRW.ValueRW.AbilityIndex = i;
						}
					}
				}
			}

			if (abilitiesActions.Build.WasPressedThisFrame())
			{
				if (builder.ValueRW.Building == Entity.Null)
				{
					//TODO HARDCODE
					var building = SystemAPI.GetBuffer<Blueprint>(entity)[0].Building;
					builder.ValueRW.Building = building;
				}
				else
				{
					builder.ValueRW.Building = Entity.Null;
				}
			}

			if (builder.ValueRW.Building != Entity.Null && commonActions.Select.WasPressedThisFrame())
			{
				var commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

				var faction = SystemAPI.GetComponent<Faction>(entity);
				var buildInput = SystemAPI.GetComponentRW<BuildInput>(entity);

				//TODO HARDCODE
				buildInput.ValueRW.BlueprintIndex = 0;
				buildInput.ValueRW.LocalTransform = new LocalTransform()
				{
					Position = pointer.CellPosiiton,
					Rotation = quaternion.identity,
					Scale = 1
				};
				buildInput.ValueRW.Faction = faction.ID;
				buildInput.ValueRW.Event.Set();

				builder.ValueRW.Building = Entity.Null;
				commandBuffer.Playback(entityManager);
			}

			void Discard()
			{
				selectionRW.ValueRW.AbilityInProcess = false;
			}
		}
	}
}
