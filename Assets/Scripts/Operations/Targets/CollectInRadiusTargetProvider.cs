using UnityEngine;
using System.Collections.Generic;

namespace Omniverse
{
	public class CollectInRadiusTargetProvider : ITargetProvider<Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Get(Entity actor) => actor.PhysicsService.GetEntitiesInSphere<Unit>(actor, Radius, Filter);
	}
}
