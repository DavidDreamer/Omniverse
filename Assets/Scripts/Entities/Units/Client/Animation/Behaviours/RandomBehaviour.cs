using UnityEngine;
using UnityEngine.Animations;

namespace Omniverse.Entities.Units.Client
{
	public class RandomBehaviour: StateMachineBehaviour
	{
		private static int Random { get; } = Animator.StringToHash("Random");

		public override void OnStateEnter(
			Animator animator,
			AnimatorStateInfo stateInfo,
			int layerIndex,
			AnimatorControllerPlayable controller)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex, controller);

			animator.SetFloat(Random, UnityEngine.Random.value);
		}
	}
}
