using System;
using UnityEngine;

namespace Omniverse.Items
{
	public static class ItemDescUtils
	{
		public static IItem Build(this ItemDesc itemDesc)
		{
			return itemDesc switch
			{
				ItemDesc desc => new Item(desc),
				// ResourceItemDesc desc => new ResourceItem(desc),
				// PropertyItemDesc desc => new PropertyItem(desc),
				_ => throw new ArgumentOutOfRangeException(nameof(itemDesc))
			};
		}
	}

	[CreateAssetMenu(menuName = "Omniverse/Desc/Item")]
	public class ItemDesc: ScriptableObject
	{
		[field: SerializeField]
		public Sprite Icon { get; private set; }
		
		[field: SerializeField]
		public ItemPresenter Prefab { get; private set; }
	}
}
