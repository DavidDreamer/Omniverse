using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ApplyEffectAction : IAction<Entity>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(EntityManager entityManager, Entity actor, Entity target)
		{
			Effect effect = new()
			{
				Desc = Effect,
				Time = Effect.Duration
			};

			var effects = entityManager.GetBuffer<Effect>(target);

			effects.Add(effect);
		}
	}
}
