using Dreambox.Physics;
using UnityEngine;

namespace Omniverse.Units.Rendering
{
	public class UnitRenderer: UnitRendererBase, IPoolObject
	{
		private static class AnimatorVariables
		{
			public static int IsMoving { get; } = Animator.StringToHash(nameof(IsMoving));
			public static int MovementSpeed { get; } = Animator.StringToHash(nameof(MovementSpeed));
			public static int Attack { get; } = Animator.StringToHash(nameof(Attack));
		}
		
		[field: SerializeField]
		public Animator Animator { get; set; }

		[field: SerializeField]
		private HealthBar HealthBar { get; set; }
		
		[field: SerializeField]
		private Ragdoll Ragdoll { get; set; }

		[field: SerializeField]
		public UnitMarker Selection { get; set; }

		[field: SerializeField]
		public UnitMarker Focus { get; set; }
		
		[field: SerializeField]
		[field: HideInInspector]
		public Renderer[] Renderers { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public MeshFilter[] MeshFilters { get; private set; }
		
		[field: SerializeField]
		public AudioSource AudioSource { get; private set; }
		
		public Unit Unit { get; private set; }

		private void OnValidate()
		{
			Renderers = GetComponentsInChildren<Renderer>(true);
			MeshFilters = GetComponentsInChildren<MeshFilter>(true);
		}
		
		public override void Initialize(Unit unit)
		{
			Unit = unit;
			Unit.Died += OnDied;
			Unit.Attack.Started += OnAttackStarted;

			HealthBar.Initialize(Unit);
			Selection.Initialize(Unit);
			Focus.Initialize(Unit);
		}

		private void OnDestroy()
		{
			if (Unit == null)
			{
				return;
			}
			
			Unit.Died -= OnDied;
			Unit.Attack.Started -= OnAttackStarted;
		}

		private void Update()
		{
			Animator.SetBool(AnimatorVariables.IsMoving, Unit.Presenter.NavMeshAgent.velocity.sqrMagnitude > 0);
			//TODO
			Animator.SetFloat(AnimatorVariables.MovementSpeed, 1);
		}

		private void OnDied()
		{
			ProcessLivingState(false);
		}

		private void ProcessLivingState(bool alive)
		{
			Animator.enabled = alive;
			
			if (Ragdoll != null)
			{
				Ragdoll.Enable(!alive);
			}

			HealthBar.gameObject.SetActive(alive);
		}
		
		private void OnAttackStarted()
		{
			Animator.SetTrigger(AnimatorVariables.Attack);
		}

		public override void Cleanup()
		{
		}
	}
}
