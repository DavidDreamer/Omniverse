using UnityEngine;

namespace Omniverse.Abilities
{
	public class UnitTarget : ITarget
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }
	}
}
