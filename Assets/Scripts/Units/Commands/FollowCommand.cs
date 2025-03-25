using Unity.Entities;

namespace Omniverse
{
	public class FollowCommand : Command
	{
		private DynamicEntity Target { get; }

		public FollowCommand(DynamicEntity entity, DynamicEntity target) : base(entity)
		{
			Target = target;
		}

		public override bool Tick(ref SystemState state)
		{
			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Target.Entity);
			navAgent.targetEntity = Target.Entity;
			return false;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Target.Entity);
			navAgent.IsActive = false;
		}
	}
}
