using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse.Units
{
	public class UnitPresenter: EntityPresenter<Unit, UnitDesc>, IPoolObject
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Collider Hitbox { get; private set; }

		public Unit Unit { get; private set; }

		public UniTaskCompletionSource UniTaskCompletionSource { get; set; } = new();

		public void Bind(Unit unit)
		{
			Unit = unit;
		}

		public void Cleanup()
		{
			UniTaskCompletionSource = new UniTaskCompletionSource();

			Unit = null;

			if (Hitbox != null)
			{
				Hitbox.enabled = true;
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
				if (Unit.Properties.TryGetValue(PropertyID.MovementSpeed, out Property property))
				{
					NavMeshAgent.speed = property.Amount;
				}
				else
				{
					NavMeshAgent.speed = 0;
				}
				
				if (Unit.Properties.TryGetValue(PropertyID.RotationSpeed, out property))
				{
					NavMeshAgent.angularSpeed = property.Amount;
				}
				else
				{
					NavMeshAgent.angularSpeed = 0;
				}
				
				NavMeshAgent.isStopped = Unit.Status.HasFlag(UnitStatus.Stunned);
			}
		}

		public void Despawn()
		{
			UniTaskCompletionSource.TrySetResult();
		}

		public virtual void OnDeath()
		{
			Hitbox.enabled = false;

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = false;
			}

			UniTaskCompletionSource.TrySetResult();
		}
	}
}
