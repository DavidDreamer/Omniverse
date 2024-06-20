using System.Collections.Generic;
using System.Linq;
using Omniverse.Units.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	//TODO add limit
	public class UnitSelector: ILateTickable
	{
		public List<UnitRenderer> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;
		
		public UnitRenderer SelectedUnit => SelectedUnits[SelectionIndex];
		
		public int SelectionIndex { get; private set; }
		
		private UnitSelectorConfig Config { get; }
		
		[Inject]
		private EntityDetector EntityDetector { get; set; }
		
		public UnitSelector(UnitSelectorConfig config)
		{
			Config = config;
		}
		
		public void LateTick()
		{
			if (EntityDetector.Entities.Count == 0)
			{
				return;
			}

			var unitRenderer = EntityDetector.Entities.First().GetComponentInChildren<UnitRenderer>();
			if (unitRenderer == null)
			{
				return;
			}
			
			bool isSelected = SelectedUnits.Contains(unitRenderer);

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
	}
}
