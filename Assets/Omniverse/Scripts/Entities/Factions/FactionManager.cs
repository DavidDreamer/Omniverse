using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace Omniverse
{
	[UnityEngine.Scripting.Preserve]
	public class FactionManager: IInitializable
	{
		public Dictionary<int, Faction> Factions { get; } = new();
		
		[Inject]
		private FactionDesc[] FactionDescs { get; set; }
		
		[Inject]
		private ResourceDesc[] ResourceDescs { get; set; }
		
		public void Initialize()
		{
			for (var i = 0; i < FactionDescs.Length; i++)
			{
				FactionDesc desc = FactionDescs[i];
				Factions.Add(i, new Faction(i, desc, ResourceDescs));
			}
		}
	}

	public static class UnitFactionUtils
	{
		public static bool IsAllyFor(this IFactious first, IFactious second) => first.FactionID == second.FactionID;
		
		public static bool IsEnemyFor(this IFactious first, IFactious second) => first.FactionID != second.FactionID;
	}
}
