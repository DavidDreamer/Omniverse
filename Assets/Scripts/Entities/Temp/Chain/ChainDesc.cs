using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Chain")]
	public class ChainDesc : EntityDesc
	{
		[field: SerializeField]
		public MultiAction Action { get; private set; }

		[field: SerializeField]
		public float BounceRange { get; private set; }

		[field: SerializeField]
		public float BounceInterval { get; private set; }
	}
}
