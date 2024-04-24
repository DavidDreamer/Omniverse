using Omniverse.Mapping;
using UnityEngine;
using VContainer;

namespace Omniverse.UI
{
	public class MiniMapCameraBounds: MonoBehaviour
	{
		[field: SerializeField]
		public RectTransform ParentRectTransform { get; set; }
		
		[field: SerializeField]
		public RectTransform RectTransform { get; set; }
		
		[Inject]
		private MiniMap MiniMap { get; set; }
		
		private void Update()
		{
			UnityEngine.Camera mainCamera = UnityEngine.Camera.main;
			Transform cameraTransform = mainCamera.transform;

			float distanceToViewPoint = 25f / Mathf.Cos(Mathf.Deg2Rad * cameraTransform.eulerAngles.x);
			
			Vector3 viewPoint = cameraTransform.position + cameraTransform.forward * distanceToViewPoint;
			float sizeY = 2.0f * distanceToViewPoint * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float sizeX = sizeY * mainCamera.aspect;
			Vector3 offset = new Vector3(sizeX, 0, sizeY) / 2f;

			Vector3 left = viewPoint - offset;
			Vector3 right = viewPoint + offset;
			
			Rect parentRect = ParentRectTransform.rect;
			
			float mapRelativePositionX = Mathf.InverseLerp(0, MiniMap.MapSettings.Size.x, viewPoint.x);
			float mapRelativePositionY = Mathf.InverseLerp(0, MiniMap.MapSettings.Size.y, viewPoint.z);

			float width = parentRect.width / 2f;
			float rectRelativePositionX = Mathf.Lerp(-width, width, mapRelativePositionX);
			float height = parentRect.height / 2f;
			float rectRelativePositionY = Mathf.Lerp(-height, height, mapRelativePositionY);
			
			RectTransform.anchoredPosition = new Vector2(rectRelativePositionX, rectRelativePositionY);

			//RectTransform.anchorMin = ConverCoordinateFromWorldToRectSpace(left);
			//RectTransform.anchorMax = ConverCoordinateFromWorldToRectSpace(right);

			RectTransform.sizeDelta = ConvertWorldSpaceSizeToRectSpace(new Vector2(sizeX, sizeY));

			Vector2 ConvertWorldSpaceSizeToRectSpace(Vector2 size)
			{
				return size * (parentRect.size / MiniMap.MapSettings.Size);
			}
			
			Vector2 ConverCoordinateFromWorldToRectSpace(Vector3 position)
			{
				float mapRelativePositionX = Mathf.InverseLerp(0, MiniMap.MapSettings.Size.x, position.x);
				float mapRelativePositionY = Mathf.InverseLerp(0, MiniMap.MapSettings.Size.y, position.z);

				// float width = parentRect.width / 2f;
				// float rectRelativePositionX = Mathf.Lerp(-width, width, mapRelativePositionX);
				// float height = parentRect.height / 2f;
				// float rectRelativePositionY = Mathf.Lerp(-height, height, mapRelativePositionY);

				return new Vector2(mapRelativePositionX, mapRelativePositionY);
			}
		}
	}
}
