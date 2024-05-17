using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class MovementDesc
	{
		[field: SerializeField]
		public float Speed { get; private set; }
		
		[field: SerializeField]
		public float RotationSpeed { get; private set; }
	}
}
