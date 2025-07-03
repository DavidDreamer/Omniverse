using System.Linq;
using Omniverse;
using Omniverse.UI;
using Unity.Entities;

public class DebugFactionWidget : Widget
{
	public TMPro.TMP_Dropdown Dropdown;

	public override void Initialize(EntityManager entityManager)
	{
		base.Initialize(entityManager);

		var gameOptions = entityManager.GetSingletonManaged<GameOptions>();

		var factionNames = gameOptions.Factions.Select(faction => faction.Name).ToList();
		Dropdown.AddOptions(factionNames);
		Dropdown.onValueChanged.AddListener(ChangeFaction);
	}

	private void ChangeFaction(int faction)
	{
		var query = EntityManager.CreateEntityQuery(new ComponentType[] { typeof(Player) });
		var player = query.GetSingleton<Player>();
		player.FactionID = faction;
		query.SetSingleton(player);
		query.Dispose();
	}
}
