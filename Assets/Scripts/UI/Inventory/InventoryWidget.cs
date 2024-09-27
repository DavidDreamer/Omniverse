using System.Collections.Generic;
using Omniverse.Input;
using Omniverse.Units;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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

		[Inject]
		private Selector Selector { get; set; }

		[Inject]
		private IObjectResolver ObjectResolver { get; set; }

		public void LateUpdate()
		{
			if (Selector.HasSelection is false)
			{
				return;
			}

			Unit unit = Selector.SelectedUnit;

			UpdateSlotsCount(unit.Inventory.Slots.Count);

			for (var i = 0; i < unit.Inventory.Slots.Count; i++)
			{
				InventorySlot inventorySlot = unit.Inventory.Slots[i];
				Slots[i].Bind(inventorySlot);
			}
		}

		private void UpdateSlotsCount(int count)
		{
			int slotsDelta = count - Slots.Count;

			if (slotsDelta > 0)
			{
				while (slotsDelta != 0)
				{
					InventorySlotWidget slot = ObjectResolver.Instantiate(InventorySlotPrefab, SlotsHolder);
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
