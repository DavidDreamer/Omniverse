using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ApplyEffectAction : IAction<DynamicEntity>
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public void Perform(EntityManager entityManager, DynamicEntity actor, DynamicEntity target)
		{
			//TODO ECS
			//target.ApplyEffect(Effect);
		}
	}
}
