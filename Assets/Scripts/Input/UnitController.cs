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
								selectedUnit.Target = item;
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
				unit.MoveToPosition(position);
			}

			NavigationPointCreated?.Invoke(position);
		}
	}
}
