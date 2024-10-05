using System.Collections.Generic;
using System.Linq;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	public class CollectUnitTargetsFromSphere : CollectUnitTargets
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[Inject]
		private PhysicsSettings PhysicsSettings { get; set; }

		public override IEnumerable<Unit> GetUnits(ExecutionContext context)
		{
			//TODO
			IFactious caster = context.Caster as IFactious;
			Vector3 position = context.Points.First();
			return PhysicsHelper.GetUnitsInSphere(position, Radius, PhysicsSettings.HitboxLayerMask).Where(unit => Filter.Match(caster, unit));
		}
	}
}
