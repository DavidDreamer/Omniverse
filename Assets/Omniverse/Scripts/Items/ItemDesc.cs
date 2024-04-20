using System;
using UnityEngine;

namespace Omniverse
{
	public static class ItemDescUtils
	{
		public static IItem Build(this ItemDesc itemDesc)
		{
			return itemDesc switch
			{
				ResourceItemDesc desc => new ResourceItem(desc),
				PropertyItemDesc desc => new PropertyItem(desc),
				_ => throw new ArgumentOutOfRangeException(nameof(itemDesc))
			};
		}
	}
	
	public abstract class ItemDesc: ScriptableObject
	{
		[field: SerializeField]
		public ItemPresenter Prefab { get; private set; }
	}
}
