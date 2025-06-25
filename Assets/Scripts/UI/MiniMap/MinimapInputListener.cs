using Omniverse.Input;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omniverse.UI
{
	public class MinimapInputListener : Widget, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
	{
		[field: SerializeField]
		private RectTransform RectTransform { get; set; }

		[field: SerializeField]
		[field: HideInInspector]
		private Canvas Canvas { get; set; }

		private bool MovingEnabled { get; set; }

		private void OnValidate()
		{
			Canvas = GetComponentInParent<Canvas>(true);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
				{
					MovingEnabled = true;
					MoveCameraToPointerPosition(eventData);
					break;
				}
			}
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
				{
					if (MovingEnabled)
					{
						MoveCameraToPointerPosition(eventData);
					}

					break;
				}
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
				{
					MovingEnabled = false;
					break;
				}
				case PointerEventData.InputButton.Right:
				{
					Vector3 position = TransformPosition(eventData);
					//TODO ECS
					//UnitController.ProcessNavigationPoint(position);
					break;
				}
			}
		}

		private Vector3 TransformPosition(PointerEventData data)
		{
			var mapSettings = EntityManager.GetSingleton<MapSettings>();

			RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, data.position, data.pressEventCamera, out Vector2 localPoint);

			float x = Mathf.InverseLerp(RectTransform.rect.xMin, RectTransform.rect.xMax, localPoint.x);
			float y = Mathf.InverseLerp(RectTransform.rect.yMin, RectTransform.rect.yMax, localPoint.y);

			float xWorldSpace = (x - 0.5f) * mapSettings.Size.x;
			float yWorldSpace = (y - 0.5f) * mapSettings.Size.y;

			return new Vector3(xWorldSpace, 0, yWorldSpace);
		}

		private void MoveCameraToPointerPosition(PointerEventData eventData)
		{
			Vector3 viewPoint = TransformPosition(eventData);
			var system = EntityManager.World.GetExistingSystemManaged<ProcessCameraInputSystem>();
			system.SetViewPoint(viewPoint);
		}
	}
}
