using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class CollectUnitTargetsFromSector : CollectUnitTargets
	{
		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		[field: Range(0, 360)]
		public float Angle { get; private set; }

		public override IEnumerable<Unit> GetUnits(ExecutionContext context)
		{
			yield return null;

			// Transform transform = context.Caster.Presenter;
			//
			// return PhysicsHelper.GetUnitsInSector(transform.position,
			// 	transform.forward,
			// 	Desc.Radius,
			// 	Desc.Angle,
			// 	PhysicsSettings.HitboxLayerMask);
		}
	}
}
