using Omniverse.Units;
using VContainer;

namespace Omniverse
{
	public class ResourceItem: Item<ResourceItemDesc>, IConsumableItem
	{
		[Inject]
		private FactionManager FactionManager { get; set; }

		public ResourceItem(ResourceItemDesc desc): base(desc)
		{
		}
		
		public bool CanBeConsumed(Unit unit) => true;

		public void OnConsumed(Unit unit)
		{
			Faction faction = FactionManager.Factions[unit.FactionID];
			faction.ChangeCurrency(Desc.Resource, Desc.Amount);
		}
	}
}
