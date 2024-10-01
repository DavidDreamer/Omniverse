using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class AnimationTriggerDesc : IActionDesc
	{
		[field: SerializeField]
		public string Name { get; private set; }

		public UniTask Perform(ExecutionContext context, CancellationToken token)
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
