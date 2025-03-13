using UnityEngine.AI;

namespace Omniverse
{
	public class FollowCommand : Command
	{
		private UnitObsolete Target { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public FollowCommand(UnitObsolete unit, UnitObsolete target) : base(unit)
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

			NavMeshAgent.ResetPath();
		}
	}
}
