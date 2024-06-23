using System;
using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Entities.Items
{
	public static class ItemDescUtils
	{
		public static Item Construct(this ItemDesc desc) => new(desc, -1);
	}

	[CreateAssetMenu(menuName = "Omniverse/Desc/Item")]
	public class ItemDesc: EntityDesc
	{
		[field: SerializeField]
		public Sprite Icon { get; private set; }
		
		[field: SerializeField]
		public ItemPresenter Prefab { get; private set; }
		
		[field: SerializeField]
		public AbilityDesc Ability { get; private set; }
		
		[field: SerializeField]
		public bool Consumable { get; private set; }
	}
}
