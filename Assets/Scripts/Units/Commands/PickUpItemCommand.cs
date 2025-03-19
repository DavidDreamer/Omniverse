using Unity.Entities;

namespace Omniverse
{
	public class PickUpItemCommand : Command
	{
		private Entity Item { get; }

		public PickUpItemCommand(Entity entity, Entity item) : base(entity)
		{
			Item = item;
		}

		public override void Start(ref SystemState state)
		{
			base.Start(ref state);

			var navAgent = state.EntityManager.GetComponentData<NavAgentComponent>(Entity);
			navAgent.targetEntity = Item;
			navAgent.IsActive = true;
		}

		public override bool Tick(ref SystemState state)
		{
			//if (Vector3.Distance(Item.transform.position, NavMeshAgent.nextPosition) <= 0.1f)
			//{
			//	TODO ECS

			//	Item.gameObject.SetActive(false);

			//	Unit.Inventory.Add(Item);
			//	return true;
			//}

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
