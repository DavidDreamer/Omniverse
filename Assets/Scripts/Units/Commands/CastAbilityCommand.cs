using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	//TODO ECS
	public abstract class CastAbilityCommand : Command
	{
		public Entity AbilityEntity;

		public Ability Ability;

		//public override bool IsRepeatable => Ability.Desc.Casting.Repetitive;

		protected CastAbilityCommand(DynamicEntity entity, Entity abilityEntity) : base(entity)
		{
			AbilityEntity = abilityEntity;
		}

		public override void Start(ref SystemState state)
		{
			base.Start(ref state);

			var casting = state.EntityManager.GetComponentData<Casting>(AbilityEntity);
			casting.Start();
			state.EntityManager.SetComponentData(AbilityEntity, casting);
		}

		public override bool Tick(ref SystemState state)
		{
			AbilityCastError error = Ability.CanBeCasted(Entity);

			if (error is not AbilityCastError.None)
			{
				return true;
			}

			var casting = state.EntityManager.GetComponentData<Casting>(AbilityEntity);
			casting.Tick(state.EntityManager.World.Time.DeltaTime);
			state.EntityManager.SetComponentData(AbilityEntity, casting);

			if (!casting.Finished)
			{
				return false;
			}

			Cast(ref state);

			return true;
		}

		protected virtual void Cast(ref SystemState state)
		{
			//foreach (CostDesc cost in Ability.Desc.Cost)
			//{
			//	Unit.Properties[cost.PropertyID].Modify(cost.PropertyModifier);
			//}
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var casting = state.EntityManager.GetComponentData<Casting>(AbilityEntity);
			casting.Reset();
			state.EntityManager.SetComponentData(AbilityEntity, casting);
		}
	}

	public class CastAbilityCommand<TTarget> : CastAbilityCommand
	{
		private TTarget Target { get; }

		public CastAbilityCommand(DynamicEntity entity, Entity abilityEntity, TTarget target) : base(entity, abilityEntity)
		{
			Target = target;
		}

		protected override void Cast(ref SystemState state)
		{
			base.Cast(ref state);

			var entityManager = state.EntityManager;

			var cooldown = entityManager.GetComponentData<Cooldown>(AbilityEntity);
			cooldown.TimeLeft = cooldown.Time;
			entityManager.SetComponentData(AbilityEntity, cooldown);

			var abilityActiveOperation = entityManager.GetComponentObject<AbilityActiveOperation>(AbilityEntity);
			var operation = (IOperation<TTarget>)abilityActiveOperation.Operation;
			operation.Perform(entityManager, Entity, Target);
		}
	}
}
