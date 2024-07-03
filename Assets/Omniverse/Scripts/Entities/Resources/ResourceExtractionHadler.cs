using VContainer;

namespace Omniverse
{
	public class ResourceExtractionHadler
	{
		[Inject]
		private FactionManager FactionManager { get; set; }

		public void Extract(ResourceSource resourceSource, int amount, int factionID)
		{
			resourceSource.Extract(ref amount);
			Faction faction = FactionManager.Factions[factionID];
			faction.ChangeResource(resourceSource.Desc.Resource, amount);
		}
	}
}
