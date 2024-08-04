using UnityEngine;

namespace Omniverse.Input
{
	[CreateAssetMenu(menuName = "Omniverse/Config/UnitControllerConfig")]
	public class UnitControllerConfig : ScriptableObject
	{
		[field: SerializeField]
		public NavigationPoint NavigationPointPrefab { get; private set; }

		[field: SerializeField]
		public float NavigationPointDuration { get; private set; }
	}
}
