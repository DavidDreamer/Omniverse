using Unity.Entities;

namespace Omniverse
{
	public class AttackCommand : Command
	{
		private DynamicEntity Target { get; }

		public AttackCommand(DynamicEntity entity, DynamicEntity target) : base(entity)
		{
			Target = target;
		}

		public override bool Tick(ref SystemState state)
		{
			//AttackModule attack = Unit.Attack;

			//if (!attack.InProcess)
			//{
			//	if (attack.TargetIsInRange(Target))
			//	{
			//		NavMeshAgent.ResetPath();
			//		attack.BeginAttack();
			//	}
			//}

			//if (attack.InProcess)
			//{
			//	attack.Tick(Target, deltaTime);
			//}
			//else
			//{
			//	NavMeshAgent.destination = Target.NavMeshAgent.nextPosition;
			//}

			return false;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);
			navAgent.IsActive = false;
		}
	}
}
