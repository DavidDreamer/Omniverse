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
		private Player Player { get; set; }

		[Inject]
		private Detector Detector { get; set; }

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
			Vector2 mousePosition = Mouse.current.position.value;
			Ray ray = camera.ScreenPointToRay(mousePosition);
			float deltaTime = Time.deltaTime;

			NavmeshUtils.GetNavMeshPositionFromCursor(ray, out Vector3 position);
			CursorWorldPosition = position;

			bool abilityInProcess = AbilityController.ActiveAbility is not null;

			if (abilityInProcess)
			{
				Detector.SetupForAbility(AbilityController.ActiveAbility);
			}
			else
			{
				Detector.SetDefaultDetectableType();
			}

			IFactious source = Selector.HasSelection ? Selector.SelectedUnit : Player;
			Detector.Tick(ray, source);

			if (abilityInProcess)
			{
				AbilityController.ProcessAbility(Detector.Target, CursorWorldPosition);
			}
			else
			{
				Selector.Tick(camera, mouse, Detector.Target);
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

				UnitController.Tick(Detector.Target, CursorWorldPosition);
			}

			if (!Selector.InProcess)
			{
				CameraController.Tick(mouse, deltaTime);
			}
		}
	}
}
