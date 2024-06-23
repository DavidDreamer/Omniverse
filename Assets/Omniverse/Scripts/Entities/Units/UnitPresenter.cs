using UnityEngine;
using UnityEngine.AI;

namespace Omniverse.Entities.Units
{
	public class UnitPresenter: EntityPresenter<Unit, UnitDesc>, IPoolObject
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		public void Cleanup()
		{
			if (HitBox != null)
			{
				HitBox.enabled = true;
			}

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = true;
			}
		}

		public void FixedTick()
		{
			if (NavMeshAgent != null)
			{
				if (Entity.Properties.TryGetValue(PropertyID.MovementSpeed, out Property property))
				{
					NavMeshAgent.speed = property.Amount;
				}
				else
				{
					NavMeshAgent.speed = 0;
				}
				
				if (Entity.Properties.TryGetValue(PropertyID.RotationSpeed, out property))
				{
					NavMeshAgent.angularSpeed = property.Amount;
				}
				else
				{
					NavMeshAgent.angularSpeed = 0;
				}
				
				NavMeshAgent.isStopped = Entity.Status.HasFlag(UnitStatus.Stunned);
			}
		}

		public virtual void OnDeath()
		{
			HitBox.enabled = false;

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = false;
			}
		}
	}
}
