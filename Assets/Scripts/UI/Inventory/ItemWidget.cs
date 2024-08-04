using Omniverse.Input;
using Omniverse.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Omniverse.UI
{
	public class ItemWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		[Inject]
		private AbilityHandlerResolver AbilityHandlerResolver { get; set; }

		[Inject]
		private UnitSelector UnitSelector { get; set; }

		private Item Item { get; set; }

		public void Bind(Item item)
		{
			Item = item;
			Icon.sprite = Item.Desc.Icon;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					if (Item.Ability is not null)
					{
						AbilityHandlerResolver.TryCastAbility(UnitSelector.SelectedUnit, Item.Ability);
					}
					break;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Highlight.enabled = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Highlight.enabled = false;
		}
	}
}
