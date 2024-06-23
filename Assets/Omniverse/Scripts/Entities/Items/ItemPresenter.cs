namespace Omniverse.Entities.Items
{
	public class ItemPresenter: EntityPresenter<Item, ItemDesc>, IPoolObject
	{
		public virtual void Cleanup()
		{
		}
	}
}
