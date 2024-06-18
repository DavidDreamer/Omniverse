namespace Omniverse.Items
{
	public class Item: Item<ItemDesc>
	{
		public Item(ItemDesc desc, int factionID): base(desc, factionID)
		{
		}
	}
	
	public abstract class Item<TDesc>: Entity<TDesc>
		where TDesc: ItemDesc
	{
		public ItemPresenter Presenter { get; set; }

		protected Item(TDesc desc, int factionID): base(desc, factionID)
		{
		}
	}
}
