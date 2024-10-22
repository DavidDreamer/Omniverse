using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class InventoryDesc
	{
		[field: SerializeField]
		public int Capacity { get; private set; }
	}
}
