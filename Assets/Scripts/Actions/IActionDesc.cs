using Cysharp.Threading.Tasks;
using System.Threading;

namespace Omniverse.Actions
{
	public interface IActionDesc
	{
		UniTask Perform(ExecutionContext context, CancellationToken token);
	}
}
