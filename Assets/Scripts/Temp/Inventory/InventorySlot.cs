using UnityEngine;

namespace Omniverse
{
	public class InventorySlot
	{
		public GameObject Item { get; set; }
	}

	public static class InventorySlotUtils
	{
		public static bool IsEmpty(this InventorySlot inventorySlot) => inventorySlot.Item is null;
	}
}
