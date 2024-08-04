using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class AnimationTrigger : Action<AnimationTriggerDesc>
	{
		public AnimationTrigger(AnimationTriggerDesc desc) : base(desc)
		{
		}

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
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
