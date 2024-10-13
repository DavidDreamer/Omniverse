using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class Teleport : ScriptableObject, IAction<Unit, Vector3>
	{
		public void Perform(Unit unit, Vector3 target)
		{
			unit.NavMeshAgent.Warp(target);
		}
	}
}
