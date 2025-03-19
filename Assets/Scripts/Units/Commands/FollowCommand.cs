using Unity.Entities;

namespace Omniverse
{
	public class FollowCommand : Command
	{
		private Entity Target { get; }

		public FollowCommand(Entity entity, Entity target) : base(entity)
		{
			Target = target;
		}

		public override bool Tick(ref SystemState state)
		{
			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity);
			navAgent.targetEntity = Target;
			return false;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity);
			navAgent.IsActive = false;
		}
	}
}
