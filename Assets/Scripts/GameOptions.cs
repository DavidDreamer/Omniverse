using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Omniverse
{
	public struct FactionsData : IComponentData
	{
		public NativeHashMap<int, NativeArray<int>> Resources;
	}

	[Serializable]
	public class GameOptions : IComponentData
	{
		public int2 MapSize;
		public FogOfWarMode FogOfWarMode;
		public ResourceDesc[] Resources;
		public FactionDesc[] Factions;
	}
}
