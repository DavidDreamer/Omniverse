using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public static class PhysicsService
	{
		public static IEnumerable<Entity> GetEntitiesInSphere(EntityManager entityManager, Entity source, float radius, FactiousFilter filter)
		{
			var physicsWorld = entityManager.GetSingleton<PhysicsWorldSingleton>();
			var settings = entityManager.GetSingleton<PhysicsSettings>();

			var localTransform = entityManager.GetComponentData<LocalTransform>(source);
			Vector3 position = localTransform.Position;

			var distanceHits = new NativeList<DistanceHit>();
			var collisionFilter = new CollisionFilter
			{
				BelongsTo = ~0u,
				CollidesWith = (uint)settings.HitboxLayerMask.value
			};

			if (physicsWorld.OverlapSphere(position, radius, ref distanceHits, collisionFilter))
			{
				foreach (DistanceHit hit in distanceHits)
				{
					Entity hitEntity = hit.Entity;

					if (entityManager.HasComponent<Unit>(hit.Entity))
					{
						yield return hitEntity;
					}
				}
			}
		}

		public static Entity GetClosestEntity(EntityManager entityManager, Entity source, float radius, FactiousFilter filter)
		{
			var physicsWorld = entityManager.GetSingleton<PhysicsWorldSingleton>();
			var settings = entityManager.GetSingleton<PhysicsSettings>();

			var sourceLocalTransform = entityManager.GetComponentData<LocalTransform>(source);
			float3 sourcePosition = sourceLocalTransform.Position;

			var distanceHits = new NativeList<DistanceHit>();
			var collisionFilter = new CollisionFilter
			{
				BelongsTo = ~0u,
				CollidesWith = (uint)settings.HitboxLayerMask.value
			};

			if (physicsWorld.OverlapSphere(sourcePosition, radius, ref distanceHits, collisionFilter))
			{
				float minDistance = float.MaxValue;
				Entity closestEntity = default;

				foreach (DistanceHit hit in distanceHits)
				{
					Entity entity = hit.Entity;

					if (!entityManager.HasComponent<Unit>(hit.Entity))
					{
						continue;
					}

					var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
					float3 position = localTransform.Position;

					float distance = Vector3.SqrMagnitude(sourcePosition - position);

					if (distance < minDistance)
					{
						minDistance = distance;
						closestEntity = entity;
					}
				}

				return closestEntity;
			}

			return default;
		}

		public static IEnumerable<Entity> GetEntitiesInSector<TEntity>(
			EntityManager entityManager,
			Entity source,
			Vector3 forward,
			float radius,
			float angle,
			FactiousFilter filter)
		{
			var sourceLocalTransform = entityManager.GetComponentData<LocalTransform>(source);
			float3 sourcePosition = sourceLocalTransform.Position;

			foreach (Entity entity in GetEntitiesInSphere(entityManager, source, radius, filter))
			{
				var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
				float3 position = localTransform.Position;

				float3 direction = math.normalize(position - sourcePosition);

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
			UnityEngine.Ray ray = camera.ScreenPointToRay(screenPosition);
			Vector3 worldPlaneNormal = Vector3.up;
			Vector3 projection = Vector3.ProjectOnPlane(ray.direction, worldPlaneNormal);
			float angle = Vector3.Angle(ray.direction, projection) * Mathf.Deg2Rad;
			float height = ray.origin.y;
			float distance = height / Mathf.Sin(angle);
			Vector3 point = ray.origin + distance * ray.direction;
			return point;
		}

		public static IEnumerable<Entity> GetEntitiesInScreenRect(EntityManager entityManager, Camera camera, Vector3 start, Vector3 end)
		{
			Vector3 first = ScreenPointToWorldGround(camera, start);
			Vector3 second = ScreenPointToWorldGround(camera, end);

			Vector3 center = (first + second) / 2f;

			float width = Mathf.Abs(first.x - second.x) / 2f;
			const float height = 100f;
			float length = Mathf.Abs(first.z - second.z) / 2f;

			Vector3 halfExtens = new(width, height, length);

			var physicsWorldSingleton = entityManager.GetSingleton<PhysicsWorldSingleton>();
			var settings = entityManager.GetSingleton<PhysicsSettings>();

			var hits = new NativeList<DistanceHit>(Allocator.Temp);
			var filter = new CollisionFilter()
			{
				BelongsTo = ~0u,
				CollidesWith = (uint)settings.HitboxLayerMask.value,
			};

			physicsWorldSingleton.OverlapBox(center, Quaternion.identity, halfExtens, ref hits, filter);

			foreach (DistanceHit hit in hits)
			{
				yield return hit.Entity;
			}

			hits.Dispose();
		}
	}
}
