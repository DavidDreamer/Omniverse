using Omniverse.Abilities;

namespace Omniverse.Entities.Items
{
	public class Item: Entity<ItemDesc>
	{
		public ItemPresenter Presenter { get; set; }

		public Ability Ability { get; }

		public Item(ItemDesc desc, int factionID): base(desc, factionID)
		{
			if (desc.Ability is not null)
			{
				Ability = new Ability(desc.Ability);
			}
		}
	}
}
