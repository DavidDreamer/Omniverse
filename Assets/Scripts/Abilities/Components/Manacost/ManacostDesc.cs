using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ManacostDesc
	{
		[field: SerializeField]
		public float Value { get; private set; }

		[field: SerializeField]
		public PropertyModifierMode Mode { get; private set; }
	}
}
