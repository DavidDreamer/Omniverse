using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Actions
{
	public class AddTargetSelfDesc : IActionDesc
	{
		public UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			context.Entities.Add(context.Caster);
			return UniTask.CompletedTask;
		}
	}
}
