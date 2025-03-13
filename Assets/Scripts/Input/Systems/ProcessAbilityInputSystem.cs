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
			var abilities = SystemAPI.GetBuffer<Ability>(entity);

			var abilityActions = abilitiesActions.Get().actions;

			for (int i = 0; i < abilityActions.Count; ++i)
			{
				if (abilityActions[i].WasPressedThisFrame())
				{
					if (i < abilities.Length)
					{
						Ability ability = abilities[i];

						//if (ability.Desc.ActiveOperation is not null)
						//{
						//	if (ActiveAbility == ability)
						//	{
						//		Discard();
						//		return;
						//	}

						//	if (ActiveAbility is not null)
						//	{
						//		Discard();
						//	}

							//TODO ECS
							//AbilityCastError error = ability.CanBeCasted(unit);

							//if (error is not AbilityCastError.None)
							//{
							//	return;
							//}

							//if (ability.Desc.Target is NoneTarget)
							//{
							//	if (ability.Desc.Casting.Time == 0)
							//	{
							//		var castImmediateAbilityCommand = new CastImmediateAbilityCommand(unit, ability);
							//		unit.CommandModule.Add(castImmediateAbilityCommand);
							//	}
							//	else
							//	{
							//		var command = new CastAbilityCommand<None>(unit, ability, None.Instance);
							//		AddCommand(unit, command);
							//	}
							//}
							//else
							//{
							//	ActiveUnit = unit;
							//	ActiveAbility = ability;
							//}
						//}
					}
				}
			}

			//bool abilityInProcess = AbilityController.ActiveAbility is not null;

			//if (abilityInProcess)
			//{
			//	//AbilityController.ProcessAbility(entityDetector.Entity, CursorWorldPosition, additiveMode);
			//}

			void Discard()
			{
				abilityInput.InProcess = false;
			}
		}

		//public void ProcessAbility(OmniverseEntity target, Vector3? cursorWorldPosition, bool additiveMode)
		//{
		//	switch (ActiveAbility.Desc.Target)
		//	{
		//		case VectorTarget vectorTarget:
		//		{

		//			if (!CommonActions.Select.WasPressedThisFrame())
		//			{
		//				return;
		//			}

		//			if (!cursorWorldPosition.HasValue)
		//			{
		//				return;
		//			}

		//			if (vectorTarget.Mode is VectorTargetMode.Direction)
		//			{
		//				Vector3 direction = cursorWorldPosition.Value - ActiveUnit.transform.position;
		//				direction.Set(direction.x, 0, direction.z);
		//				direction.Normalize();

		//				var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, direction);
		//				AddCommand(ActiveUnit, castAbilityCommand);
		//			}
		//			else
		//			{
		//				var approachPositionForAbilityCastCommand = new ApproachPositionForAbilityCastCommand(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
		//				AddCommand(ActiveUnit, approachPositionForAbilityCastCommand);
		//				var castAbilityCommand = new CastAbilityCommand<Vector3>(ActiveUnit, ActiveAbility, cursorWorldPosition.Value);
		//				ActiveUnit.CommandModule.Add(castAbilityCommand);
		//			}
		//			break;
		//		}
		//		case UnitTarget:
		//		case ResourceSourceTarget:
		//		{
		//			if (!CommonActions.Select.WasPressedThisFrame())
		//			{
		//				return;
		//			}

		//			if (target == null)
		//			{
		//				return;
		//			}

		//			var approachEntityForAbilityCastCommand = new ApproachEntityForAbilityCastCommand(ActiveUnit, ActiveAbility, target);
		//			AddCommand(ActiveUnit, approachEntityForAbilityCastCommand);

		//			switch (target)
		//			{
		//				case UnitObsolete unit:
		//				{
		//					var castAbilityCommand = new CastAbilityCommand<UnitObsolete>(ActiveUnit, ActiveAbility, unit);
		//					ActiveUnit.CommandModule.Add(castAbilityCommand);
		//					break;
		//				}
		//				case ResourceSource resourceSource:
		//				{
		//					var castAbilityCommand = new CastAbilityCommand<ResourceSource>(ActiveUnit, ActiveAbility, resourceSource);
		//					ActiveUnit.CommandModule.Add(castAbilityCommand);
		//					break;
		//				}
		//			}

		//			break;
		//		}
		//	}

		//	Discard();
		//}

		//private void AddCommand(UnitObsolete unit, ICommand command)
		//{
		//	if (!CommonActions.AdditiveMode.IsPressed())
		//	{
		//		unit.CommandModule.Reset();
		//	}

		//	unit.CommandModule.Add(command);
		//}
	}
}
