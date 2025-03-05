using Omniverse.Abilities;

namespace Omniverse.Items
{
	public class Item : OmniverseEntity<ItemDesc>, IPoolObject
	{
		public Ability Ability { get; set; }

		public override void Initialize(ItemDesc desc)
		{
			base.Initialize(desc);

			if (desc.Ability is not null)
			{
				Ability = new Ability(desc.Ability, this);
			}
		}

		public void Cleanup()
		{
		}
	}
}
