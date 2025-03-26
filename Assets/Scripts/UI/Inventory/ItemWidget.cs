using Omniverse.Input;
using Omniverse.Items;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omniverse.UI
{
	public class ItemWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		private Item Item { get; set; }

		public void Bind(Item item)
		{
			Item = item;
			Icon.sprite = Item.Desc.Icon;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			var selection = ECSUtils.GetSingleton<Selection>();

			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					if (Item.Ability != Entity.Null)
					{
						//TODO ECS
						//AbilityController.Process(selection.Entity, Item.Ability);
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
