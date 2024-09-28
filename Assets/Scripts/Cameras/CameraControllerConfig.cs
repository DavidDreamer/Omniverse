using Dreambox.Core;
using UnityEngine;

namespace Omniverse.Cameras
{
	[CreateAssetMenu(menuName = "Omniverse/Config/CameraControllerConfig", fileName = nameof(CameraControllerConfig))]
	public class CameraControllerConfig : ScriptableObject
	{
		[field: SerializeField]
		public FloatRange HeightRange { get; private set; }

		[field: SerializeField]
		[field: Range(0, 1)]
		public float DefaultHeight { get; private set; }

		[field: SerializeField]
		public Vector3 Rotation { get; private set; }

		[field: SerializeField]
		public FloatRange XBounds { get; private set; }

		[field: SerializeField]
		public FloatRange ZBounds { get; private set; }

		[field: SerializeField]
		public float Speed { get; private set; }
	}
}