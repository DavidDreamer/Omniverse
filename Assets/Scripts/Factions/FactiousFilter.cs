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
		public static bool Match(this FactiousFilter filter, int source, int target)
		{
			if (source == target)
			{
				return filter.HasFlag(FactiousFilter.Ally);
			}
			else
			{
				return filter.HasFlag(FactiousFilter.Enemy);
			}
		}

		public static IEnumerable<Faction> Match(this IEnumerable<Faction> items, Faction source, FactiousFilter filter)
		{
			foreach (Faction item in items)
			{
				if (filter.Match(source.ID, item.ID))
				{
					yield return item;
				}
			}
		}
	}
}
