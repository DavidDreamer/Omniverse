using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class CollectInRadiusTargetProvider : ITargetConverter<None, UnitObsolete>, ITargetConverter<UnitObsolete, UnitObsolete>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<UnitObsolete> Convert(OmniverseEntity actor, None input) => PhysicsService.GetEntitiesInSphere<UnitObsolete>(actor, Radius, Filter);

		public IEnumerable<UnitObsolete> Convert(OmniverseEntity actor, UnitObsolete input) => PhysicsService.GetEntitiesInSphere<UnitObsolete>(input, Radius, Filter);
	}
}
