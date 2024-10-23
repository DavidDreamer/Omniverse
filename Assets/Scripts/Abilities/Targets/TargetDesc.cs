using System;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class TargetDesc
	{
		[field: SerializeField]
		public TargetType Type { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		[field: SerializeField]
		public ResourceDesc[] ResourceSources { get; private set; }
	}
}
