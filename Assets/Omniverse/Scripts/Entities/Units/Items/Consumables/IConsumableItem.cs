using Omniverse.Entities.Items;
using Omniverse.Entities.Units;

namespace Omniverse
{
	public interface IConsumableItem
	{
		bool CanBeConsumed(Unit unit);

		void OnConsumed(Unit unit);
	}
}
