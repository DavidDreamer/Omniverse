using UnityEngine;

namespace Omniverse.Abilities
{
	public class PointTargetDesc : ITargetDesc
	{
		[field: SerializeField]
		public float Range { get; private set; }
	}

	public class PointTarget : ITarget
	{
		public PointTargetDesc Desc { get; }

		public Vector3 Value { get; set; }

		public PointTarget(PointTargetDesc desc)
		{
			Desc = desc;
		}
	}
}
