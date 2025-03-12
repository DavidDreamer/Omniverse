using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public static class PhysicsService
	{
		private static Collider[] Colliders { get; } = new Collider[128];

		public static OmniverseEntity GetEntity(Ray ray)
		{
			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, settings.HitboxLayerMask))
			{
				return hitInfo.collider.GetComponentInParent<OmniverseEntity>();
			}

			return null;
		}

		public static IEnumerable<TEntity> GetEntitiesInSphere<TEntity>(OmniverseEntity source, float radius, FactiousFilter filter) where TEntity : OmniverseEntity
		{
			Vector3 sourcePosition = source.transform.position;

			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			int count = Physics.OverlapSphereNonAlloc(sourcePosition, radius, Colliders, settings.HitboxLayerMask);

			for (int i = 0; i < count; ++i)
			{
				var entity = Colliders[i].GetComponentInParent<TEntity>();

				if (entity == null)
				{
					continue;
				}

				if (!filter.Match(source.FactionID, entity.FactionID))
				{
					continue;
				}

				yield return entity;
			}
		}

		public static TEntity GetClosestEntity<TEntity>(OmniverseEntity source, float radius, FactiousFilter filter) where TEntity : OmniverseEntity
		{
			Vector3 sourcePosition = source.transform.position;

			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			int count = Physics.OverlapSphereNonAlloc(sourcePosition, radius, Colliders, settings.HitboxLayerMask);

			float minDistance = float.MaxValue;
			TEntity closestEntity = null;

			for (int i = 0; i < count; ++i)
			{
				var entity = Colliders[i].GetComponentInParent<TEntity>();

				if (entity == null)
				{
					continue;
				}

				if (!filter.Match(source.FactionID, entity.FactionID))
				{
					continue;
				}

				float distance = Vector3.SqrMagnitude(sourcePosition - entity.transform.position);

				if (distance < minDistance)
				{
					minDistance = distance;
					closestEntity = entity;
				}
			}

			return closestEntity;
		}

		public static IEnumerable<TEntity> GetEntitiesInSector<TEntity>(
			OmniverseEntity source,
			Vector3 forward,
			float radius,
			float angle,
			FactiousFilter filter)
			where TEntity : OmniverseEntity
		{
			Vector3 position = source.transform.position;
			foreach (TEntity entity in GetEntitiesInSphere<TEntity>(source, radius, filter))
			{
				Vector3 direction = (entity.transform.position - position).normalized;

				float currentAngle = Vector3.Angle(direction, forward);

				if (currentAngle > angle)
				{
					continue;
				}

				yield return entity;
			}
		}

		public static Vector3 ScreenPointToWorldGround(Camera camera, Vector2 screenPosition)
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

		public static IEnumerable<TEntity> GetEntitiesInScreenRect<TEntity>(Camera camera, Vector3 start, Vector3 end) where TEntity : OmniverseEntity
		{
			Vector3 first = ScreenPointToWorldGround(camera, start);
			Vector3 second = ScreenPointToWorldGround(camera, end);

			Vector3 center = (first + second) / 2f;

			float width = Mathf.Abs(first.x - second.x) / 2f;
			const float height = 100f;
			float length = Mathf.Abs(first.z - second.z) / 2f;

			Vector3 halfExtens = new(width, height, length);

			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			int count = Physics.OverlapBoxNonAlloc(center, halfExtens, Colliders, Quaternion.identity, settings.HitboxLayerMask);

			for (int i = 0; i < count; ++i)
			{
				var entity = Colliders[i].GetComponentInParent<TEntity>();

				if (entity == null)
				{
					continue;
				}

				yield return entity;
			}
		}
	}
}
