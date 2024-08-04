using Omniverse.Units;
using UnityEngine;

namespace Omniverse.UI
{
	public class InventorySlotWidget : MonoBehaviour
	{
		[field: SerializeField]
		private ItemWidget ItemWidget { get; set; }

		private InventorySlot Slot { get; set; }

		public void Bind(InventorySlot slot)
		{
			Slot = slot;
			bool hasItem = !slot.IsEmpty();
			ItemWidget.gameObject.SetActive(hasItem);
			if (hasItem)
			{
				ItemWidget.Bind(slot.Item);
			}
		}
	}
}
