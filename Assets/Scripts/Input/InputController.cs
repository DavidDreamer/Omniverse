using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class InputController : ILateTickable
	{
		[Inject]
		private InputActions.AbilitiesActions AbilitiesActions { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		[Inject]
		private CameraController CameraController { get; set; }

		[Inject]
		private UnitController UnitController { get; set; }

		[Inject]
		private AbilityController AbilityController { get; set; }

		public Vector3 CursorWorldPosition { get; private set; }

		public void LateTick()
		{
			Camera camera = Camera.main;
			Mouse mouse = Mouse.current;

			NavmeshUtils.GetNavMeshPositionFromCursor(out Vector3 position);
			CursorWorldPosition = position;

			if (AbilityController.ActiveAbility is null)
			{
				Selector.Tick(camera, mouse);
			}
			else
			{
				AbilityController.ProcessAbility(CursorWorldPosition);
			}

			if (Selector.HasSelection)
			{
				var unit = Selector.SelectedUnit;

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

			if (!Selector.InProcess)
			{
				CameraController.Tick();
			}
		}
	}
}
