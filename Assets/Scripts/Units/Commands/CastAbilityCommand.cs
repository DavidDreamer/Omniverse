using Omniverse.Abilities;
using Unity.Entities;
using UnityEngine;

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
			AbilityCastError error = AbilityEntity.CanBeCasted(state.EntityManager);

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

			var input = entityManager.GetComponentData<AbilityInput>(AbilityEntity);
			input.Cast.Set();

			if (typeof(TTarget) == typeof(Vector3))
			{
				input.Vector = (Vector3)(object)Target;
			}
			entityManager.SetComponentData(AbilityEntity, input);
		}
	}
}
