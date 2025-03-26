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

			//TODO ECS
			//Ability.Cast(entityManager, Entity, None.Instance);
		}
	}
}
