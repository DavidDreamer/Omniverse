using UnityEngine.AI;

namespace Omniverse.Units
{
	public class FollowCommand : Command
	{
		private Unit Target { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public FollowCommand(Unit unit, Unit target) : base(unit)
		{
			Target = target;
		}

		public override bool Tick(float deltaTime)
		{
			NavMeshAgent.destination = Target.NavMeshAgent.nextPosition;
			return false;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.isStopped = true;
			NavMeshAgent.ResetPath();
		}
	}
}
