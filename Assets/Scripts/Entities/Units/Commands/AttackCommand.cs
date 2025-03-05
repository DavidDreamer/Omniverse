using UnityEngine.AI;
using static Omniverse.OmniverseEntity;

namespace Omniverse
{
	public class AttackCommand : Command
	{
		private Unit Target { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public AttackCommand(Unit unit, Unit target) : base(unit)
		{
			Target = target;
		}

		public override bool Tick(float deltaTime)
		{
			AttackModule attack = Unit.Attack;

			if (!attack.InProcess)
			{
				if (attack.TargetIsInRange(Target))
				{
					NavMeshAgent.ResetPath();
					attack.BeginAttack();
				}
			}

			if (attack.InProcess)
			{
				attack.Tick(Target, deltaTime);
			}
			else
			{
				NavMeshAgent.destination = Target.NavMeshAgent.nextPosition;
			}

			return false;
		}

		public override void Cleanup()
		{
			base.Cleanup();

			NavMeshAgent.ResetPath();
		}
	}
}
