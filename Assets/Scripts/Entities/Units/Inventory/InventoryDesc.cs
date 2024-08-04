using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public class InventoryDesc
	{
		[field: SerializeField]
		public int Capacity { get; private set; }
	}
}
