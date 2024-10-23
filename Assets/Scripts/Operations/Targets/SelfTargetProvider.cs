using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetProvider<Unit>, ITargetProvider<Vector3>
	{
		IEnumerable<Unit> ITargetProvider<Unit>.Get(Entity actor)
		{
			yield return (Unit)actor;
		}

		IEnumerable<Vector3> ITargetProvider<Vector3>.Get(Entity actor)
		{
			yield return actor.transform.position;
		}
	}
}
