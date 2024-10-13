using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchProjectile : ScriptableObject, IAction<Unit, Vector3>
	{
		[field: SerializeField]
		public ProjectileDesc Projectile { get; private set; }

		public void Perform(Unit actor, Vector3 target)
		{
			actor.SpawnProjectile(Projectile, target);
		}
	}
}