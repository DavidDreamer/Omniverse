using UnityEngine;

namespace Omniverse
{
	public class ApplyEffectAction : IAction<Entity>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(Entity actor, Entity target) => target.ApplyEffect(Effect);
	}
}
