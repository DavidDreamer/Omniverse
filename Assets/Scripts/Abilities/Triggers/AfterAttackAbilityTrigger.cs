using UnityEngine;

namespace Omniverse.Abilities
{
	public class AfterAttackAbilityTrigger : IAbilityTrigger
	{
		[field: SerializeReference]
		public IOperation<UnitObsolete> Operations { get; private set; }

		private OmniverseEntity Entity { get; set; }

		public void Listen(OmniverseEntity entity)
		{
			Entity = entity;

			entity.Attack.Performed += OnAttackPerformed;
		}

		private void OnAttackPerformed(OmniverseEntity target)
		{
			Operations.Perform(Entity, (UnitObsolete)target);
		}
	}
}
