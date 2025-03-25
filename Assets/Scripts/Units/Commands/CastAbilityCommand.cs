using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	//TODO ECS
	public abstract class CastAbilityCommand : Command
	{
		public Ability Ability { get; }

		//public override bool IsRepeatable => Ability.Desc.Casting.Repetitive;

		protected CastAbilityCommand(DynamicEntity entity, Ability ability) : base(entity)
		{
			Ability = ability;
		}

		public override void Start(ref SystemState state)
		{
			base.Start(ref state);

			Ability.Casting.Start();
		}

		public override bool Tick(ref SystemState state)
		{
			AbilityCastError error = Ability.CanBeCasted(Entity);

			if (error is not AbilityCastError.None)
			{
				return true;
			}

			Ability.Casting.Tick(state.EntityManager.World.Time.DeltaTime);

			if (!Ability.Casting.Finished)
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

			Ability.Casting.Reset();
		}
	}

	public class CastAbilityCommand<TTarget> : CastAbilityCommand
	{
		private TTarget Target { get; }

		public CastAbilityCommand(DynamicEntity entity, Ability ability, TTarget target) : base(entity, ability)
		{
			Target = target;
		}

		protected override void Cast(ref SystemState state)
		{
			base.Cast(ref state);

			Ability.Cast(state.EntityManager, Entity, Target);
		}
	}
}
