using System;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class CastingDesc
	{
		[field: SerializeField]
		public float Range { get; private set; }

		[field: SerializeField]
		public float Time { get; private set; }

		[field: SerializeField]
		public bool Repetitive { get; private set; }

		[field: SerializeField]
		public string AnimationTrigger { get; private set; }
	}
}
