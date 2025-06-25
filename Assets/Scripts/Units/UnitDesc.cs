using System.Collections.Generic;
using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Unit", fileName = nameof(UnitDesc))]
	public class UnitDesc : EntityDesc
	{
		[field: SerializeField]
		public MovementDesc Movement { get; private set; }

		[field: SerializeField]
		public HealthDesc Health { get; private set; }

		[field: SerializeField]
		public bool Invulnerable { get; private set; }

		[field: SerializeField]
		public ManaDesc Mana { get; private set; }

		[field: SerializeField]
		public ExperienceDesc Experience { get; private set; }

		[field: SerializeField]
		public List<PropertyDesc> Properties { get; private set; }

		[field: SerializeField]
		public List<AbilityDesc> Abilities { get; private set; }

		[field: SerializeField]
		public List<LootDesc> Loot { get; private set; }

		[field: SerializeField]
		public InventoryDesc Inventory { get; private set; }
	}
}
