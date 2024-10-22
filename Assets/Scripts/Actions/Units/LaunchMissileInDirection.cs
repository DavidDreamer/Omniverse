using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchMissileInDirection : Action<Unit, Vector3>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public override void Perform(Unit actor, Vector3 target)
		{
			actor.SpawnMissile(Missile, target);
		}
	}
}