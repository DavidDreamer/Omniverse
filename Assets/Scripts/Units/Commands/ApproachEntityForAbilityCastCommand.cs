using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse
{
	public class ApproachEntityForAbilityCastCommand : Command
	{
		public AbilityObsolete Ability { get; }

		public OmniverseEntity Target { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public ApproachEntityForAbilityCastCommand(UnitObsolete unit, AbilityObsolete ability, OmniverseEntity target) : base(unit)
		{
			Ability = ability;
			Target = target;
		}

		public override bool Tick(float deltaTime)
		{
			NavMeshAgent.destination = Target.transform.position;
			return Vector3.Distance(Target.transform.position, NavMeshAgent.nextPosition) <= Ability.Desc.Casting.Range;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.ResetPath();
		}
	}
}
