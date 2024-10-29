using UnityEngine;

namespace Omniverse.Abilities
{
	public class AfterAttackAbilityTrigger : IAbilityTrigger
	{
		[field: SerializeReference]
		public IOperation<Unit> Operations { get; private set; }

		private Entity Entity { get; set; }

		public void Listen(Entity entity)
		{
			Entity = entity;

			entity.Attack.Performed += OnAttackPerformed;
		}

		private void OnAttackPerformed(Entity target)
		{
			Operations.Perform(Entity, (Unit)target);
		}
	}
}
