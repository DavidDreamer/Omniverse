using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.Input
{
	public class Selector
	{
		public const int Capacity = 16;
		private const float SelectionBoxTreshold = 16;

		public HashSet<UnitObsolete> SelectedUnits { get; } = new();

		public bool HasSelection => SelectedUnits.Count > 0;

		public UnitObsolete SelectedUnit => SelectedUnits.ElementAt(SelectionIndex);

		public int SelectionIndex { get; private set; }

		private Player Player { get; }

		private InputActions.CommonActions CommonActions { get; set; }

		public Vector2 StartPosition { get; private set; }

		public Vector2 EndPosition { get; private set; }

		public bool InProcess { get; private set; }

		public Selector()
		{
			var inputSystemData = ECSUtils.GetSingletonManaged<InputSystemData>();
			CommonActions = inputSystemData.InputActions.Common;

			Player = ECSUtils.GetSingleton<Player>();
		}

		public void Tick(Camera camera, Mouse mouse, OmniverseEntity target)
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
					foreach (UnitObsolete unit in PhysicsService.GetEntitiesInScreenRect<UnitObsolete>(camera, StartPosition, EndPosition))
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

		private void TrySelectSingleTarget(OmniverseEntity target)
		{
			if (target == null)
			{
				return;
			}

			var unit = target as UnitObsolete;
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

		private void TrySelect(UnitObsolete unit)
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

		private void RemoveFromSelection(UnitObsolete unit)
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
