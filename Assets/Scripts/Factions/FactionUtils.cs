namespace Omniverse
{
	public static class FactionUtils
	{
		public static bool IsAllyFor(this IFactious first, IFactious second) => first.FactionID == second.FactionID;

		public static bool IsEnemyFor(this IFactious first, IFactious second) => first.FactionID != second.FactionID;

		public static bool Match(this FactiousFilter filter, IFactious first, IFactious second)
		{
			if (filter.HasFlag(FactiousFilter.Self) && first == second)
			{
				return true;
			}

			if (filter.HasFlag(FactiousFilter.Ally) && first.IsAllyFor(second))
			{
				return true;
			}

			if (filter.HasFlag(FactiousFilter.Enemy) && first.IsEnemyFor(second))
			{
				return true;
			}

			return false;
		}
	}
}
