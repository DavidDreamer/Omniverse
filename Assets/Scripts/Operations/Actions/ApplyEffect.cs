using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ApplyEffect : IAction<Unit>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(Unit actor, Unit target)
		{
			var effect = new Effect(Effect);
			target.ApplyEffect(effect);
		}
	}
}
