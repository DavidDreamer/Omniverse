using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class ApproachPositionForAbilityCastCommand : Command
	{
		public Entity Ability { get; }

		public Vector3 Position { get; }

		public ApproachPositionForAbilityCastCommand(DynamicEntity entity, Entity ability, Vector3 position) : base(entity)
		{
			Ability = ability;
			Position = position;
		}

		public override void Start(ref SystemState state)
		{
			base.Start(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);

			navAgent.IsActive = true;
			navAgent.targetPosition = Position;
		}

		public override bool Tick(ref SystemState state)
		{
			var localTransform = state.EntityManager.GetComponentData<LocalTransform>(Entity.Entity);

			//TODO ECS
			return Vector3.Distance(Position, localTransform.Position) <= 10f;// Ability.CastRange;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);
			navAgent.IsActive = false;
		}
	}
}
