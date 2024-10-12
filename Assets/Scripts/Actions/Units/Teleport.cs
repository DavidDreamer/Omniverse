using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Teleport : Action
	{
		public override void PerformTemp(OperationContext context)
		{
			Vector3 position = context.Points.First();

			foreach (Unit unit in context.Units())
			{
				unit.NavMeshAgent.Warp(position);
			}
		}
	}
}
