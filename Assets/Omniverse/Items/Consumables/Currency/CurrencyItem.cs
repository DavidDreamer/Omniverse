using VContainer;
using Object = UnityEngine.Object;

namespace Omniverse
{
	public class CurrencyItem: Item<CurrencyItemDesc>, IConsumableItem
	{
		[Inject]
		private FactionManager FactionManager { get; set; }

		public CurrencyItem(CurrencyItemDesc desc): base(desc)
		{
		}
		
		public bool CanBeConsumed(Unit unit) => true;

		public void OnConsumed(Unit unit)
		{
			Faction faction = FactionManager.Factions[unit.FactionID];
			faction.ChangeCurrency(Desc.CurrencyID, Desc.Amount);

			Object.Destroy(Presenter.gameObject);
		}
	}
}
