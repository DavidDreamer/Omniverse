namespace Omniverse
{
	public static class FactionUtils
	{
		public static bool IsAllyFor(this Faction first, Faction second) => first.ID == second.ID;

		public static bool IsEnemyFor(this Faction first, Faction second) => first.ID != second.ID;
	}
}
