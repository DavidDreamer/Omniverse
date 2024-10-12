using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;

namespace Omniverse.Actions
{
	public static class ActionDescUtils
	{
		public static IEnumerable<Unit> Units(this ActionContext context) => context.Entities.OfType<Unit>();
	}
}
