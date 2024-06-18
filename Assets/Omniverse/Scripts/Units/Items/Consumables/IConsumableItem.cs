using Omniverse.Items;
using Omniverse.Units;

namespace Omniverse
{
	public interface IConsumableItem: IItem
	{
		bool CanBeConsumed(Unit unit);

		void OnConsumed(Unit unit);
	}
}
