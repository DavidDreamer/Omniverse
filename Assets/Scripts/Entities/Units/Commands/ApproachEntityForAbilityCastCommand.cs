using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse
{
	public class ApproachEntityForAbilityCastCommand : Command
	{
		public Ability Ability { get; }

		public Entity Target { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public ApproachEntityForAbilityCastCommand(Unit unit, Ability ability, Entity target) : base(unit)
		{
			Ability = ability;
			Target = target;
		}

		public override bool Tick(float deltaTime)
		{
			NavMeshAgent.destination = Target.transform.position;
			return Vector3.Distance(Target.transform.position, NavMeshAgent.nextPosition) <= Ability.Desc.Target.Range;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.ResetPath();
		}
	}
}
