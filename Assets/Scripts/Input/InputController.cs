using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class InputController : ILateTickable
	{
		private InputActions.CommonActions CommonActions { get; set; }

		private InputActions.AbilitiesActions AbilitiesActions { get; set; }

		private Player Player { get; }

		[Inject]
		private Detector Detector { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		[Inject]
		private UnitController UnitController { get; set; }

		[Inject]
		private AbilityController AbilityController { get; set; }

		public Vector3? CursorWorldPosition { get; private set; }

		private InputController()
		{
			var inputSystemData = ECSUtils.GetSingletonManaged<InputSystemData>();
			CommonActions = inputSystemData.InputActions.Common;
			AbilitiesActions = inputSystemData.InputActions.Abilities;

			Player = ECSUtils.GetSingleton<Player>();
		}

		public void LateTick()
		{
			Camera camera = Camera.main;
			Mouse mouse = Mouse.current;
			Vector2 mousePosition = Mouse.current.position.value;
			Ray ray = camera.ScreenPointToRay(mousePosition);
			float deltaTime = Time.deltaTime;
			bool additiveMode = CommonActions.AdditiveMode.IsPressed();

			bool mouseIsOverUI = EventSystem.current.IsPointerOverGameObject();

			if (mouseIsOverUI)
			{
				CursorWorldPosition = null;
			}
			else
			{
				if (NavmeshUtils.GetNavMeshPositionFromCursor(ray, out Vector3 position))
				{
					CursorWorldPosition = position;
				}
				else
				{
					CursorWorldPosition = null;
				}
			}

			bool abilityInProcess = AbilityController.ActiveAbility is not null;

			if (abilityInProcess)
			{
				Detector.SetupForAbility(AbilityController.ActiveAbility);
			}
			else
			{
				Detector.SetDefaultDetectableType();
			}

			int sourceFactionID = Selector.HasSelection ? Selector.SelectedUnit.FactionID : Player.FactionID;
			Detector.Tick(ray, sourceFactionID);

			if (abilityInProcess)
			{
				AbilityController.ProcessAbility(Detector.Target, CursorWorldPosition, additiveMode);
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
						if (i < unit.Abilities.Count)
						{
							Ability ability = unit.Abilities[i];

							if (ability.Desc.ActiveOperation is not null)
							{
								AbilityController.Process(unit, ability);
							}
						}
					}
				}

				UnitController.Tick(Detector.Target, CursorWorldPosition);
			}

			if (!Selector.InProcess)
			{
				Object.FindFirstObjectByType<CameraController>().Tick(mouse, deltaTime);
			}
		}
	}
}
