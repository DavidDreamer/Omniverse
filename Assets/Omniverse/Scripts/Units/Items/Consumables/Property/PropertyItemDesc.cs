using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Desc/Property Item")]
	public class PropertyItemDesc: ItemDesc
	{
		[field: SerializeField]
		public PropertyTag PropertyTag { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }
	}
}
