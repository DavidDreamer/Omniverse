using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ApplyEffectDesc : IActionDesc
	{
		[field: SerializeField]
		public EffectDesc Effect { get; private set; }

		public UniTask Perform(ExecutionContext context, CancellationToken token)
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
