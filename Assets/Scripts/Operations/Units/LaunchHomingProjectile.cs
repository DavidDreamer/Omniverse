using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchHomingProjectile : Action
	{
		[field: SerializeField]
		public HomingProjectileDesc Projectile { get; private set; }

		public override void PerformTemp(OperationContext context)
		{
			//TODO
			var unit = context.Actor as Unit;

			Vector3 position = context.Actor.transform.position;
			var homingProjectile = Instantiate(Projectile.Model, position, Quaternion.identity).GetComponent<HomingProjectile>();
			homingProjectile.Initialize(Projectile);
			homingProjectile.ChangeFaction(unit.FactionID);
			context.ObjectResolver.Inject(homingProjectile);

			Unit target = context.Units().First();
			homingProjectile.Target = target;
		}
	}
}