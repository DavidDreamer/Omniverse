using Omniverse.Abilities;
using Unity.Entities;

namespace Omniverse
{
	//TODO ECS
	public abstract class CastAbilityCommand : Command
	{
		public Ability Ability { get; }

		//public override bool IsRepeatable => Ability.Desc.Casting.Repetitive;

		protected CastAbilityCommand(Entity entity, Ability ability) : base(entity)
		{
			Ability = ability;
		}

		//public override void Start()
		//{
		//	base.Start();

		//	Ability.Casting.Start();
		//}

		public override bool Tick(ref SystemState state)
		{
			//AbilityCastError error = Ability.CanBeCasted(Unit);

			//if (error is not AbilityCastError.None)
			//{
			//	return true;
			//}

			//Ability.Casting.Tick(deltaTime);

			//if (!Ability.Casting.Finished)
			//{
			//	return false;
			//}

			//foreach (CostDesc cost in Ability.Desc.Cost)
			//{
			//	Unit.Properties[cost.PropertyID].Modify(cost.PropertyModifier);
			//}

			Cast();

			return true;
		}

		protected abstract void Cast();

		//public override void Cleanup()
		//{
		//	base.Cleanup();

		//	Ability.Casting.Reset();
		//}
	}

	public class CastAbilityCommand<TTarget> : CastAbilityCommand
	{
		private TTarget Target { get; }

		public CastAbilityCommand(Entity entity, Ability ability, TTarget target) : base(entity, ability)
		{
			Target = target;
		}

		protected override void Cast()
		{
			//Ability.Cast(Target);
		}
	}
}
