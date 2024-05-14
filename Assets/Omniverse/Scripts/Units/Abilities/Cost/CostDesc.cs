using System;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class CostDesc
	{
		[field: SerializeField]
		public PropertyTag PropertyTag { get; private set; }
		
		[field: SerializeField]
		public float Amount { get; set; }
		
		[field: SerializeField]
		public CostMode Mode { get; private set; }
	}
}
