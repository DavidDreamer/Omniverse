using Cysharp.Threading.Tasks;
using Dreambox.Core;
using Dreambox.Physics;
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
		public AudioSource AudioSource { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public Renderer[] Renderers { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public MeshFilter[] MeshFilters { get; private set; }

		[field: SerializeField]
		public float DespawnDelay { get; private set; }

		[field: SerializeField]
		public GameObject Selection { get; set; }

		[field: SerializeField]
		public GameObject Focus { get; set; }

		public Unit Unit { get; private set; }

		public UniTaskCompletionSource UniTaskCompletionSource { get; set; } = new();

		private void OnValidate()
		{
			Renderers = GetComponentsInChildren<Renderer>(true);
			MeshFilters = GetComponentsInChildren<MeshFilter>(true);
		}

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
