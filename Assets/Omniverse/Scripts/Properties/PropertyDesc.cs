using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class PropertyDesc
	{
		[field: SerializeField]
		public PropertyTag Tag { get; private set; }

		[field: SerializeField]
		public float Capacity { get; private set; }

		[field: SerializeField]
		public float Regeneration { get; private set; }

		[field: SerializeField]
		public bool Vital { get; private set; }
	}
}
