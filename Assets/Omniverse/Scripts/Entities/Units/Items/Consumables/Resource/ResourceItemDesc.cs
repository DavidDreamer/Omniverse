using Omniverse.Entities.Items;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Resource Item")]
	public class ResourceItemDesc: ItemDesc
	{
		[field: SerializeField]
		public ResourceDesc Resource { get; private set; }
		
		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
