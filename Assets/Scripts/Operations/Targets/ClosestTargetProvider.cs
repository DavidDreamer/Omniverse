using UnityEngine;
using System.Collections.Generic;

namespace Omniverse
{
	public class ClosestTargetProvider : ITargetProvider<Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Get(Entity actor)
		{
			yield return actor.PhysicsService.GetClosestEntity<Unit>(actor, Radius, Filter);
		}
	}
}
