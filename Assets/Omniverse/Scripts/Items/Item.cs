namespace Omniverse.Items
{
	public interface IItem
	{
		ItemPresenter Presenter { get; set; }
	}


	public class Item: Item<ItemDesc>
	{
		public Item(ItemDesc desc): base(desc)
		{
		}
	}
	
	public abstract class Item<TDesc>: IItem
		where TDesc: ItemDesc
	{
		public TDesc Desc { get;  }
		
		public ItemPresenter Presenter { get; set; }

		protected Item(TDesc desc)
		{
			Desc = desc;
		}
	}
}
