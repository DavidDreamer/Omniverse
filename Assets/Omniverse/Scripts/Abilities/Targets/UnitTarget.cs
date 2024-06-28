using Omniverse.Entities;
using UnityEngine;

namespace Omniverse.Abilities
{
	public class UnitTarget: EntityTarget
	{
		[field: SerializeField]
		public EntityTargetType Type { get; private set; }
	}
}
