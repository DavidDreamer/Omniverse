using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace Omniverse
{
	[Preserve]
	public class FactionManager
	{
		public Dictionary<int, Faction> Factions { get; } = new();

		public FactionManager(List<FactionDesc> factionDescs)
		{
			for (var i = 0; i < factionDescs.Count; i++)
			{
				FactionDesc desc = factionDescs[i];
				Factions.Add(i, new Faction(i, desc));
			}
		}
	}

	public static class UnitFactionUtils
	{
		public static bool IsAllyFor(this Unit source, Unit target) => source.FactionID == target.FactionID;
		
		public static bool IsEnemyFor(this Unit source, Unit target) => source.FactionID != target.FactionID;
	}
}
