using UnityEngine;

namespace Omniverse.Abilities
{
	public enum VectorTargetMode
	{
		Position,
		Direction
	}

	public class VectorTarget : ITarget
	{
		[field: SerializeField]
		public VectorTargetMode Mode { get; private set; }
	}
}
