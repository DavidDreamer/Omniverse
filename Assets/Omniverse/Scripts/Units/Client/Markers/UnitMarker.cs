using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

namespace Omniverse.Units.Rendering
{
	public class UnitMarker: MonoBehaviour
	{
		[field: SerializeField]
		private UnitMarkerConfig Config { get; set; }

		[field: SerializeField]
		private DecalProjector DecalProjector { get; set; }

		[Inject]
		private IPlayer Player { get; set; }

		public void Initialize(Unit unit)
		{
			DecalProjector.material = Player.FactionID == unit.FactionID ? Config.AllyMaterial : Config.EnemyMaterial;
		}
	}
}
