using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.Cameras
{
	public class CameraController: MonoBehaviour
	{
		[field: SerializeField]
		private CameraControllerConfig Config { get; set; }
		
		private float CurrentHeight { get; set; }

		public void Start()
		{
			CurrentHeight = Config.HeightRange.Evaluate(Config.DefaultHeight);

			transform.position = new Vector3(transform.position.x, CurrentHeight, transform.position.z);
			transform.eulerAngles = Config.Rotation;

			Cursor.lockState = CursorLockMode.Confined;
		}

		private void ProcessScreenBorderMovements(ref Vector3 position)
		{
			Vector2 mousePosition = Mouse.current.position.value;
			float sensativity = Config.ScreenBordersMovement.Sensitivity;
			float speed = Config.ScreenBordersMovement.Speed;
			float deltaTime = Time.deltaTime;
			
			float halfScreenWidth = Screen.width / 2f;
			float halfScreenHeight = Screen.height / 2f;
			
			float xDeltaRaw = mousePosition.x - halfScreenWidth;
			float yDetalRaw = mousePosition.y - halfScreenHeight;
		
			float xDeltaClamped = Mathf.Clamp(Mathf.Abs(xDeltaRaw) / halfScreenWidth, sensativity, 1);
			float yDeltaClamped = Mathf.Clamp(Mathf.Abs(yDetalRaw) / halfScreenHeight, sensativity, 1);
			
			float xDeltaNormalized = Mathf.InverseLerp(sensativity, 1, xDeltaClamped);
			float yDeltaNormalized = Mathf.InverseLerp(sensativity, 1, yDeltaClamped);
			
			float xDirection = Mathf.Sign(xDeltaRaw);
			float yDirection = Mathf.Sign(yDetalRaw);

			float xOffset = xDeltaNormalized * xDirection * speed * deltaTime;
			float yOffset = yDeltaNormalized * yDirection * speed * deltaTime;
			
			position += new Vector3(xOffset, 0, yOffset);
		}

		private void ProcessBounds(ref Vector3 position)
		{
			float clampedX = Config.XBounds.Clamp(position.x);
			float clampedZ = Config.ZBounds.Clamp(position.z);
			position = new Vector3(clampedX, position.y, clampedZ);
		}
		
		public void LateUpdate()
		{
			Vector3 position = transform.position;
			if (Mouse.current.middleButton.isPressed)
			{
				float x = Mouse.current.delta.x.value;
				float y = Mouse.current.delta.y.value;

				position += new Vector3(x, 0, y);
			}

			ProcessScreenBorderMovements(ref position);
			ProcessBounds(ref position);

			transform.position = position;
		}

		public void SetViewPoint(Vector3 viewPoint)
		{
			float distanceToViewPoint = CurrentHeight / Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);

			transform.position = viewPoint - transform.forward * distanceToViewPoint;
		}
	}
}
