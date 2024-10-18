using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Teleport : Action<Unit, Vector3>
	{
		public override void Perform(Unit actor, Vector3 target)
		{
			actor.NavMeshAgent.Warp(target);
		}
	}
}
