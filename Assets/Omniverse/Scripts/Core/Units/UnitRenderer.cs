using UnityEngine;

namespace Omniverse
{
	public class UnitRenderer: MonoBehaviour
	{
		private static class AnimatorVariables
		{
			public static int IsMoving { get; } = Animator.StringToHash(nameof(IsMoving));
			public static int MovementSpeed { get; } = Animator.StringToHash(nameof(MovementSpeed));
		}
		
		[field: SerializeField]
		public Animator Animator { get; set; }

		[field: SerializeField]
		public UnitPresenter UnitPresenter { get; set; }
		
		private void Update()
		{
			Animator.SetBool(AnimatorVariables.IsMoving, UnitPresenter.NavMeshAgent.velocity.sqrMagnitude > 0);
			//TODO
			Animator.SetFloat(AnimatorVariables.MovementSpeed, 1);
		}
	}
}
