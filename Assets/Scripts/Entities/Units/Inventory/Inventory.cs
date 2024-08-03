using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Entities.Items;

namespace Omniverse.Entities.Units
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

			if (item.Desc.Consumable && item.Ability is not null)
			{
				WaitForConsumeAsync(slot, item, default).Forget();
			}
		}

		private async UniTaskVoid WaitForConsumeAsync(InventorySlot slot, Item item, CancellationToken token)
		{
			await item.Ability.Used.Task;

			slot.Item = null;
		}
	}
}
