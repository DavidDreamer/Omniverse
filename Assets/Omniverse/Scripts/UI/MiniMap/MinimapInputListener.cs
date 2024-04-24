using Omniverse.Cameras;
using Omniverse.Mapping;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Omniverse.UI
{
	public class MinimapInputListener: MonoBehaviour, 
		IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler, IPointerExitHandler
	{
		private bool Mova;
		
		[field: SerializeField]
		private RectTransform RectTransform { get; set; }
		
		[Inject]
		private Map Map { get; set; }
		
		[field: SerializeField]
		public CameraController CameraController { get; set; }
		
		public void OnPointerDown(PointerEventData eventData)
		{
			Mova = true;
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			//throw new System.NotImplementedException();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Mova = false;

			Vector2 sizeMultiplier = Map.MapSettings.Size / RectTransform.rect.size;
			Vector2 worldSpacePosition = eventData.position * sizeMultiplier;
			Vector3 viewPoint = new Vector3(worldSpacePosition.x, 0, worldSpacePosition.y);

			CameraController.SetViewPoint(viewPoint);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//throw new System.NotImplementedException();
		}
	}
}
