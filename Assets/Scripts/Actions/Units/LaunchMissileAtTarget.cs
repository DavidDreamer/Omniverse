using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchMissileAtTarget : Action<Unit, Unit>
	{
		[field: SerializeField]
		public MissileDesc Missile { get; private set; }

		public override void Perform(Unit actor, Unit target)
		{
			actor.SpawnMissile(Missile, target);
		}
	}
}