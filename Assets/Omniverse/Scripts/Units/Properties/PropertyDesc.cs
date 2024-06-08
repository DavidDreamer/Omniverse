using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class PropertyDesc
	{
		[field: SerializeField]
		public PropertyID ID { get; private set; }

		[field: SerializeField]
		public float Capacity { get; private set; }

		[field: SerializeField]
		public float Regeneration { get; private set; }

		[field: SerializeField]
		public bool Vital { get; private set; }
	}
}
