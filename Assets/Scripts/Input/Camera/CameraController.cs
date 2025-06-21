using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omniverse.Input
{
	public class CameraController : MonoBehaviour
	{
		[field: SerializeField]
		private Camera Camera { get; set; }

		[field: SerializeField]
		public CameraControllerConfig Config { get; set; }

		private float CurrentHeight { get; set; }

		public void Start()
		{
			CurrentHeight = Config.HeightRange.Evaluate(Config.DefaultHeight);

			Camera.transform.position = new Vector3(Camera.transform.position.x, CurrentHeight, Camera.transform.position.z);
			Camera.transform.eulerAngles = Config.Rotation;

			Cursor.lockState = CursorLockMode.Confined;
		}

		public void SetViewPoint(Vector3 viewPoint)
		{
			float distanceToViewPoint = CurrentHeight / Mathf.Sin(Mathf.Deg2Rad * Camera.transform.eulerAngles.x);

			Camera.transform.position = viewPoint - Camera.transform.forward * distanceToViewPoint;
		}
	}
}
