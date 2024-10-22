using Omniverse.Units;

namespace Omniverse
{
	public interface IAction<TTarget>
	{
		public void Perform(Unit actor, TTarget target);
	}

}
