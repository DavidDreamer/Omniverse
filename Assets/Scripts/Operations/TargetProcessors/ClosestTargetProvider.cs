using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class ClosestTargetProvider : ITargetConverter<None, UnitObsolete>, ITargetConverter<UnitObsolete, UnitObsolete>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<UnitObsolete> Convert(OmniverseEntity actor, None input)
		{
			yield return actor.PhysicsService.GetClosestEntity<UnitObsolete>(actor, Radius, Filter);
		}

		public IEnumerable<UnitObsolete> Convert(OmniverseEntity actor, UnitObsolete input)
		{
			yield return actor.PhysicsService.GetClosestEntity<UnitObsolete>(input, Radius, Filter);
		}
	}
}
