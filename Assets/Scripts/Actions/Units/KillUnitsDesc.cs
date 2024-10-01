using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Actions
{
	public class KillUnitsDesc : IActionDesc
	{
		public UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO: insta kill

			return UniTask.CompletedTask;
		}
	}
}
