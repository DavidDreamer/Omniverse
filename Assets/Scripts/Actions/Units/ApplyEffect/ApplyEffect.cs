using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Units;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class ApplyEffect : Action<ApplyEffectDesc>
	{
		public ApplyEffect(ApplyEffectDesc desc) : base(desc)
		{
		}

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			foreach (var unit in context.Units())
			{
				var effect = new Effect(Desc.Effect);

				unit.ApplyEffect(effect);
			}

			return UniTask.CompletedTask;
		}
	}
}
