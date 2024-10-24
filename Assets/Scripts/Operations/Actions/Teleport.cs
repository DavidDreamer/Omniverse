using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class Teleport : IAction<Vector3>
	{
		public void Perform(Entity actor, Vector3 target)
		{
			actor.NavMeshAgent.Warp(target);
		}
	}
}
