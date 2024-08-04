using Omniverse.Abilities;
using UnityEngine;
using VContainer;

namespace Omniverse.Items
{
	public class Item : Entity<ItemDesc>, IPoolObject
	{
		public Ability Ability { get; set; }

		[field: SerializeField]
		private IObjectResolver ObjectResolver { get; set; }

		public override void Initialize(ItemDesc desc)
		{
			base.Initialize(desc);

			if (desc.Ability is not null)
			{
				Ability = new Ability(ObjectResolver, desc.Ability);
			}
		}

		public void Cleanup()
		{
		}
	}
}
