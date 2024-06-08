using System.Collections.Generic;
using Omniverse.Units.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Omniverse.Input
{
	//TODO add limit
	public class UnitSelector: ITickable
	{
		public List<UnitRenderer> FocusedUnits { get; } = new();

		public List<UnitRenderer> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;
		
		public UnitRenderer SelectedUnit => SelectedUnits[SelectionIndex];
		
		public int SelectionIndex { get; private set; }
		
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
				var unitRenderer = hitInfo.collider.GetComponentInChildren<UnitRenderer>();
				
				if (unitRenderer != null)
				{
					bool isSelected = SelectedUnits.Contains(unitRenderer);

					if (!isSelected)
					{
						AddFocus(unitRenderer);
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
								RemoveFromSelection(unitRenderer);
							}
						}
						else
						{
							AddToSelection(unitRenderer);
						}
					}
				}
			}

			UpdateSelectionIndex();
			
			void UpdateSelectionIndex()
			{
				if (Keyboard.current.tabKey.wasReleasedThisFrame)
				{
					SelectionIndex++;
					if (SelectionIndex >= SelectedUnits.Count)
					{
						SelectionIndex = 0;
					}
				}
			}
		}

		private void AddToSelection(UnitRenderer unitRenderer)
		{
			unitRenderer.Selection.gameObject.SetActive(true);
			SelectedUnits.Add(unitRenderer);
		}

		private void RemoveFromSelection(UnitRenderer unitRenderer)
		{
			unitRenderer.Selection.gameObject.SetActive(false);
			SelectedUnits.Remove(unitRenderer);
		}

		private void ClearSelection()
		{
			foreach (UnitRenderer unitRenderer in SelectedUnits)
			{
				unitRenderer.Selection.gameObject.SetActive(false);
			}

			SelectedUnits.Clear();
			SelectionIndex = 0;
		}
		
		private void AddFocus(UnitRenderer unitRenderer)
		{
			unitRenderer.Focus.gameObject.SetActive(true);
			FocusedUnits.Add(unitRenderer);
		}
		
		private void ClearFocus()
		{
			foreach (UnitRenderer unitRenderer in FocusedUnits)
			{
				unitRenderer.Focus.gameObject.SetActive(false);
			}

			FocusedUnits.Clear();
		}
	}
}
