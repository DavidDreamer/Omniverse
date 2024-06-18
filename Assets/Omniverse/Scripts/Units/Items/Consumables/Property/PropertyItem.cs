using Omniverse.Items;
using Omniverse.Units;

namespace Omniverse
{
	public class PropertyItem: Item<PropertyItemDesc>, IConsumableItem
	{
		public PropertyItem(PropertyItemDesc desc): base(desc)
		{
		}

		public bool CanBeConsumed(Unit unit)
		{
			if (unit.Properties.TryGetValue(Desc.PropertyID, out Property property) is false)
			{
				return false;
			}

			if (property.IsFull())
			{
				return false;
			}

			return true;
		}

		public void OnConsumed(Unit unit)
		{
			Property property = unit.Properties[Desc.PropertyID];
			property.Change(Desc.Amount);
		}
	}
}
