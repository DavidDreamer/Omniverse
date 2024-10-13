using UnityEngine;

namespace Omniverse
{
	public class ChainDesc : EntityDesc
	{
		[field: SerializeField]
		public float TimeInterval { get; private set; }
	}
}
