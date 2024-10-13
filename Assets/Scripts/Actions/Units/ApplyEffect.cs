using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ApplyEffect : ScriptableObject, IAction<Unit, Unit>
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
