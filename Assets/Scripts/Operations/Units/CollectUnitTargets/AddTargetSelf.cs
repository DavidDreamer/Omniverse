using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class AddTargetSelf : Action
	{
		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			context.Entities.Add(context.Caster);
			return UniTask.CompletedTask;
		}
	}
}
