using System;
using System.Collections.Generic;

namespace Omniverse
{
	[Flags]
	public enum FactiousFilter
	{
		Self = 0x1,
		Ally = 0x2,
		Enemy = 0x4
	}

	public static class FactiousFilterUtils
	{
		public static bool Match(this FactiousFilter filter, IFactious source, IFactious target)
		{
			if (source == target)
			{
				return filter.HasFlag(FactiousFilter.Self);
			}

			if (source.IsAllyFor(target))
			{
				return filter.HasFlag(FactiousFilter.Ally);
			}
			else
			{
				return filter.HasFlag(FactiousFilter.Enemy);
			}
		}

		public static IEnumerable<IFactious> Match(this IEnumerable<IFactious> items, IFactious source, FactiousFilter filter)
		{
			foreach (IFactious item in items)
			{
				if (filter.Match(source, item))
				{
					yield return item;
				}
			}
		}
	}
}
