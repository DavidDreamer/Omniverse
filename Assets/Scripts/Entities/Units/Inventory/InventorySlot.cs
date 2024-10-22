using Omniverse.Items;

namespace Omniverse
{
	public class InventorySlot
	{
		public Item Item { get; set; }
	}

	public static class InventorySlotUtils
	{
		public static bool IsEmpty(this InventorySlot inventorySlot) => inventorySlot.Item is null;
	}
}
