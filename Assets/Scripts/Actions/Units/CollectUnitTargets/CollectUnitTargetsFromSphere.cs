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

		public override IEnumerable<Unit> GetUnits(ActionContext context)
		{
			//TODO
			IFactious caster = context.Actor as IFactious;
			Vector3 position = context.Vectors.First();
			return context.PhysicsService.GetEntitiesInSphere<Unit>(position, Radius).Where(unit => Filter.Match(caster, unit));
		}
	}
}
