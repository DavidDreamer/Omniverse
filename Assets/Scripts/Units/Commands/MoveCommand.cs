using UnityEngine;
using UnityEngine.AI;

namespace Omniverse
{
	public class MoveCommand : Command
	{
		public Vector3 Position { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public MoveCommand(UnitObsolete unit, Vector3 position) : base(unit)
		{
			Position = position;
		}

		public override void Start()
		{
			base.Start();

			NavMeshAgent.destination = Position;
		}

		public override bool Tick(float deltaTime)
		{
			return Vector3.Distance(Position, NavMeshAgent.nextPosition) <= NavMeshAgent.stoppingDistance;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.ResetPath();
		}
	}
}
