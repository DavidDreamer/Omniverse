using System;
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

		private UnitPresenter UnitPresenter { get; set; }

		public void Initialize(UnitPresenter unitPresenter)
		{
			UnitPresenter = unitPresenter;
			UnitPresenter.Unit.Attack.Started += OnAttackStarted;
		}
		
		private void OnDestroy()
		{
			if (UnitPresenter == null)
			{
				return;
			}

			UnitPresenter.Unit.Attack.Started -= OnAttackStarted;
		}

		private void Update()
		{
			Animator.SetBool(AnimatorVariables.IsMoving, UnitPresenter.NavMeshAgent.velocity.sqrMagnitude > 0);
			//TODO
			Animator.SetFloat(AnimatorVariables.MovementSpeed, 1);
		}

		private void OnAttackStarted()
		{
			Animator.SetTrigger(AnimatorVariables.Attack);
		}
	}
}
