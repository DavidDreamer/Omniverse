using System;
using Omniverse.Entities.Items;
using UnityEngine;

namespace Omniverse.Entities.Units
{
	[Serializable]
	public class LootDesc
	{
		[field: SerializeField]
		public ItemDesc Item { get; private set; }
		
		[field: SerializeField]
		[field: Range(0f, 1f)]
		public float DropChance { get; private set; }
	}
}
