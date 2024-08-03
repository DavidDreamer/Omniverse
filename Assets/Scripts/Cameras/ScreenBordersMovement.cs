using System;
using UnityEngine;

namespace Omniverse.Cameras
{
	[Serializable]
	public class ScreenBordersMovement
	{
		[field: SerializeField]
		public float Threshold { get; private set; }
		
		[field: SerializeField]
		public float Speed { get; private set; }
	}
}
