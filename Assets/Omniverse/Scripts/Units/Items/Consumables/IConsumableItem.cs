using Omniverse.Items;
using Omniverse.Units;

namespace Omniverse
{
	public interface IConsumableItem
	{
		bool CanBeConsumed(Unit unit);

		void OnConsumed(Unit unit);
	}
}
