using UnityEngine;

namespace Omniverse
{
	public class ApplyEffectAction : IAction<OmniverseEntity>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(OmniverseEntity actor, OmniverseEntity target) => target.ApplyEffect(Effect);
	}
}
