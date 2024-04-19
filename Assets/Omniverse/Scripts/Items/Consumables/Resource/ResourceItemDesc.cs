using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Resource Item")]
	public class ResourceItemDesc: ItemDesc
	{
		[field: SerializeField]
		[field: Resource]
		public int ResourceID { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
