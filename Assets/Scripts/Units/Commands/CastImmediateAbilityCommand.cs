using Omniverse.Abilities;

namespace Omniverse
{
	public class CastImmediateAbilityCommand : ImmediateCommand
	{
		private AbilityObsolete Ability { get; }

		public CastImmediateAbilityCommand(UnitObsolete unit, AbilityObsolete ability) : base(unit)
		{
			Ability = ability;
		}

		public override void Execute() => Ability.Cast(None.Instance);
	}
}
