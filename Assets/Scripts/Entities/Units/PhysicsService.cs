using System.Collections.Generic;
using UnityEngine;

namespace Omniverse.Units
{
	public class PhysicsService
	{
		private PhysicsSettings Settings { get; }

		private Collider[] Colliders { get; } = new Collider[128];

		public PhysicsService(PhysicsSettings settings)
		{
			Settings = settings;
		}

		public IEnumerable<Unit> GetUnitsInSphere(
			Vector3 position,
			float radius)
		{
			int count = Physics.OverlapSphereNonAlloc(position, radius, Colliders, Settings.HitboxLayerMask);

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

		public IEnumerable<Unit> GetUnitsInSector(Vector3 position,
			Vector3 forward,
			float radius,
			float angle)
		{
			foreach (Unit unit in GetUnitsInSphere(position, radius))
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

		public Vector3 ScreenPointToWorldGround(Camera camera, Vector2 screenPosition)
		{
			Ray ray = camera.ScreenPointToRay(screenPosition);
			Vector3 worldPlaneNormal = Vector3.up;
			Vector3 projection = Vector3.ProjectOnPlane(ray.direction, worldPlaneNormal);
			float angle = Vector3.Angle(ray.direction, projection) * Mathf.Deg2Rad;
			float height = ray.origin.y;
			float distance = height / Mathf.Sin(angle);
			Vector3 point = ray.origin + distance * ray.direction;
			return point;
		}

		public IEnumerable<Unit> GetUnitsInScreenRect(Camera camera, Vector3 start, Vector3 end)
		{
			Vector3 first = ScreenPointToWorldGround(camera, start);
			Vector3 second = ScreenPointToWorldGround(camera, end);

			Vector3 center = (first + second) / 2f;

			float width = Mathf.Abs(first.x - second.x) / 2f;
			const float height = 100f;
			float length = Mathf.Abs(first.z - second.z) / 2f;

			Vector3 halfExtens = new(width, height, length);

			int count = Physics.OverlapBoxNonAlloc(center, halfExtens, Colliders, Quaternion.identity, Settings.HitboxLayerMask);

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
	}
}
