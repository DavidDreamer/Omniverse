namespace Omniverse
{
	public interface IConsumableItem: IItem
	{
		bool CanBeConsumed(Unit unit);

		void OnConsumed(Unit unit);
	}
	
	public abstract class ConsumableItem<TDesc>: Item<TDesc>, IConsumableItem
		where TDesc: ConsumableItemDesc
	{
		protected ConsumableItem(TDesc desc): base(desc)
		{
		}
		
		public abstract bool CanBeConsumed(Unit unit);

		public abstract void OnConsumed(Unit unit);
	}
}
