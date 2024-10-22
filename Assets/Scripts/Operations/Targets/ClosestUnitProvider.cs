using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	public class ClosestUnitProvider : ITargetProvider<Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public Unit Get(Unit actor)
		{
			return actor.PhysicsService.GetClosestEntity<Unit>(actor, Radius, Filter);
		}
	}
}
