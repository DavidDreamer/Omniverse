using System.Collections.Generic;

namespace Omniverse
{
	public interface ITargetConverter<TTargetIn, TTargetOut>
	{
		IEnumerable<TTargetOut> Convert(Entity entity, TTargetIn input);
	}
}
