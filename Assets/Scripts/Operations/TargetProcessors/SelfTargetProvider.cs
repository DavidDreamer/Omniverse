using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class SelfTargetProvider : ITargetConverter<None, UnitObsolete>, ITargetConverter<None, Vector3>
	{
		IEnumerable<UnitObsolete> ITargetConverter<None, UnitObsolete>.Convert(OmniverseEntity actor, None input)
		{
			yield return (UnitObsolete)actor;
		}

		IEnumerable<Vector3> ITargetConverter<None, Vector3>.Convert(OmniverseEntity actor, None input)
		{
			yield return actor.transform.position;
		}
	}
}
