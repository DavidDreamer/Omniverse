using Omniverse.Cameras;
using Omniverse.Mapping;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Omniverse.UI
{
	public class MinimapInputListener: MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
	{
		[field: SerializeField]
		private RectTransform RectTransform { get; set; }

		[Inject]
		private Map Map { get; set; }

		[field: SerializeField]
		public CameraController CameraController { get; set; }

		private bool MovingEnabled { get; set; }

		public void OnPointerDown(PointerEventData eventData)
		{
			MovingEnabled = true;

			ProcessMovingToPointerPosition(eventData);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			if (MovingEnabled)
			{
				ProcessMovingToPointerPosition(eventData);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			MovingEnabled = false;
		}

		private void ProcessMovingToPointerPosition(PointerEventData eventData)
		{
			Vector2 sizeMultiplier = Map.MapSettings.Size / RectTransform.rect.size;
			Vector2 worldSpacePosition = eventData.position * sizeMultiplier;
			var viewPoint = new Vector3(worldSpacePosition.x, 0, worldSpacePosition.y);

			CameraController.SetViewPoint(viewPoint);
		}
	}
}
