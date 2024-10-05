using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Omniverse
{
	public class OperationHandler
	{
		[Inject]
		public IObjectResolver ObjectResolver { get; set; }

		public async UniTask PerformAsync(Operation operation, Entity actor, CancellationToken token)
		{
			var context = new OperationContext(actor);
			ObjectResolver.Inject(context);

			do
			{
				operation = await operation.PerformAsync(context, token);
			}
			while (operation != null);
		}

		public async UniTask PerformAsync(Operation operation, Entity actor, OperationContext context, CancellationToken token)
		{
			ObjectResolver.Inject(context);

			do
			{
				operation = await operation.PerformAsync(context, token);
			}
			while (operation != null);
		}
	}
}
