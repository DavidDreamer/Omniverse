using System;
using Omniverse.Entities.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public class LaunchProjectileDesc : IActionDesc
	{
		[field: SerializeField]
		public ProjectileDesc Projectile { get; private set; }

		public IAction Build() => new LaunchProjectile(this);
	}
}
