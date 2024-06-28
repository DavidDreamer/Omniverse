using UnityEngine;

namespace Omniverse.Abilities
{
	public class ResourceSourceTarget: EntityTarget
	{
		[field: SerializeField]
		public ResourceDesc[] Resources { get; private set; }
	}
}
