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
		private PropertyTag MovementSpeedProperty { get; set; }
		
		[field: SerializeField]
		private PropertyTag RotationSpeedProperty { get; set; }
		
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; set; }

		[field: SerializeField]
		public Collider Hitbox { get; private set; }

		[field: SerializeField]
		private Ragdoll Ragdoll { get; set; }

		[field: SerializeField]
		public AudioSource AudioSource { get; private set; }
		
		[field: SerializeField]
		[field: HideInInspector]
		public Renderer[] Renderers { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public MeshFilter[] MeshFilters { get; private set; }
		
		[field: SerializeField]
		[field: HideInInspector]
		private IDeathHandler[] DeathHandlers { get; set; }

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
			DeathHandlers = GetComponentsInChildren<IDeathHandler>();
		}

		public void Bind(Unit unit)
		{
			Unit = unit;

			if (NavMeshAgent != null)
			{
				if (unit.Properties.TryGetValue(MovementSpeedProperty, out Property property))
				{
					NavMeshAgent.speed = property.Amount.Value;
				}
				
				if (unit.Properties.TryGetValue(RotationSpeedProperty, out Property rotationSpeedProperty))
				{
					NavMeshAgent.angularSpeed = rotationSpeedProperty.Amount.Value;
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

			if (Animator != null)
			{
				Animator.enabled = true;
			}

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = true;
			}

			if (Ragdoll != null)
			{
				Ragdoll.Enable(false);
			}
		}

		public void Tick()
		{
			if (NavMeshAgent != null)
			{
				NavMeshAgent.isStopped = Unit.Status.HasFlag(UnitStatus.Stunned);
			}
		}
		
		public void AddForce(Vector3 force)
		{
			if (Ragdoll != null)
			{
				Ragdoll.AddForce(force);
			}
		}

		public void Despawn()
		{
			UniTaskCompletionSource.TrySetResult();
		}

		public virtual void OnDeath()
		{
			Hitbox.enabled = false;

			if (Animator != null)
			{
				Animator.enabled = false;
			}

			if (NavMeshAgent != null)
			{
				NavMeshAgent.enabled = false;
			}

			if (Ragdoll != null)
			{
				Ragdoll.Enable(true);
			}

			if (DeathHandlers != null)
			{
				foreach (IDeathHandler deathHandler in DeathHandlers)
				{
					deathHandler.OnDead(this);
				}
			}

			UniTaskCompletionSource.TrySetResult();
		}

		public void UpdateMovementFromNavMeshAgent()
		{
			Animator.SetFloat(AnimatorParameter.Get("MovementSpeed"), NavMeshAgent.velocity.magnitude);
		}
		
		public void MoveDirection(Vector3 direction)
		{
			Animator.SetFloat(AnimatorParameter.Get("MovementSpeed"), direction != Vector3.zero ? 1f : 0f);

			if (direction != Vector3.zero)
			{
				float dotProduct = Vector3.Dot(transform.forward, direction);

				if (dotProduct <= 0)
				{
					transform.forward = direction;
				}
				else
				{
					float delta = NavMeshAgent.angularSpeed * Time.deltaTime;
					transform.forward = Vector3.RotateTowards(transform.forward, direction, delta, 1f);
				}
			}

			Vector3 velocity = direction * NavMeshAgent.speed * Time.deltaTime;
			Vector3 nextPosition = transform.position + velocity;

			if (NavMesh.SamplePosition(nextPosition, out NavMeshHit hit, NavMeshAgent.height * 2, 1))
			{
				transform.position = hit.position;
			}
		}
	}
}
