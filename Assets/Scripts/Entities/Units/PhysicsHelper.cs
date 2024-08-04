using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Units
{
	public static class PhysicsHelper
	{
		private static Collider[] Colliders { get; } = new Collider[128];

		public static IEnumerable<Unit> GetUnitsInSphere(
			Vector3 position,
			float radius,
			LayerMask layerMask)
		{
			int count = Physics.OverlapSphereNonAlloc(position, radius, Colliders, layerMask.value);

			for (int i = 0; i < count; ++i)
			{
				var unit = Colliders[i].GetComponentInParent<Unit>();

				if (unit == null)
				{
					continue;
				}

				yield return unit;
			}
		}

		public static IEnumerable<Unit> GetUnitsInSector(Vector3 position,
			Vector3 forward,
			float radius,
			float angle,
			LayerMask layerMask)
		{
			foreach (Unit unit in GetUnitsInSphere(position, radius, layerMask))
			{
				Vector3 direction = (unit.transform.position - position).normalized;

				float currentAngle = Vector3.Angle(direction, forward);

				if (currentAngle > angle)
				{
					continue;
				}

				yield return unit;
			}
		}
	}
}
