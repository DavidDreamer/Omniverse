using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Omniverse.Camera
{
	public class UnitSelector: ITickable
	{
		public Dictionary<Unit, GameObject> HoveredUnits { get; } = new();

		public Dictionary<Unit, GameObject> SelectedUnits { get; } = new();

		private UnitSelectorConfig Config { get; }

		public UnitSelector(UnitSelectorConfig config)
		{
			Config = config;
		}

		public void Tick()
		{
			UnityEngine.Camera camera = UnityEngine.Camera.main;

			if (camera == null)
			{
				return;
			}

			foreach (var pair in HoveredUnits)
			{
				Object.Destroy(pair.Value);
			}

			HoveredUnits.Clear();

			Ray ray = camera.ScreenPointToRay(Mouse.current.position.value);

			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue))
			{
				var unitPreseter = hitInfo.collider.GetComponent<UnitPresenter>();
			
				if (unitPreseter != null)
				{
					var unit = unitPreseter.Unit;
					bool isSelected = SelectedUnits.ContainsKey(unit);

					if (!isSelected)
					{
						AddHover(unit);
					}

					bool additiveMode = Keyboard.current.shiftKey.isPressed;
					bool wasClicked = Mouse.current.leftButton.wasReleasedThisFrame;
					
					if (wasClicked)
					{
						if (!additiveMode)
						{
							ClearSelection();
						}
						
						if (isSelected)
						{
							if (additiveMode)
							{
								RemoveFromSelection(unit);
							}
						}
						else
						{
							AddToSelection(unit);
						}
					}
				}
			}
		}

		private void AddToSelection(Unit unit)
		{
			GameObject selection = Object.Instantiate(Config.SelectionPrefab, unit.Presenter.transform);
			SelectedUnits.Add(unit, selection);
		}

		private void RemoveFromSelection(Unit unit)
		{
			Object.Destroy(SelectedUnits[unit]);
			SelectedUnits.Remove(unit);
		}

		private void ClearSelection()
		{
			foreach (var pair in SelectedUnits)
			{
				Object.Destroy(pair.Value);
			}

			SelectedUnits.Clear();
		}
		
		private void AddHover(Unit unit)
		{
			GameObject hover = Object.Instantiate(Config.HoverPrefab, unit.Presenter.transform);
			HoveredUnits.Add(unit, hover);
		}

		private void RemoveHover(Unit unit)
		{
			Object.Destroy(HoveredUnits[unit]);
			HoveredUnits.Remove(unit);
		}
	}
}
