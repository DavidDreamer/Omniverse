using Unity.Entities;

namespace Omniverse
{
	public class CastImmediateAbilityCommand : ImmediateCommand
	{
		private Ability Ability { get; }

		public CastImmediateAbilityCommand(DynamicEntity entity, Ability ability) : base(entity)
		{
			Ability = ability;
		}

		public override void Execute(EntityManager entityManager) => Ability.Cast(entityManager, Entity, None.Instance);
	}
}
