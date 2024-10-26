using System.Collections.Generic;

namespace Omniverse
{
	public interface ITargetConverter<in TTargetIn, out TTargetOut>
	{
		IEnumerable<TTargetOut> Convert(Entity actor, TTargetIn input);
	}
}
