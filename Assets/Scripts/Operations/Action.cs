using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse
{
	public abstract class Action : Operation
	{
		[field: SerializeField]
		[field: OperationPicker]
		public Operation Then { get; private set; }

		public override async UniTask<Operation> PerformAsync(ExecutionContext context, CancellationToken token)
		{
			await Perform(context, token);
			return Then;
		}

		public abstract UniTask Perform(ExecutionContext context, CancellationToken token);
	}
}
