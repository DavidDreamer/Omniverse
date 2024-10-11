using UnityEngine;

namespace Omniverse
{
	[CreateAssetMenu(menuName = "Omniverse/Settings/Game")]
	public class GameSettings : ScriptableObject
	{
		[field: SerializeField]
		public ResourceDesc[] Resources { get; private set; }

		[field: SerializeField]
		public FactionDesc[] Factions { get; private set; }

		[field: SerializeField]
		public MapSettings MapSettings { get; private set; }

		[field: SerializeField]
		public FogOfWarConfig FogOfWarConfig { get; private set; }
	}
}
