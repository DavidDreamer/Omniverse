using UnityEngine;

namespace Omniverse
{
	public class ApplyEffectAction : IAction<Entity>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(Entity actor, Entity target)
		{
			var effect = new Effect(Effect);
			target.ApplyEffect(effect);
		}
	}
}
