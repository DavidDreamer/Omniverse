using UnityEngine;

namespace Omniverse.Entities.Units
{
	[CreateAssetMenu(menuName = "Omniverse/Config/Unit Manager", fileName = nameof(UnitManagerConfig))]
	public class UnitManagerConfig: ScriptableObject
	{
		[field: SerializeField]
		public float DespawnDelay { get; private set; }
	}
}
