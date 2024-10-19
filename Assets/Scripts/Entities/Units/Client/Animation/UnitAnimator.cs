using Dreambox.Physics;
using Omniverse.Abilities;
using UnityEngine;

namespace Omniverse.Units.Client
{
	public class UnitAnimator : EntityComponent<Unit>
	{
		private static class AnimatorVariables
		{
			public static int IsMoving { get; } = Animator.StringToHash(nameof(IsMoving));

			public static int MovementSpeed { get; } = Animator.StringToHash(nameof(MovementSpeed));

			public static int Attack { get; } = Animator.StringToHash(nameof(Attack));

			public static int AttackSpeed { get; } = Animator.StringToHash(nameof(AttackSpeed));
		}

		[field: SerializeField]
		public Animator Animator { get; set; }

		[field: SerializeField]
		private Ragdoll Ragdoll { get; set; }

		public override void Initialize(Unit unit)
		{
			base.Initialize(unit);

			unit.Died += OnDied;
			unit.Attack.Started += OnAttackStarted;

			foreach (Ability ability in unit.Abilities)
			{
				ability.Casting.Started += OnAbiltiyCastingStarted;
			}
		}

		private void OnAbiltiyCastingStarted(Casting casting)
		{
			string trigger = casting.Desc.AnimationTrigger;

			if (string.IsNullOrEmpty(trigger))
			{
				return;
			}

			Animator.SetTrigger(trigger);
		}

		private void Update()
		{
			if (Entity.IsDead)
			{
				return;
			}

			Animator.SetBool(AnimatorVariables.IsMoving,
				!Entity.NavMeshAgent.isStopped && Entity.NavMeshAgent.velocity.sqrMagnitude > 0);

			float movementSpeed = Entity.Properties[PropertyID.MovementSpeed].Amount;
			Animator.SetFloat(AnimatorVariables.MovementSpeed, movementSpeed);

			float attackSpeed = Entity.Properties[PropertyID.AttackSpeed].Amount;
			Animator.SetFloat(AnimatorVariables.AttackSpeed, attackSpeed);
		}

		private void OnAttackStarted()
		{
			Animator.SetTrigger(AnimatorVariables.Attack);
		}

		private void OnDied()
		{
			Animator.enabled = false;

			if (Ragdoll != null)
			{
				Ragdoll.Enable(true);
			}
		}
	}
}
