using System;
using Omniverse.Abilities;
using Omniverse.Entities.Units;
using UnityEngine;
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
		private AbilityHandlerResolver AbilityHandlerResolver { get; set; }
		
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
			AbilityHandlerResolver.TryCastAbility(unit, ability);
		}
	}
}
