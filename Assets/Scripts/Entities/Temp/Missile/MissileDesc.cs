using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Missile")]
	public class MissileDesc : EntityDesc
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		[field: SerializeField]
		public float Range { get; private set; }

		[field: SerializeField]
		public float Speed { get; private set; }

		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public MultiAction HitAction { get; private set; }
	}
}
