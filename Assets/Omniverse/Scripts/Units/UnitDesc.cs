using System.Collections.Generic;
using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Unit", fileName = nameof(UnitDesc))]
	public class UnitDesc: EntityDesc
	{
		[field: SerializeField]
		public Presentation Presentation { get; private set; }

		[field: SerializeField]
		public ExperienceDesc Experience { get; private set; }
		
		[field: SerializeField]
		public List<PropertyDesc> Properties { get; private set; }
		
		[field: SerializeField]
		public List<AbilityDesc> Abilities { get; private set; }
		
		[field: SerializeField]
		public List<LootDesc> Loot { get; private set; }
	}
}
