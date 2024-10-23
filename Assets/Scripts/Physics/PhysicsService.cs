using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace Omniverse
{
	public class PhysicsService
	{
		private PhysicsSettings Settings { get; }

		private Collider[] Colliders { get; } = new Collider[128];

		public PhysicsService(PhysicsSettings settings)
		{
			Settings = settings;
		}

		public Entity GetEntity(Ray ray)
		{
			if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Settings.HitboxLayerMask))
			{
				return hitInfo.collider.GetComponentInParent<Entity>();
			}

			return null;
		}

		public IEnumerable<TEntity> GetEntitiesInSphere<TEntity>(Entity source, float radius, FactiousFilter filter) where TEntity : Entity
		{
			Vector3 sourcePosition = source.transform.position;

			int count = Physics.OverlapSphereNonAlloc(sourcePosition, radius, Colliders, Settings.HitboxLayerMask);

			for (int i = 0; i < count; ++i)
			{
				var entity = Colliders[i].GetComponentInParent<TEntity>();

				if (entity == null)
				{
					continue;
				}

				if (!filter.Match(source, entity))
				{
					continue;
				}

				yield return entity;
			}
		}

		public TEntity GetClosestEntity<TEntity>(Entity source, float radius, FactiousFilter filter) where TEntity : Entity
		{
			Vector3 sourcePosition = source.transform.position;

			int count = Physics.OverlapSphereNonAlloc(sourcePosition, radius, Colliders, Settings.HitboxLayerMask);

			float minDistance = float.MaxValue;
			TEntity closestEntity = null;

			for (int i = 0; i < count; ++i)
			{
				var entity = Colliders[i].GetComponentInParent<TEntity>();

				if (entity == null)
				{
					continue;
				}

				if (!filter.Match(source, entity))
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

		public IEnumerable<TEntity> GetEntitiesInSector<TEntity>(
			Entity source,
			Vector3 forward,
			float radius,
			float angle,
			FactiousFilter filter)
			where TEntity : Entity
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

		public IEnumerable<TEntity> GetEntitiesInScreenRect<TEntity>(Camera camera, Vector3 start, Vector3 end) where TEntity : Entity
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
