namespace Omniverse
{
	public static class FactionUtils
	{
		public static bool IsAllyFor(this IFactious first, IFactious second) => first.FactionID == second.FactionID;

		public static bool IsEnemyFor(this IFactious first, IFactious second) => first.FactionID != second.FactionID;
	}
}
