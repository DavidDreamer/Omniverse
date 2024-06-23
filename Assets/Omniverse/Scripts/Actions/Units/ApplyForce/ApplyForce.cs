using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Entities.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class ApplyForce: Action<ApplyForceDesc>
	{
		public ApplyForce(ApplyForceDesc desc): base(desc)
		{
		}

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			foreach (Unit unit in context.Units())
			{
				//TODO
				//unit.AddForce(force);
			}

			return UniTask.CompletedTask;
		}
	}
}
