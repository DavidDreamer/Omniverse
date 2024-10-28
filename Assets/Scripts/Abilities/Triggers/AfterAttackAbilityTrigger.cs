using Dreambox.Core;
using UnityEngine;

namespace Omniverse.Abilities
{
	public class AfterAttackAbilityTrigger : IAbilityTrigger
	{
		[field: SerializeReference]
		[field: Versatile(typeof(IAction<Entity>))]
		public IAction<Entity> Action { get; private set; }

		private Entity Entity { get; set; }

		public void Listen(Entity entity)
		{
			Entity = entity;

			entity.Attack.Performed += OnAttackPerformed;
		}

		private void OnAttackPerformed(Entity target)
		{
			Action.Perform(Entity, target);
		}
	}
}
