using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class CollectInRadiusTargetProvider : ITargetConverter<None, Unit>, ITargetConverter<Unit, Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Convert(OmniverseEntity actor, None input) => actor.PhysicsService.GetEntitiesInSphere<Unit>(actor, Radius, Filter);

		public IEnumerable<Unit> Convert(OmniverseEntity actor, Unit input) => actor.PhysicsService.GetEntitiesInSphere<Unit>(input, Radius, Filter);
	}
}
