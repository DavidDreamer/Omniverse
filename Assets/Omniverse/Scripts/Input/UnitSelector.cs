using System.Collections.Generic;
using Omniverse.Entities.Items;
using Omniverse.Entities.Units;
using Omniverse.Entities.Units.Client;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	//TODO add limit
	public class UnitSelector: ILateTickable
	{
		public List<Unit> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;
		
		public Unit SelectedUnit => SelectedUnits[SelectionIndex];
		
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
			if (EntityDetector.Target == null)
			{
				return;
			}

			var unit = EntityDetector.Target as Unit;
			if (unit == null)
			{
				return;
			}
			
			bool isSelected = SelectedUnits.Contains(unit);

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

			UpdateDetectionFilter();
			UpdateSelectionIndex();

			void UpdateDetectionFilter()
			{
				if (HasSelection)
				{
					EntityDetector.AddToFilter<Item>();
				}
				else
				{
					EntityDetector.RemoveFromFilter<Item>();
				}
			}
			
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

		private void AddToSelection(Unit unit)
		{
			//TODO
			unit.GetComponentInChildren<UnitRenderer>().Selection.gameObject.SetActive(true);
			SelectedUnits.Add(unit);
		}

		private void RemoveFromSelection(Unit unit)
		{
			//TODO
			unit.GetComponentInChildren<UnitRenderer>().Selection.gameObject.SetActive(false);
			SelectedUnits.Remove(unit);
		}

		private void ClearSelection()
		{
			//TODO
			foreach (Unit unit in SelectedUnits)
			{
				unit.GetComponentInChildren<UnitRenderer>().Selection.gameObject.SetActive(false);
			}

			SelectedUnits.Clear();
			SelectionIndex = 0;
		}
	}
}
