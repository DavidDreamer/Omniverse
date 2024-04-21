using System;
using UnityEngine;

namespace Omniverse.Cameras
{
	[Serializable]
	public class ScreenBordersMovement
	{
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float Sensitivity { get; private set; }
		
		[field: SerializeField]
		public float Speed { get; private set; }
	}
}
