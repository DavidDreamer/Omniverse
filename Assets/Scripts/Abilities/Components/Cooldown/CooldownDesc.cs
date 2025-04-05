using System;
using UnityEngine;

namespace Omniverse.Abilities
{
	[Serializable]
	public class CooldownDesc
	{
		[field: SerializeField]
		public float Duration { get; private set; }
	}
}
