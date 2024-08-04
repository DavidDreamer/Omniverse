using UnityEngine;

namespace Omniverse.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Unit Manager", fileName = nameof(UnitManagerConfig))]
	public class UnitManagerConfig : ScriptableObject
	{
		[field: SerializeField]
		public Unit UnitPrefab { get; private set; }

		[field: SerializeField]
		public float DespawnDelay { get; private set; }
	}
}
