using UnityEngine;

namespace Omniverse.Abilities
{
	public abstract class EntityTarget: ITarget
	{
		[field: SerializeField]
		public float Range { get; private set; }
	}
}
