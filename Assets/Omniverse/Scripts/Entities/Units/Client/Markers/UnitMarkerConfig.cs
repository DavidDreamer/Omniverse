using UnityEngine;

namespace Omniverse.Entities.Units.Client
{
	[CreateAssetMenu(menuName = "Omniverse/Client/Unit Marker", fileName = nameof(UnitMarkerConfig))]
	public class UnitMarkerConfig: ScriptableObject
	{
		[field: SerializeField]
		public Material AllyMaterial { get; private set; }
		
		[field: SerializeField]
		public Material EnemyMaterial { get; private set; }
	}
}
