using UnityEngine;

namespace Omniverse.Camera
{
	[CreateAssetMenu(menuName = "Omniverse/Config/UnitSelector")]
	public class UnitSelectorConfig: ScriptableObject
	{
		[field: SerializeField]
		public GameObject HoverPrefab { get; private set; }
		
		[field: SerializeField]
		public GameObject SelectionPrefab { get; private set; }
	}
}
