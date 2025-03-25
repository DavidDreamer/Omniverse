using System.Collections.Generic;
using Unity.Entities;

namespace Omniverse
{
	public interface ITargetConverter<in TTargetIn, out TTargetOut>
	{
		IEnumerable<TTargetOut> Convert(EntityManager entityManager, DynamicEntity actor, TTargetIn input);
	}
}
