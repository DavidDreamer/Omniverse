using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchHomingProjectile : ScriptableObject, IAction<Unit, Unit>
	{
		[field: SerializeField]
		public HomingProjectileDesc Projectile { get; private set; }

		public void Perform(Unit actor, Unit target)
		{
			actor.SpawnProjectile(Projectile, target);
		}
	}
}