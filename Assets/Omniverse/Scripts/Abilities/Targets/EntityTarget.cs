using Omniverse.Entities;
using UnityEngine;

namespace Omniverse.Abilities
{
	public class EntityTarget: ITarget
	{
		[field: SerializeField]
		public float Range { get; private set; }
		
		[field: SerializeField]
		public ResourceDesc[] Resources { get; private set; }
		
		[field: SerializeField]
		public EntityTargetType Type { get; private set; }
	}
}
