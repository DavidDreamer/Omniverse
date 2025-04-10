using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class LaunchMissileAction : IAction<DynamicEntity>, IAction<Vector3>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public void Perform(EntityManager entityManager, DynamicEntity actor, DynamicEntity target)
		{
			//TODO ECS
			//actor.SpawnMissile(Missile, target);
		}

		public void Perform(EntityManager entityManager, DynamicEntity actor, Vector3 target)
		{
			var query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(EntityReferences) });
			var singleton = query.GetSingleton<EntityReferences>();
			query.Dispose();

			if (entityManager.World.IsServer())
			{
				Entity fireball = entityManager.Instantiate(singleton.Fireball);

				float3 position = actor.LocalTransform.ValueRO.Position + new float3(0f, 1f, 0f) + (float3)target;
				var localTransform = entityManager.GetComponentData<LocalTransform>(fireball);
				localTransform.Position = position;
				entityManager.SetComponentData(fireball, localTransform);

				var missile = entityManager.GetComponentData<Missile>(fireball);
				missile.Direction = target;
				entityManager.SetComponentData(fireball, missile);
			}
		}
	}
}
