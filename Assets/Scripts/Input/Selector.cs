using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Omniverse.Input
{
	public class Selector
	{
		public const int Capacity = 16;
		private const float SelectionBoxTreshold = 16;

		public HashSet<Unit> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;

		public Unit SelectedUnit => SelectedUnits.ElementAt(SelectionIndex);

		public int SelectionIndex { get; private set; }

		[Inject]
		private Player Player { get; set; }

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		[Inject]
		private PhysicsService PhysicsService { get; set; }

		public Vector2 StartPosition { get; private set; }

		public Vector2 EndPosition { get; private set; }

		public bool InProcess { get; private set; }

		public void Tick(Camera camera, Mouse mouse, Entity target)
		{
			InputAction selectAction = CommonActions.Select;

			if (selectAction.WasPressedThisFrame())
			{
				StartPosition = mouse.position.value;
				InProcess = true;
			}

			if (selectAction.IsPressed())
			{
				EndPosition = mouse.position.value;
			}

			if (InProcess && selectAction.WasReleasedThisFrame())
			{
				InProcess = false;

				bool additiveMode = CommonActions.AdditiveMode.IsPressed();

				if (!additiveMode)
				{
					ClearSelection();
				}

				if (Vector2.Distance(StartPosition, EndPosition) > SelectionBoxTreshold)
				{
					foreach (Unit unit in PhysicsService.GetEntitiesInScreenRect<Unit>(camera, StartPosition, EndPosition))
					{
						if (unit.FactionID != Player.FactionID)
						{
							continue;
						}

						TrySelect(unit);
					}
				}
				else
				{
					TrySelectSingleTarget(target);
				}
			}

			UpdateSelectionIndex();
		}

		private void TrySelectSingleTarget(Entity target)
		{
			if (target == null)
			{
				return;
			}

			var unit = target as Unit;
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
				TrySelect(unit);
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

		private void TrySelect(Unit unit)
		{
			if (SelectedUnits.Contains(unit))
			{
				return;
			}

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
