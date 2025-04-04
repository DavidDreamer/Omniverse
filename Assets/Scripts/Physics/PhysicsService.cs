using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Omniverse
{
	public static class PhysicsService
	{
		public static IEnumerable<DynamicEntity> GetEntitiesInSphere(EntityManager entityManager, DynamicEntity source, float radius, FactiousFilter filter)
		{
			var physicsWorld = ECSUtils.GetSingleton<PhysicsWorldSingleton>();
			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			Vector3 position = source.LocalTransform.ValueRO.Position;

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

					if (entityManager.HasAspect<DynamicEntity>(hit.Entity))
					{
						yield return entityManager.GetAspect<DynamicEntity>(hitEntity);
					}
				}
			}
		}

		public static DynamicEntity GetClosestEntity(EntityManager entityManager, DynamicEntity source, float radius, FactiousFilter filter)
		{
			var physicsWorld = ECSUtils.GetSingleton<PhysicsWorldSingleton>();
			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

			float3 position = source.LocalTransform.ValueRO.Position;

			var distanceHits = new NativeList<DistanceHit>();
			var collisionFilter = new CollisionFilter
			{
				BelongsTo = ~0u,
				CollidesWith = (uint)settings.HitboxLayerMask.value
			};

			if (physicsWorld.OverlapSphere(position, radius, ref distanceHits, collisionFilter))
			{
				float minDistance = float.MaxValue;
				DynamicEntity closestEntity = default;

				foreach (DistanceHit hit in distanceHits)
				{
					Entity hitEntity = hit.Entity;

					if (!entityManager.HasAspect<DynamicEntity>(hit.Entity))
					{
						continue;
					}

					DynamicEntity dynamicEntity = entityManager.GetAspect<DynamicEntity>(hitEntity);

					float distance = Vector3.SqrMagnitude(position - dynamicEntity.LocalTransform.ValueRO.Position);

					if (distance < minDistance)
					{
						minDistance = distance;
						closestEntity = dynamicEntity;
					}
				}

				return closestEntity;
			}

			return default;
		}

		public static IEnumerable<DynamicEntity> GetEntitiesInSector<TEntity>(
			EntityManager entityManager,
			DynamicEntity source,
			Vector3 forward,
			float radius,
			float angle,
			FactiousFilter filter)
		{
			float3 position = source.LocalTransform.ValueRO.Position;
			foreach (DynamicEntity entity in GetEntitiesInSphere(entityManager, source, radius, filter))
			{
				float3 direction = math.normalize(entity.LocalTransform.ValueRO.Position - position);

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

		public static IEnumerable<Entity> GetEntitiesInScreenRect(Camera camera, Vector3 start, Vector3 end)
		{
			Vector3 first = ScreenPointToWorldGround(camera, start);
			Vector3 second = ScreenPointToWorldGround(camera, end);

			Vector3 center = (first + second) / 2f;

			float width = Mathf.Abs(first.x - second.x) / 2f;
			const float height = 100f;
			float length = Mathf.Abs(first.z - second.z) / 2f;

			Vector3 halfExtens = new(width, height, length);

			var physicsWorldSingleton = ECSUtils.GetSingleton<PhysicsWorldSingleton>();
			var settings = ECSUtils.GetSingleton<PhysicsSettings>();

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
