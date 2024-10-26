using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class CollectInRadiusTargetProvider : ITargetProvider<Unit>, ITargetConverter<None, Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Convert(Entity actor, None input) => actor.PhysicsService.GetEntitiesInSphere<Unit>(actor, Radius, Filter);

		public IEnumerable<Unit> Get(Entity actor) => actor.PhysicsService.GetEntitiesInSphere<Unit>(actor, Radius, Filter);
	}
}
