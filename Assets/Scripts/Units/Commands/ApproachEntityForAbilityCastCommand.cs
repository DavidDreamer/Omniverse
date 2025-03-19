using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class ApproachEntityForAbilityCastCommand : Command
	{
		public Ability Ability { get; }

		public Entity Target { get; }

		public ApproachEntityForAbilityCastCommand(Entity entity, Ability ability, Entity target) : base(entity)
		{
			Ability = ability;
			Target = target;
		}

		public override bool Tick(ref SystemState state)
		{
			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity);
			var transform = state.EntityManager.GetComponentData<LocalTransform>(Entity);
			var targetTransform = state.EntityManager.GetComponentData<LocalTransform>(Target);

			navAgent.targetEntity = Target;

			return Vector3.Distance(targetTransform.Position, transform.Position) <= Ability.CastRange;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity);

			navAgent.targetEntity = Entity.Null;
		}
	}
}
