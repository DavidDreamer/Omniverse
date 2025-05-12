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

		[field: SerializeField]
		public float Range { get; private set; }

		public void Perform(EntityManager commandBuffer, DynamicEntity actor, DynamicEntity target)
		{
			//TODO ECS
			//actor.SpawnMissile(Missile, target);
		}

		public void Perform(EntityManager entityManager, DynamicEntity actor, Vector3 target)
		{
			if (!entityManager.World.IsServer())
			{
				return;
			}

			var references = entityManager.GetSingleton<EntityReferences>();

			Entity fireball = entityManager.Instantiate(references.Fireball);

			float3 position = actor.LocalTransform.ValueRO.Position + new float3(0f, 1f, 0f) + (float3)target;
			var localTransform = entityManager.GetComponentData<LocalTransform>(fireball);
			localTransform.Position = position;
			entityManager.SetComponentData(fireball, localTransform);

			var missile = entityManager.GetComponentData<Missile>(fireball);
			missile.StartPosition = position;
			missile.Direction = target;
			entityManager.SetComponentData(fireball, missile);

			entityManager.SetComponentData(fireball, new Range
			{
				Value = Range
			});
		}
	}
}
