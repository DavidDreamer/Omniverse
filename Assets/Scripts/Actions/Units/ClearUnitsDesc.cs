using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Actions
{
	public class ClearUnitsDesc : IActionDesc
	{
		public UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			context.Entities.Clear();
			return UniTask.CompletedTask;
		}
	}
}
