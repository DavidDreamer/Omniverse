using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Omniverse
{
	public class MoveCommand : Command
	{
		public Vector3 Position { get; }

		public MoveCommand(DynamicEntity entity, Vector3 position) : base(entity)
		{
			Position = position;
		}

		public override void Start(ref SystemState state)
		{
			base.Start(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);

			navAgent.IsActive = true;
			navAgent.targetPosition = Position;

			state.EntityManager.SetComponentData(Entity.Entity, navAgent);
		}

		public override bool Tick(ref SystemState state)
		{
			var transform = state.EntityManager.GetComponentData<LocalTransform>(Entity.Entity);
			return Vector3.Distance(Position, transform.Position) <= 0.1f;
		}

		public override void Cleanup(ref SystemState state)
		{
			base.Cleanup(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity.Entity);
			navAgent.IsActive = false;

			state.EntityManager.SetComponentData(Entity.Entity, navAgent);
		}
	}
}
