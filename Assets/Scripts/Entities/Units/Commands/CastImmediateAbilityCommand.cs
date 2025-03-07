using Omniverse.Abilities;

namespace Omniverse
{
	public class CastImmediateAbilityCommand : ImmediateCommand
	{
		private Ability Ability { get; }

		public CastImmediateAbilityCommand(UnitObsolete unit, Ability ability) : base(unit)
		{
			Ability = ability;
		}

		public override void Execute() => Ability.Cast(None.Instance);
	}
}
