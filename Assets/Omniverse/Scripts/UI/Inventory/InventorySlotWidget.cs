using System;
using Omniverse.Entities.Units;
using Omniverse.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class InventorySlotWidget: MonoBehaviour, IPointerClickHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[Inject]
		private AbilityController AbilityController { get; set; }
		
		[Inject]
		private UnitSelector UnitSelector { get; set; }
		
		private InventorySlot Slot { get; set; }
		
		public void Bind(InventorySlot slot)
		{
			Slot = slot;
			bool hasItem = slot.Item != null;
			Icon.gameObject.SetActive(hasItem);
			if (hasItem)
			{
				Icon.sprite = slot.Item.Desc.Icon;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (Slot.Item?.Ability is null)
			{
				return;
			}
			
			AbilityController.TryCastAbility(UnitSelector.SelectedUnit.Unit, Slot.Item.Ability);
		}
	}
}
