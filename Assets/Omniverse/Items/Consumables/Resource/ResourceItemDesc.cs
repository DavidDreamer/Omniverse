using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu]
	public class ResourceItemDesc: ItemDesc
	{
		[field: SerializeField]
		[field: Resource]
		public int ResourceID { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
