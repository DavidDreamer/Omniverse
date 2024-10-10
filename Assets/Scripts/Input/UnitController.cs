using System;
using Omniverse.Items;
using Omniverse.Units;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Omniverse.Input
{
	public class UnitController
	{
		public event Action<Vector3> NavigationPointCreated;

		[Inject]
		private InputActions.CommonActions CommonActions { get; set; }

		[Inject]
		private Selector Selector { get; set; }

		public void Tick(Entity target, Vector3? position)
		{
			if (CommonActions.Command.WasReleasedThisFrame())
			{
				if (target != null)
				{
					switch (target)
					{
						case Unit unit:
							foreach (Unit selectedUnit in Selector.SelectedUnits)
							{
								if (selectedUnit != unit)
								{
									selectedUnit.Target = unit;
								}
							}
							break;
						case Item item:
							foreach (Unit selectedUnit in Selector.SelectedUnits)
							{
								var command = new PickUpItemCommand(selectedUnit, item);
								AddCommand(selectedUnit, command);
							}
							break;
					}
				}
				else
				{
					if (position.HasValue)
					{
						CreateNavigationPoint(position.Value);
					}
				}
			}
			else if (CommonActions.Stop.WasPerformedThisFrame())
			{
				foreach (Unit unit in Selector.SelectedUnits)
				{
					unit.ClearCommands();
				}
			}
		}

		public void ProcessNavigationPoint(Vector3 position)
		{
			if (NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, float.MaxValue, 1))
			{
				CreateNavigationPoint(navMeshHit.position);
			}
		}

		private void CreateNavigationPoint(Vector3 position)
		{
			foreach (Unit unit in Selector.SelectedUnits)
			{
				MoveCommand moveCommand = new(unit, position);
				AddCommand(unit, moveCommand);
			}

			NavigationPointCreated?.Invoke(position);
		}

		private void AddCommand(Unit unit, ICommand command)
		{
			bool forced = !CommonActions.AdditiveMode.IsPressed();
			unit.AddCommand(command, forced);
		}
	}
}
