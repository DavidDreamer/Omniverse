using System.Collections.Generic;
using System.Linq;
using Omniverse.Entities.Units;

namespace Omniverse.Actions
{
	public static class ActionDescUtils
	{
		public static IEnumerable<Unit> Units(this ExecutionContext context) => context.Entities.OfType<Unit>();
	}
}
