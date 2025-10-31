using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class LaunchMissileAction : IAction<Entity>, IAction<Vector3>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		[field: SerializeField]
		public float Range { get; private set; }

		public void Perform(EntityManager commandBuffer, Entity actor, Entity target)
		{
			//TODO ECS
			//actor.SpawnMissile(Missile, target);
		}

		public void Perform(EntityManager entityManager, Entity actor, Vector3 target)
		{
			var references = entityManager.GetSingleton<EntityReferences>();

			Entity fireball = entityManager.Instantiate(references.Fireball);

			var actorLocalTransform = entityManager.GetComponentData<LocalTransform>(actor);

			float3 position = actorLocalTransform.Position + new float3(0f, 1f, 0f) + (float3)target;
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
