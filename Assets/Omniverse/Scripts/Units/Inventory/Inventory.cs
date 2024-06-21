using System.Collections.Generic;
using System.Linq;
using Omniverse.Items;

namespace Omniverse.Units
{
	public class Inventory
	{
		public List<InventorySlot> Slots { get; }

		public Inventory(InventoryDesc desc)
		{
			Slots = new List<InventorySlot>(desc.Capacity);
			for (int i = 0; i < desc.Capacity; ++i)
			{
				var slot = new InventorySlot();
				Slots.Add(slot);
			}
		}

		public void Add(Item item)
		{
			InventorySlot slot = Slots.First(slot => slot.IsEmpty());
			slot.Item = item;
		}
	}
}
