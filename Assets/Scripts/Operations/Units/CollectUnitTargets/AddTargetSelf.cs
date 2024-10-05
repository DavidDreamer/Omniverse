using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Actions
{
	public class AddTargetSelf : Action
	{
		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			context.Entities.Add(context.Actor);
			return UniTask.CompletedTask;
		}
	}
}
