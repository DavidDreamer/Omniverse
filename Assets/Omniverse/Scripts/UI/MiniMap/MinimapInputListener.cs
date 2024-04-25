using System;
using Omniverse.Cameras;
using Omniverse.Input;
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

		[Inject]
		private UnitController UnitController { get; set; }

		[field: SerializeField]
		public CameraController CameraController { get; set; }

		private bool MovingEnabled { get; set; }

		public void OnPointerDown(PointerEventData eventData)
		{
			switch (eventData.button)
			{
				case PointerEventData.InputButton.Left:
				{
					MovingEnabled = true;
					ProcessMovingToPointerPosition(eventData);
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
						ProcessMovingToPointerPosition(eventData);
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
					UnitController.ProcessNavigationPoint(position);
					break;
				}
			}
		}

		private Vector3 TransformPosition(Vector2 position)
		{
			Vector2 sizeMultiplier = Map.MapSettings.Size / RectTransform.rect.size;
			Vector2 worldSpacePosition = position * sizeMultiplier;
			return new Vector3(worldSpacePosition.x, 0, worldSpacePosition.y);
		}

		private void ProcessMovingToPointerPosition(PointerEventData eventData)
		{
			Vector3 viewPoint = TransformPosition(eventData.position);
			CameraController.SetViewPoint(viewPoint);
		}
	}
}
