using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Omniverse.UI
{
	//TODO ECS
	public class ItemWidget : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[field: SerializeField]
		private Image Icon { get; set; }

		[field: SerializeField]
		private Image Highlight { get; set; }

		private Entity Item { get; set; }

		public void Bind(Entity item)
		{
			Item = item;
			//Icon.sprite = Item.Desc.Icon;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			var selection = ECSUtils.GetSingleton<Selection>();

			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
					//if (Item.Ability != Entity.Null)
					//{
						//AbilityController.Process(selection.Entity, Item.Ability);
					//}
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
