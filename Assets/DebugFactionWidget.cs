using System.Linq;
using Omniverse;
using Unity.Entities;
using UnityEngine;

public class DebugFactionWidget : MonoBehaviour
{
	public TMPro.TMP_Dropdown Dropdown;

	void Start()
	{
		var gameOptions = ECSUtils.GetSingletonManaged<GameOptions>();

		var factionNames = gameOptions.Factions.Select(faction => faction.Name).ToList();
		Dropdown.AddOptions(factionNames);
		Dropdown.onValueChanged.AddListener(ChangeFaction);
	}

	private void ChangeFaction(int faction)
	{
		var query = ECSUtils.ClientWorld.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(Player) });
		var player = query.GetSingleton<Player>();
		player.FactionID = faction;
		query.SetSingleton(player);
		query.Dispose();
	}
}
