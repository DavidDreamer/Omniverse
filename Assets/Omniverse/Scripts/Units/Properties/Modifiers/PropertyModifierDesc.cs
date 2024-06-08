using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class PropertyModifierDesc
	{
		[field: SerializeField]
		public PropertyID ID { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }
	}
}
