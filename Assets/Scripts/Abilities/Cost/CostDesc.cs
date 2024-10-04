using System;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class CostDesc
	{
		[field: SerializeField]
		public PropertyID PropertyID { get; private set; }

		[field: SerializeField]
		public float Amount { get; set; }

		[field: SerializeField]
		public CalculationMode Mode { get; private set; }
	}
}
