using UnityEngine;

namespace Omniverse
{
	public class LaunchMissileAction : IAction<OmniverseEntity>, IAction<Vector3>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public void Perform(OmniverseEntity actor, OmniverseEntity target)
		{
			actor.SpawnMissile(Missile, target);
		}

		public void Perform(OmniverseEntity actor, Vector3 target)
		{
			actor.SpawnMissile(Missile, target);
		}
	}
}
