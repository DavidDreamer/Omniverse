using UnityEngine;

namespace Omniverse.Actions
{
	public class ApplyEffect : Action<Unit, Unit>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public override void Perform(Unit actor, Unit target)
		{
			var effect = new Effect(Effect);
			target.ApplyEffect(effect);
		}
	}
}
