using Omniverse.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse.Units
{
	public class ApproachPositionForAbilityCastCommand : Command
	{
		public Ability Ability { get; }

		public Vector3 Position { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public ApproachPositionForAbilityCastCommand(Unit unit, Ability ability, Vector3 position) : base(unit)
		{
			Ability = ability;
			Position = position;
		}

		public override void Start()
		{
			base.Start();

			NavMeshAgent.destination = Position;
		}

		public override bool Tick(float deltaTime)
		{
			return Vector3.Distance(Position, NavMeshAgent.nextPosition) <= Ability.Desc.Target.Range;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.ResetPath();
		}
	}
}
