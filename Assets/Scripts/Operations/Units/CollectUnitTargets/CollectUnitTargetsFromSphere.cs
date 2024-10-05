using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	public class CollectUnitTargetsFromSphere : CollectUnitTargets
	{
		//TODO
		[field: SerializeField]
		public PhysicsService PhysicsService { get; set; }

		[field: SerializeField]
		public float Radius { get; private set; }

		public override IEnumerable<Unit> GetUnits(ExecutionContext context)
		{
			//TODO
			IFactious caster = context.Caster as IFactious;
			Vector3 position = context.Points.First();
			return PhysicsService.GetUnitsInSphere(position, Radius).Where(unit => Filter.Match(caster, unit));
		}
	}
}
