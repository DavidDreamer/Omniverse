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

		public void Spawn(MissileDesc desc, Unit owner, Vector3 position, Vector3 direction)
		{
			var missile = Object.Instantiate(desc.Model, position, Quaternion.identity).GetComponent<Missile>();
			ObjectResolver.Inject(missile);
			missile.Initialize(desc, owner, direction);
			Items.Add(missile);
		}

		public void Spawn(MissileDesc desc, Unit owner, Vector3 position, Unit target)
		{
			var missile = Object.Instantiate(desc.Model, position, Quaternion.identity).GetComponent<Missile>();
			ObjectResolver.Inject(missile);
			missile.Initialize(desc, owner, target);
			Items.Add(missile);
		}

		public void Spawn(ChainDesc desc, Vector3 position, Unit owner, Unit target, int factionID)
		{
			var chain = Object.Instantiate(desc.Model, position, Quaternion.identity).GetComponent<Chain>();
			chain.Initialize(desc, factionID);
			chain.Owner = owner;
			ObjectResolver.Inject(chain);
			chain.Target = target;

			Items.Add(chain);
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
