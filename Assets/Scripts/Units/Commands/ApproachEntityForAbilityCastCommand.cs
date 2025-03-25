using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class ApproachEntityForAbilityCastCommand : Command
	{
		public Ability Ability { get; }

		public DynamicEntity Target { get; }

		public ApproachEntityForAbilityCastCommand(DynamicEntity entity, Ability ability, DynamicEntity target) : base(entity)
		{
			Ability = ability;
			Target = target;
		}

		public override bool Tick(ref SystemState state)
		{
			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);
			var transform = state.EntityManager.GetComponentData<LocalTransform>(Entity.Entity);
			var targetTransform = state.EntityManager.GetComponentData<LocalTransform>(Target.Entity);

			navAgent.targetEntity = Target.Entity;

			return Vector3.Distance(targetTransform.Position, transform.Position) <= Ability.CastRange;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);

			navAgent.targetEntity = default;
		}
	}
}
