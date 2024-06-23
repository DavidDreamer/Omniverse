using System;
using Omniverse.Entities.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public class ApplyEffectDesc: IActionDesc
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }
	}
}
