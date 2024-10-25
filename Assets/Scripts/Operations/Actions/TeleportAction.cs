using UnityEngine;

namespace Omniverse
{
	public class TeleportAction : IAction<Vector3>
	{
		public void Perform(Entity actor, Vector3 target)
		{
			actor.NavMeshAgent.Warp(target);
		}
	}
}
