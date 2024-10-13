using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse
{
	public class TempNameManager
	{
		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		private List<TempName> Items { get; } = new();

		public void Spawn(HomingProjectileDesc desc, Vector3 position, Unit owner, Unit target, int factionID)
		{
			var homingProjectile = Object.Instantiate(desc.Model, position, Quaternion.identity).GetComponent<HomingProjectile>();
			homingProjectile.Initialize(desc);
			homingProjectile.FactionID = factionID;
			homingProjectile.Owner = owner;
			ObjectResolver.Inject(homingProjectile);
			homingProjectile.Target = target;

			Items.Add(homingProjectile);
		}

		public void Spawn(ProjectileDesc desc, Vector3 position, Vector3 direction, int factionID) 
		{
			var projectile = Object.Instantiate(desc.Model, position, Quaternion.identity).GetComponent<Projectile>();
			projectile.Initialize(desc);
			projectile.FactionID = factionID;
			ObjectResolver.Inject(projectile);
			projectile.Direction = direction;

			Items.Add(projectile);
		}

		public void Tick(float deltaTime)
		{
			foreach (var item in Items)
			{
				item.Tick(deltaTime);
			}

			for (int i = 0; i < Items.Count; ++i)
			{
				if (Items[i].Completed)
				{
					Object.Destroy(Items[i].gameObject);
					Items.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
