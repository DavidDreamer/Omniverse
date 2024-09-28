using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Omniverse.Input
{
	public class CameraController : IInitializable
	{
		[Inject]
		private Camera Camera { get; set; }

		[Inject]
		private CameraControllerConfig Config { get; set; }

		private float CurrentHeight { get; set; }

		public void Initialize()
		{
			CurrentHeight = Config.HeightRange.Evaluate(Config.DefaultHeight);

			Camera.transform.position = new Vector3(Camera.transform.position.x, CurrentHeight, Camera.transform.position.z);
			Camera.transform.eulerAngles = Config.Rotation;

			Cursor.lockState = CursorLockMode.Confined;
		}

		public void Tick(Mouse mouse, float deltaTime)
		{
			Vector3 position = Camera.transform.position;

			ProcessSnapping(ref position);
			ProcessScreenBorderMovements(ref position);
			ProcessBounds(ref position);

			Camera.transform.position = position;

			void ProcessSnapping(ref Vector3 position)
			{
				if (mouse.middleButton.isPressed)
				{
					float x = mouse.delta.x.value;
					float y = mouse.delta.y.value;

					position += new Vector3(x, 0, y);
				}
			}

			void ProcessScreenBorderMovements(ref Vector3 position)
			{
				const float threshold = 4f;

				Vector2 mousePosition = mouse.position.value;

				float xDirection = mousePosition.x < threshold ? -1 : mousePosition.x > Screen.width - 1 - threshold ? 1 : 0;
				float yDirection = mousePosition.y < threshold ? -1 : mousePosition.y > Screen.height - 1 - threshold ? 1 : 0;
				var direction = new Vector3(xDirection, 0, yDirection);

				float distance = Config.Speed * deltaTime;

				position += direction.normalized * distance;
			}

			void ProcessBounds(ref Vector3 position)
			{
				float clampedX = Config.XBounds.Clamp(position.x);
				float clampedZ = Config.ZBounds.Clamp(position.z);
				position = new Vector3(clampedX, position.y, clampedZ);
			}
		}

		public void SetViewPoint(Vector3 viewPoint)
		{
			float distanceToViewPoint = CurrentHeight / Mathf.Sin(Mathf.Deg2Rad * Camera.transform.eulerAngles.x);

			Camera.transform.position = viewPoint - Camera.transform.forward * distanceToViewPoint;
		}
	}
}
