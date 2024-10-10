using Omniverse.Items;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Omniverse.Units
{
	public class PickUpItemCommand : Command
	{
		private Item Item { get; }

		private NavMeshAgent NavMeshAgent => Unit.NavMeshAgent;

		public PickUpItemCommand(Unit unit, Item item) : base(unit)
		{
			Item = item;
		}

		public override void Start()
		{
			base.Start();

			NavMeshAgent.isStopped = false;
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

			NavMeshAgent.isStopped = true;
			NavMeshAgent.ResetPath();
		}
	}
}
