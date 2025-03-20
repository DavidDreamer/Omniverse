using Unity.Entities;

namespace Omniverse
{
	public class CastImmediateAbilityCommand : ImmediateCommand
	{
		private Ability Ability { get; }

		public CastImmediateAbilityCommand(Entity entity, Ability ability) : base(entity)
		{
			Ability = ability;
		}

		public override void Execute() => Ability.Cast(Entity, None.Instance);
	}
}
