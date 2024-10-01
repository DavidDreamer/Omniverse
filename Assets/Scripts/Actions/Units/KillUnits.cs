using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Actions
{
	public class KillUnits : Action
	{
		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO: insta kill

			return UniTask.CompletedTask;
		}
	}
}
