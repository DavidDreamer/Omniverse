using Omniverse.Entities.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class InventorySlotWidget: MonoBehaviour
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		public void Bind(InventorySlot slot)
		{
			bool hasItem = slot.Item != null;
			Icon.gameObject.SetActive(hasItem);
			if (hasItem)
			{
				Icon.sprite = slot.Item.Desc.Icon;
			}
		}
	}
}
