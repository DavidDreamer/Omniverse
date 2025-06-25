using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.Input
{
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial class ProcessCameraInputSystem : SystemBase
	{
		protected override void OnCreate()
		{
			RequireForUpdate<CameraController>();
		}

		protected override void OnStartRunning()
		{
			var cameraController = SystemAPI.GetSingletonRW<CameraController>();
			CameraControllerSettings settings = cameraController.ValueRO.Settings.Value;

			var camera = Camera.main;

			cameraController.ValueRW.Camera = camera;
			cameraController.ValueRW.Height = settings.HeightRange.Evaluate(settings.DefaultHeight);

			camera.transform.position = new Vector3(camera.transform.position.x, cameraController.ValueRO.Height, camera.transform.position.z);
			camera.transform.eulerAngles = settings.Rotation;

			Cursor.lockState = CursorLockMode.Confined;
		}

		protected override void OnUpdate()
		{
			var selection = SystemAPI.GetSingleton<Selection>();

			if (selection.InProcess)
			{
				return;
			}

			var cameraController = SystemAPI.GetSingletonRW<CameraController>();

			Mouse mouse = Mouse.current;
			float deltaTime = SystemAPI.Time.DeltaTime;
			var mapSettings = SystemAPI.GetSingleton<MapSettings>();

			Camera camera = cameraController.ValueRW.Camera.Value;
			Vector3 position = camera.transform.position;

			if (mouse.middleButton.isPressed)
			{
				position = ProcessSnapping(position, mouse.delta.value);
			}

			float speed = cameraController.ValueRW.Settings.Value.Speed * deltaTime;
			position = ProcessScreenBorderMovements(position, mouse.position.value, speed);
			position = ProcessBounds(position, mapSettings.Size);

			camera.transform.position = position;
		}

		private Vector3 ProcessSnapping(Vector3 position, Vector2 delta)
		{
			Vector3 offset = new(delta.x, 0, delta.y);
			return position + offset;
		}

		private Vector3 ProcessScreenBorderMovements(Vector3 position, Vector2 mousePosition, float speed)
		{
			const float threshold = 4f;

			float xDirection = mousePosition.x < threshold ? -1 : mousePosition.x > Screen.width - 1 - threshold ? 1 : 0;
			float yDirection = mousePosition.y < threshold ? -1 : mousePosition.y > Screen.height - 1 - threshold ? 1 : 0;
			var direction = new Vector3(xDirection, 0, yDirection);

			return position + direction.normalized * speed;
		}

		private Vector3 ProcessBounds(Vector3 position, int2 mapSize)
		{
			float halfSizeX = mapSize.x / 2;
			float halfSizeY = mapSize.y / 2;
			float clampedX = Mathf.Clamp(position.x, -halfSizeX, halfSizeX);
			float clampedZ = Mathf.Clamp(position.z, -halfSizeY, halfSizeY);
			return new Vector3(clampedX, position.y, clampedZ);
		}

		public void SetViewPoint(Vector3 viewPoint)
		{
			var cameraController = SystemAPI.GetSingletonRW<CameraController>().ValueRW;
			Camera camera = cameraController.Camera.Value;

			float distanceToViewPoint = cameraController.Height / Mathf.Sin(Mathf.Deg2Rad * camera.transform.eulerAngles.x);
			camera.transform.position = viewPoint - camera.transform.forward * distanceToViewPoint;
		}
	}
}
