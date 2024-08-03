using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Entities.Items
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Item")]
	public class ItemDesc: EntityDesc
	{
		[field: SerializeField]
		public Item Prefab { get; private set; }
		
		[field: SerializeField]
		public AbilityDesc Ability { get; private set; }
		
		[field: SerializeField]
		public bool Consumable { get; private set; }
	}
}
