using System.Collections.Generic;
using UnityEngine;
using Omniverse.Items;
using Omniverse.Units;
using UnityEngine.InputSystem;
using VContainer;

namespace Omniverse.Input
{
	public class UnitSelector
	{
		public const int Capacity = 16;

		public List<Unit> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;

		public Unit SelectedUnit => SelectedUnits[SelectionIndex];

		public int SelectionIndex { get; private set; }

		[Inject]
		private EntityDetector EntityDetector { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		public Vector2 SelectionBoxStart { get; private set; }

		public Vector2 SelectionBoxEnd { get; private set; }

		public bool SelectionBoxInProcess { get; private set; }

		public void Tick()
		{
			SelectionBoxInProcess = CommonActions.Select.IsPressed();

			if (CommonActions.Select.WasPressedThisFrame())
			{
				SelectionBoxStart = Mouse.current.position.value;
			}

			if (CommonActions.Select.IsPressed())
			{
				SelectionBoxEnd = Mouse.current.position.value;
			}

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

			bool additiveMode = CommonActions.AdditiveMode.IsPressed();
			bool wasClicked = CommonActions.Select.WasPerformedThisFrame();

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
				if (CommonActions.NextSelectionTarget.WasReleasedThisFrame())
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
			if (SelectedUnits.Count == Capacity)
			{
				return;
			}

			SelectedUnits.Add(unit);
		}

		private void RemoveFromSelection(Unit unit)
		{
			SelectedUnits.Remove(unit);
		}

		private void ClearSelection()
		{
			SelectedUnits.Clear();
			SelectionIndex = 0;
		}
	}
}
