using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using ExecutionContext = Omniverse.Actions.ExecutionContext;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public Cooldown Cooldown { get; }

		public bool InProcess { get; set; }

		public AutoResetUniTaskCompletionSource Used { get; }

		public ExecutionContext ExecutionContext { get; } = new();

		public Ability(IObjectResolver objectResolver, AbilityDesc desc)
		{
			Desc = desc;
			Cooldown = new Cooldown(Desc.Cooldown);
			Used = AutoResetUniTaskCompletionSource.Create();
		}

		public async UniTask Cast(Entity caster, CancellationToken token)
		{
			Cooldown?.ActivateAsync(token);

			await ExecutionContext.PerformAsync(Desc.Action, caster, token);

			Used.TrySetResult();
		}
	}
}
