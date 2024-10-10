using UnityEngine;
using UnityEngine.AI;

namespace Omniverse.Units
{
	public class MoveCommand : Command
	{
		public Vector3 Position { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public override bool IsCompleted => Vector3.Distance(Position, NavMeshAgent.nextPosition) <= NavMeshAgent.stoppingDistance;

		public MoveCommand(Unit unit, Vector3 position) : base(unit)
		{
			Position = position;
		}

		public override void Start()
		{
			base.Start();

			NavMeshAgent.isStopped = false;
			NavMeshAgent.destination = Position;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.isStopped = true;
			NavMeshAgent.ResetPath();
		}
	}
}
