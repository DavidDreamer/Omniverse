using System.Collections.Generic;
using UnityEngine;

namespace Omniverse
{
	public class ClosestTargetProvider : ITargetConverter<None, Unit>
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public IEnumerable<Unit> Convert(Entity actor, None input)
		{
			yield return actor.PhysicsService.GetClosestEntity<Unit>(actor, Radius, Filter);
		}
	}
}
