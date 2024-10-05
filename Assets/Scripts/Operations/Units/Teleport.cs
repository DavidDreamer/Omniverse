using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Teleport : Action
	{
		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			Vector3 position = context.Points.First();

			foreach (Unit unit in context.Units())
			{
				unit.NavMeshAgent.Warp(position);
			}

			return UniTask.CompletedTask;
		}
	}
}
