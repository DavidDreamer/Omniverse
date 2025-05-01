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
					Vector3 position = TransformPosition(eventData.position);
					//TODO ECS
					//UnitController.ProcessNavigationPoint(position);
					break;
				}
			}
		}

		private Vector3 TransformPosition(Vector2 position)
		{
			var gameOptions = EntityManager.GetSingletonManaged<GameOptions>();
			Vector2 mapSize = new(gameOptions.MapSize.x, gameOptions.MapSize.y);
			Vector2 sizeMultiplier = mapSize / (RectTransform.rect.size * Canvas.scaleFactor);
			Vector2 worldSpacePosition = position * sizeMultiplier;
			return new Vector3(worldSpacePosition.x, 0, worldSpacePosition.y);
		}

		private void MoveCameraToPointerPosition(PointerEventData eventData)
		{
			Vector3 viewPoint = TransformPosition(eventData.position);
			FindFirstObjectByType<CameraController>().SetViewPoint(viewPoint);
		}
	}
}
