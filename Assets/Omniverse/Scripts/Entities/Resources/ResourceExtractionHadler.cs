using VContainer;

namespace Omniverse
{
	public class ResourceExtractionHadler
	{
		[Inject]
		private FactionManager FactionManager { get; set; }

		public void Extract(IEntity entity, ResourceSource resourceSource, int amount)
		{
			resourceSource.Extract(ref amount);
			int factionID = entity.FactionID;
			Faction faction = FactionManager.Factions[factionID];
			faction.ChangeResource(resourceSource.Desc.Resource, amount);
		}
	}
}
