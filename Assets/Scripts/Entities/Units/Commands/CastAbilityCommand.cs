using Omniverse.Abilities;

namespace Omniverse
{
	public class CastAbilityCommand : Command
	{
		public Ability Ability { get; }

		public override bool IsRepeatable => Ability.Desc.Casting.Repetitive;

		public CastAbilityCommand(Unit unit, Ability ability) : base(unit)
		{
			Ability = ability;
		}

		public override void Start()
		{
			base.Start();

			Ability.Casting.Start();
		}

		public override bool Tick(float deltaTime)
		{
			//AbilityCastError error = Ability.CanBeCasted(Unit);

			//if (error is not AbilityCastError.None)
			//{
			//	return true;
			//}

			Ability.Casting.Tick(deltaTime);

			if (!Ability.Casting.Finished)
			{
				return false;
			}

			foreach (CostDesc cost in Ability.Desc.Cost)
			{
				Unit.Properties[cost.PropertyID].Modify(cost.PropertyModifier);
			}

			Cast();

			return true;
		}

		protected virtual void Cast()
		{
			Ability.Cast(None.Instance);
		}

		public override void Cleanup()
		{
			base.Cleanup();

			Ability.Casting.Reset();
		}
	}

	public class CastAbilityCommand<TTarget> : CastAbilityCommand
	{
		private TTarget Target { get; }

		public CastAbilityCommand(Unit unit, Ability ability, TTarget target) : base(unit, ability)
		{
			Target = target;
		}

		protected override void Cast()
		{
			Ability.Cast(Target);
		}
	}
}
