using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Misc/Map Settings")]
	public class MapSettings : ScriptableObject
	{
		[field: SerializeField]
		public Vector2Int Size { get; private set; }
	}
}
