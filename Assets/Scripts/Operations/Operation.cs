using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse
{
	public abstract class Operation : ScriptableObject
	{
		public abstract UniTask<Operation> PerformAsync(ExecutionContext context, CancellationToken token);
	}
}
