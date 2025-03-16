using System.Collections.Generic;
using Omniverse.Input;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.UI
{
	public class InventoryWidget : MonoBehaviour
	{
		[field: SerializeField]
		public Canvas Canvas { get; private set; }

		[field: SerializeField]
		private InventorySlotWidget InventorySlotPrefab { get; set; }

		[field: SerializeField]
		private Transform SlotsHolder { get; set; }

		public List<InventorySlotWidget> Slots { get; } = new();

		public void LateUpdate()
		{
			var selection = ECSUtils.GetSingleton<Selection>();

			if (selection.HasSelection is false)
			{
				return;
			}

			Entity unit = selection.Entity;

			//TODO ECS
			//UpdateSlotsCount(unit.Inventory.Slots.Count);

			//for (var i = 0; i < unit.Inventory.Slots.Count; i++)
			//{
			//	InventorySlot inventorySlot = unit.Inventory.Slots[i];
			//	Slots[i].Bind(inventorySlot);
			//}
		}

		private void UpdateSlotsCount(int count)
		{
			int slotsDelta = count - Slots.Count;

			if (slotsDelta > 0)
			{
				while (slotsDelta != 0)
				{
					InventorySlotWidget slot = Instantiate(InventorySlotPrefab, SlotsHolder);
					Slots.Add(slot);
					slotsDelta--;
				}
			}
			else if (slotsDelta < 0)
			{
				while (slotsDelta != 0)
				{
					Slots.RemoveAt(0);
					Destroy(Slots[0].gameObject);
					slotsDelta++;
				}
			}
		}
	}
}
