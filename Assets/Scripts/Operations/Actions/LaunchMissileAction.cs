using UnityEngine;

namespace Omniverse
{
	public class LaunchMissileAction : IAction<Entity>, IAction<Vector3>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public void Perform(Entity actor, Entity target)
		{
			actor.SpawnMissile(Missile, target);
		}

		public void Perform(Entity actor, Vector3 target)
		{
			actor.SpawnMissile(Missile, target);
		}
	}
}
