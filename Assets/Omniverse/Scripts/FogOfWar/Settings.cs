using System;
using UnityEngine;

namespace Omniverse.FogOfWar
{
	[Serializable]
	public class Settings
	{
		[field: SerializeField]
		public bool Enabled { get; private set; }

		[field: SerializeField]
		public bool Explored { get; private set; }
	}
}
