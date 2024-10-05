using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class Operation : ScriptableObject
	{
		public abstract UniTask<Operation> PerformAsync(ExecutionContext context, CancellationToken token);
	}
}
