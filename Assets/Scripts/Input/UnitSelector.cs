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

		private const float SelectionBoxTreshold = 16;
		private static object cam;

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
			InputAction selectAction = CommonActions.Select;

			if (selectAction.WasPressedThisFrame())
			{
				SelectionBoxStart = Mouse.current.position.value;
			}

			SelectionBoxInProcess = selectAction.IsPressed();

			if (selectAction.IsPressed())
			{
				SelectionBoxEnd = Mouse.current.position.value;
			}

			if (selectAction.WasReleasedThisFrame())
			{
				bool additiveMode = CommonActions.AdditiveMode.IsPressed();

				if (!additiveMode)
				{
					ClearSelection();
				}

				if (Vector2.Distance(SelectionBoxStart, SelectionBoxEnd) > SelectionBoxTreshold)
				{
					var cam = Camera.main;

					foreach (Unit unit in PhysicsHelper.GetUnitsInScreenRect(cam, SelectionBoxStart, SelectionBoxEnd))
					{
						AddToSelection(unit);
					}
				}	
				else
				{
					TrySelectSingleTarget();
				}
			}
		}

		private void TrySelectSingleTarget()
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

			if (isSelected)
			{
				RemoveFromSelection(unit);
			}
			else
			{
				AddToSelection(unit);
			}

			UpdateDetectionFilter();
			UpdateSelectionIndex();
		}

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
