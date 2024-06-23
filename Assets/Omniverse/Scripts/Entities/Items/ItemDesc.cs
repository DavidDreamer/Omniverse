using System;
using UnityEngine;

namespace Omniverse.Entities.Items
{
	public static class ItemDescUtils
	{
		public static Item Build(this ItemDesc itemDesc)
		{
			return itemDesc switch
			{
				ItemDesc desc => new Item(desc, -1),
				// ResourceItemDesc desc => new ResourceItem(desc),
				// PropertyItemDesc desc => new PropertyItem(desc),
				_ => throw new ArgumentOutOfRangeException(nameof(itemDesc))
			};
		}
	}

	[CreateAssetMenu(menuName = "Omniverse/Desc/Item")]
	public class ItemDesc: EntityDesc
	{
		[field: SerializeField]
		public Sprite Icon { get; private set; }
		
		[field: SerializeField]
		public ItemPresenter Prefab { get; private set; }
	}
}
