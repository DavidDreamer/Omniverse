using Omniverse.Units;

namespace Omniverse
{
	public interface ITargetProvider<TTarget>
	{
		TTarget Get(Unit actor);
	}
}
