using Unity.Entities;
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
			//TODO ECS
			//actor.SpawnMissile(Missile, target);
		}
	}
}
