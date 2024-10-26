using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetConverter<None, Unit>, ITargetConverter<None, Vector3>
	{
		IEnumerable<Unit> ITargetConverter<None, Unit>.Convert(Entity actor, None input)
		{
			yield return (Unit)actor;
		}

		IEnumerable<Vector3> ITargetConverter<None, Vector3>.Convert(Entity actor, None input)
		{
			yield return actor.transform.position;
		}
	}
}
