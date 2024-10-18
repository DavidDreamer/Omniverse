using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Teleport : Action<Unit, Vector3>
	{
		public override void Perform(Unit unit, Vector3 target)
		{
			unit.NavMeshAgent.Warp(target);
		}
	}
}
