using Cysharp.Threading.Tasks;
using Omniverse.Camera;
using UnityEngine;
using UnityEngine.AI;

namespace Omniverse
{
	public class UnitPresenter: MonoBehaviour, IPoolObject
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Collider Hitbox { get; private set; }
		
		[field: SerializeField]
		public FogOfWarAgent FogOfWarAgent { get; private set; }
		
		public Unit Unit { get; private set; }

		public UniTaskCompletionSource UniTaskCompletionSource { get; set; } = new();

		public void Bind(Unit unit)
		{
			Unit = unit;

			if (NavMeshAgent != null)
			{
				if (Unit.Movement == null)
				{
					NavMeshAgent.speed = 0;
					NavMeshAgent.angularSpeed = 0;
				}
				else
				{
					NavMeshAgent.speed = Unit.Movement.Speed;
					NavMeshAgent.angularSpeed = Unit.Movement.RotationSpeed;
				}
			}

			if (FogOfWarAgent != null)
			{
				FogOfWarAgent.FactionID = unit.FactionID;
			}
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
