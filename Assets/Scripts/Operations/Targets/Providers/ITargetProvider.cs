using System.Collections.Generic;

namespace Omniverse
{
	public interface ITargetProvider<TTarget>
	{
		IEnumerable<TTarget> Get(Entity actor);
	}
}
