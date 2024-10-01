using System;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public class LaunchHomingProjectileDesc : IActionDesc
	{
		[field: SerializeField]
		public HomingProjectileDesc Projectile { get; private set; }

		public IAction Build() => new LaunchHomingProjectile(this);
	}
}
