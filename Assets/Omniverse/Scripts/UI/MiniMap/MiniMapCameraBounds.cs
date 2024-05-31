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
		private Map Map { get; set; }
		
		private void Update()
		{
			UnityEngine.Camera mainCamera = UnityEngine.Camera.main;
			Transform cameraTransform = mainCamera.transform;

			float distanceToViewPoint = 25f / Mathf.Sin(Mathf.Deg2Rad * cameraTransform.eulerAngles.x);
			
			Vector3 viewPoint = cameraTransform.position + cameraTransform.forward * distanceToViewPoint;
			float sizeY = 2.0f * distanceToViewPoint * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
			float sizeX = sizeY * mainCamera.aspect;
			Vector3 offset = new Vector3(sizeX, 0, sizeY) / 2f;

			Vector3 left = viewPoint - offset;
			Vector3 right = viewPoint + offset;
			
			Rect parentRect = ParentRectTransform.rect;
			
			Vector2 mapRelativePosition = ConverCoordinateFromWorldToRectSpace(viewPoint);
			
			float width = parentRect.width / 2f;
			float rectRelativePositionX = Mathf.Lerp(-width, width, mapRelativePosition.x);
			float height = parentRect.height / 2f;
			float rectRelativePositionY = Mathf.Lerp(-height, height, mapRelativePosition.y);
			
			RectTransform.anchoredPosition = new Vector2(rectRelativePositionX, rectRelativePositionY);

			RectTransform.sizeDelta = ConvertWorldSpaceSizeToRectSpace(new Vector2(sizeX, sizeY));

			Vector2 ConvertWorldSpaceSizeToRectSpace(Vector2 size)
			{
				return size * (parentRect.size / Map.MapSettings.Size);
			}
			
			Vector2 ConverCoordinateFromWorldToRectSpace(Vector3 position)
			{
				float mapRelativePositionX = Mathf.InverseLerp(0, Map.MapSettings.Size.x, position.x);
				float mapRelativePositionY = Mathf.InverseLerp(0, Map.MapSettings.Size.y, position.z);

				return new Vector2(mapRelativePositionX, mapRelativePositionY);
			}
		}
	}
}
