using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using ExecutionContext = Omniverse.Actions.ExecutionContext;

namespace Omniverse.Abilities
{
	public class Ability
	{
		public AbilityDesc Desc { get; }

		public ITarget Target { get; }

		public Cooldown Cooldown { get; }

		public bool AwaitsTarget { get; set; }

		public bool InProcess { get; set; }

		public AutoResetUniTaskCompletionSource Used { get; }

		private ExecutionContext ExecutionContext { get; }

		public Ability(IObjectResolver objectResolver, AbilityDesc desc)
		{
			Desc = desc;
			Target = desc.Target.Build();
			Cooldown = new Cooldown(Desc.Cooldown);
			Used = AutoResetUniTaskCompletionSource.Create();
			ExecutionContext = new ExecutionContext(objectResolver, desc.Actions);
		}

		public async UniTask Cast(Entity caster, CancellationToken token)
		{
			Cooldown?.ActivateAsync(token);

			switch (Target)
			{
				case null:
					break;
				case EntityTarget entityTarget:
					ExecutionContext.Entities.Add(entityTarget.Value);
					break;
				case PointTarget pointTarget:
					ExecutionContext.Points.Add(pointTarget.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Target));
			}

			await ExecutionContext.PerformAsync(caster, token);

			Used.TrySetResult();
		}
	}
}
