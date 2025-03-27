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
			var cooldown = entityManager.GetComponentData<Cooldown>(AbilityEntity);
			cooldown.TimeLeft = cooldown.Time;
			entityManager.SetComponentData(AbilityEntity, cooldown);

			var abilityActiveOperation = entityManager.GetComponentObject<AbilityActiveOperation>(AbilityEntity);
			var operation = (IOperation<None>)abilityActiveOperation.Operation;
			operation.Perform(entityManager, Entity, None.Instance);
		}
	}
}
