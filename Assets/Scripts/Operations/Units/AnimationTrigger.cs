using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class AnimationTrigger : Action
	{
		[field: SerializeField]
		public string Name { get; private set; }

		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			//TODO
			// foreach (Unit unit in context.Units)
			// {
			// 	if (unit.Presenter.Animator != null)
			// 	{
			// 		unit.Presenter.Animator.SetTrigger(AnimatorParameter.Get(Desc.Name));
			// 	}
			// }

			return UniTask.CompletedTask;
		}
	}
}
