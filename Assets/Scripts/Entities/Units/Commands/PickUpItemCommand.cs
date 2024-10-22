using Omniverse.Items;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse
{
	public class PickUpItemCommand : Command
	{
		private Item Item { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public PickUpItemCommand(Unit unit, Item item) : base(unit)
		{
			Item = item;
		}

		public override bool Tick(float deltaTime)
		{
			NavMeshAgent.destination = Item.transform.position;

			if (Vector3.Distance(Item.transform.position, NavMeshAgent.nextPosition) <= NavMeshAgent.stoppingDistance)
			{
				Item.gameObject.SetActive(false);
				Unit.Inventory.Add(Item);
				return true;
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
