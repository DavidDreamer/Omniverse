using Dreambox.Physics;
using UnityEngine;

namespace Omniverse
{
	public class UnitRenderer: MonoBehaviour
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

		private Unit Unit { get; set; }

		public void Initialize(Unit unit)
		{
			Unit = unit;
			Unit.Died += OnDied;
			Unit.Attack.Started += OnAttackStarted;

			HealthBar.Initialize(Unit);
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
			Animator.enabled = false;
			
			if (Ragdoll != null)
			{
				Ragdoll.Enable(true);
			}
		}
		
		private void OnAttackStarted()
		{
			Animator.SetTrigger(AnimatorVariables.Attack);
		}
	}
}
