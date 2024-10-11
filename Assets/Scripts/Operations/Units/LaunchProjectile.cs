using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class LaunchProjectile : Action
	{
		[field: SerializeField]
		public ProjectileDesc Projectile { get; private set; }

		public override void PerformTemp(OperationContext context)
		{
			//TODO
			var unit = context.Actor as Unit;

			Vector3 position = context.Actor.transform.position;
			Projectile projectile = Instantiate(Projectile.Model, position, Quaternion.identity).GetComponent<Projectile>();
			projectile.Initialize(Projectile);
			projectile.ChangeFaction(unit.FactionID);
			Vector3 direction = context.Directions.First();
			projectile.Direction = direction;
		}
	}
}