using UnityEngine;
using UnityEngine.EventSystems;

namespace Omniverse.UI
{
	public class InventoryButton : MonoBehaviour, IPointerClickHandler
	{
		[field: SerializeField]
		private InventoryWidget InventoryWidget { get; set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			InventoryWidget.Canvas.enabled = !InventoryWidget.Canvas.enabled;
		}
	}
}
