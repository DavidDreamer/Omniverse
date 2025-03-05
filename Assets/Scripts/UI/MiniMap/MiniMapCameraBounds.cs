using UnityEngine;

namespace Omniverse.UI
{
	public class MiniMapCameraBounds : MonoBehaviour
	{
		[field: SerializeField]
		public RectTransform ParentRectTransform { get; set; }

		[field: SerializeField]
		public RectTransform RectTransform { get; set; }

		private Map Map { get; set; }

		private void Start()
		{
			//TEMP
			var query = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(new Unity.Entities.ComponentType[] { typeof(Map) });
			Map = query.GetSingleton<Map>();
			query.Dispose();
		}

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

			Vector2 mapSize = new(Map.Size.x, Map.Size.y);

			Vector2 mapRelativePosition = ConverCoordinateFromWorldToRectSpace(viewPoint);

			float width = parentRect.width / 2f;
			float rectRelativePositionX = Mathf.Lerp(-width, width, mapRelativePosition.x);
			float height = parentRect.height / 2f;
			float rectRelativePositionY = Mathf.Lerp(-height, height, mapRelativePosition.y);

			RectTransform.anchoredPosition = new Vector2(rectRelativePositionX, rectRelativePositionY);

			RectTransform.sizeDelta = ConvertWorldSpaceSizeToRectSpace(new Vector2(sizeX, sizeY));

			Vector2 ConvertWorldSpaceSizeToRectSpace(Vector2 size)
			{
				return size * (parentRect.size / mapSize);
			}

			Vector2 ConverCoordinateFromWorldToRectSpace(Vector3 position)
			{
				float mapRelativePositionX = Mathf.InverseLerp(0, mapSize.x, position.x);
				float mapRelativePositionY = Mathf.InverseLerp(0, mapSize.y, position.z);

				return new Vector2(mapRelativePositionX, mapRelativePositionY);
			}
		}
	}
}
