using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.Input
{
	[BurstCompile]
	[UpdateInGroup(typeof(InputSystemGroup))]
	public partial struct ProcessCameraInputSystem : ISystem
	{
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var selection = SystemAPI.GetSingleton<Selection>();

			if (selection.InProcess)
			{
				return;
			}

			var cameraController = Object.FindFirstObjectByType<CameraController>();

			Mouse mouse = Mouse.current;
			float deltaTime = SystemAPI.Time.DeltaTime;
			var mapSettings = SystemAPI.GetSingleton<MapSettings>();

			Vector3 position = cameraController.transform.position;

			if (mouse.middleButton.isPressed)
			{
				position = ProcessSnapping(position, mouse.delta.value);
			}

			float speed = cameraController.Config.Speed * deltaTime;
			position = ProcessScreenBorderMovements(position, mouse.position.value, speed);
			position = ProcessBounds(position, mapSettings.Size);

			cameraController.transform.position = position;
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
	}
}
