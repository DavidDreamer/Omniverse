using Omniverse.Abilities;

namespace Omniverse.Units
{
	public class CastEntityTargetAbilityCommand : CastAbilityCommand
	{
		private Entity Target { get; }

		public CastEntityTargetAbilityCommand(Unit unit, Ability ability, Entity target) : base(unit, ability)
		{
			Target = target;
		}

		public override void Start()
		{
			base.Start();

			Ability.OperationContext.Entities.Add(Target);
		}
	}
}
