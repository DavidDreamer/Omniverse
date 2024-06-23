using System;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class AbilityInputListener: IInitializable, IDisposable
	{
		[Inject]
		private UnitSelector UnitSelector { get; set; }
		
		[Inject]
		private InputActions.AbilitiesActions AbilitiesActions { get; set; }
		
		[Inject]
		private AbilityController AbilityController { get; set; }
		
		[Inject]
		private PointAbilityController PointAbilityController { get; set; }
		
		[Inject]
		private UnitTargetController UnitTargetController { get; set; }
		
		[Inject]
		private TrajectoryAbilityController TrajectoryAbilityController { get; set; }
		
		public void Initialize()
		{
			foreach (InputAction inputAction in AbilitiesActions.Get().actions)
			{
				inputAction.started += OnAbilityInputActionStarted;
			}
		}

		public void Dispose()
		{
			foreach (InputAction inputAction in AbilitiesActions.Get().actions)
			{
				inputAction.started -= OnAbilityInputActionStarted;
			}
		}
		
		private void OnAbilityInputActionStarted(InputAction.CallbackContext context)
		{
			if (UnitSelector.HasSelection is false)
			{
				return;
			}

			Unit unit = UnitSelector.SelectedUnit.Unit;
			
			int abilityIndex = AbilitiesActions.Get().actions.IndexOf(i => i == context.action);

			if (abilityIndex > unit.Abilities.Count)
			{
				return;
			}
			
			Ability ability = unit.Abilities[abilityIndex];
			TryCastAbility(unit, ability);
		}
		
		private void TryCastAbility(Unit unit, Ability ability)
		{
			switch (ability.Desc.Target)
			{
				case NonTarget:
					AbilityController.TryCastAbility(unit, ability);
					break;
				case PointTarget:
				{
					if (PointAbilityController.InProcess)
					{
						PointAbilityController.Cancell();
					}
					else
					{
						PointAbilityController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}
					break;
				}
				case TrajectoryTarget:
				{
					if (TrajectoryAbilityController.InProcess)
					{
						TrajectoryAbilityController.Cancell();
					}
					else
					{
						TrajectoryAbilityController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}
					break;
				}
				case UnitTarget:
				{
					if (UnitTargetController.InProcess)
					{
						UnitTargetController.Cancell();
					}
					else
					{
						UnitTargetController.GetTargetAndCast(unit, ability).SuppressCancellationThrow();
					}
					break;
				}
			}
		}
	}
}
