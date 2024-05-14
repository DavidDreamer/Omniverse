using System;
using UnityEngine;

namespace Omniverse.Units
{
	[Serializable]
	public struct UnitSpawnData
	{
		[field: SerializeField]
		public UnitDesc UnitDesc { get; private set; }
		
		[field: SerializeField]
		[field: Faction]
		public int FactionID { get; private set; }
	}
}
