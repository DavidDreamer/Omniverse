using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[CreateAssetMenu]
	public class ApplyEffect : Action
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			foreach (var unit in context.Units())
			{
				var effect = new Effect(Effect);

				unit.ApplyEffect(effect);
			}

			return UniTask.CompletedTask;
		}
	}
}
