using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Omniverse.UI
{
	public class InventoryButton: MonoBehaviour, IPointerClickHandler
	{
		[Inject]
		private InventoryWidget InventoryWidget { get; set; }

		public void OnPointerClick(PointerEventData eventData)
		{
			InventoryWidget.Canvas.enabled = !InventoryWidget.Canvas.enabled;
		}
	}
}
