using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ResourceItemDesc: ConsumableItemDesc
	{
		[field: SerializeField]
		[field: Resource]
		public int ResourceID { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
