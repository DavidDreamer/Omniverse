using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class FogOfWarConfig
	{
		[field: SerializeField]
		public bool Enabled { get; private set; }

		[field: SerializeField]
		public bool Explored { get; private set; }
	}
}
