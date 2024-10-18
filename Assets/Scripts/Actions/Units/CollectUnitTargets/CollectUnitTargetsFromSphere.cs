using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class CollectUnitTargetsFromSphere : CollectUnitTargets
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		public override IEnumerable<Unit> GetUnits(Unit actor)
		{
			//TODO
			Vector3 position = Vector3.zero;// context.Vectors.First();
			return actor.PhysicsService.GetEntitiesInSphere<Unit>(position, Radius).Where(unit => Filter.Match(actor, unit));
		}
	}
}
