using System.Threading;
using Cysharp.Threading.Tasks;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Cooldown Cooldown { get; }

		public bool InProcess { get; set; }

		public AutoResetUniTaskCompletionSource Used { get; }

		public OperationHandler OperationHandler { get; }

		public OperationContext OperationContext { get; }

		public Ability(AbilityDesc desc, Entity actor, OperationHandler operationHandler)
		{
			Desc = desc;
			Cooldown = new Cooldown(Desc.Cooldown);
			Used = AutoResetUniTaskCompletionSource.Create();

			OperationHandler = operationHandler;

			OperationContext = new OperationContext(actor);
		}

		public async UniTask Cast(Entity caster, CancellationToken token)
		{
			Cooldown?.ActivateAsync(token);

			await OperationHandler.PerformAsync(Desc.Operation, caster, OperationContext, token);

			Used.TrySetResult();

			OperationContext.Clear();
		}
	}
}
