using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class ClosestTargetProvider : ITargetConverter<None, Unit>, ITargetConverter<Unit, Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Convert(OmniverseEntity actor, None input)
		{
			yield return actor.PhysicsService.GetClosestEntity<Unit>(actor, Radius, Filter);
		}

		public IEnumerable<Unit> Convert(OmniverseEntity actor, Unit input)
		{
			yield return actor.PhysicsService.GetClosestEntity<Unit>(input, Radius, Filter);
		}
	}
}
