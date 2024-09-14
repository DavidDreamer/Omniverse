using Omniverse.Abilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class InputController : ILateTickable
	{
		[Inject]
		private InputActions.AbilitiesActions AbilitiesActions { get; set; }

		[Inject]
		private UnitSelector UnitSelector { get; set; }

		[Inject]
		private UnitController UnitController { get; set; }

		[Inject]
		private AbilityController AbilityController { get; set; }

		public Vector3 CursorWorldPosition { get; private set; }

		public void LateTick()
		{
			NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 position);
			CursorWorldPosition = position;

			if (AbilityController.ActiveAbility is null)
			{
				UnitSelector.Tick();
			}
			else
			{
				AbilityController.ProcessAbility(CursorWorldPosition);
			}

			if (UnitSelector.HasSelection)
			{
				var unit = UnitSelector.SelectedUnit;

				var abilityActions = AbilitiesActions.Get().actions;
				for (int i = 0; i < abilityActions.Count; ++i)
				{
					if (abilityActions[i].WasPressedThisFrame())
					{
						if (i <= unit.Abilities.Count)
						{
							Ability ability = unit.Abilities[i];
							AbilityController.Process(unit, ability);
						}
					}
				}

				UnitController.Tick();
			}
		}
	}
}
