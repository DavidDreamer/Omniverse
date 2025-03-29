using Unity.Entities;

namespace Omniverse
{
	public class CastImmediateAbilityCommand : ImmediateCommand
	{
		private Entity AbilityEntity { get; }

		public CastImmediateAbilityCommand(DynamicEntity entity, Entity abilityEntity) : base(entity)
		{
			AbilityEntity = abilityEntity;
		}

		public override void Execute(EntityManager entityManager)
		{
			var input = entityManager.GetComponentData<AbilityInput>(AbilityEntity);
			input.Cast.Set();
			entityManager.SetComponentData(AbilityEntity, input);
		}
	}
}
