using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class UnitSelector: ITickable
	{
		public List<Unit> FocusedUnits { get; } = new();

		public List<Unit> SelectedUnits { get; } = new();

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

			ClearFocus();

			Ray ray = camera.ScreenPointToRay(Mouse.current.position.value);

			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue))
			{
				var unitPreseter = hitInfo.collider.GetComponentInParent<UnitPresenter>();
			
				if (unitPreseter != null)
				{
					Unit unit = unitPreseter.Unit;
					bool isSelected = SelectedUnits.Contains(unit);

					if (!isSelected)
					{
						AddFocus(unit);
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
			unit.Presenter.Selection.SetActive(true);
			SelectedUnits.Add(unit);
		}

		private void RemoveFromSelection(Unit unit)
		{
			unit.Presenter.Selection.SetActive(false);
			SelectedUnits.Remove(unit);
		}

		private void ClearSelection()
		{
			foreach (Unit unit in SelectedUnits)
			{
				unit.Presenter.Selection.SetActive(false);
			}

			SelectedUnits.Clear();
		}
		
		private void AddFocus(Unit unit)
		{
			unit.Presenter.Focus.SetActive(true);
			FocusedUnits.Add(unit);
		}
		
		private void ClearFocus()
		{
			foreach (Unit unit in FocusedUnits)
			{
				unit.Presenter.Focus.SetActive(false);
			}

			FocusedUnits.Clear();
		}
	}
}
