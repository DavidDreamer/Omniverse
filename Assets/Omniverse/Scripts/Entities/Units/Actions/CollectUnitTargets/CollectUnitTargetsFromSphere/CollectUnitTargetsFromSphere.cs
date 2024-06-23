using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Omniverse.Entities.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class CollectUnitTargetsFromSphere: CollectUnitTargets<CollectUnitTargetsFromSphereDesc>
	{
		[Inject]
		private PhysicsSettings PhysicsSettings { get; set; }
		
		public CollectUnitTargetsFromSphere(CollectUnitTargetsFromSphereDesc desc): base(desc)
		{
		}

		public override IEnumerable<Unit> GetUnits(ExecutionContext context)
		{
			Vector3 position = context.Points.First();
			return PhysicsHelper.GetUnitsInSphere(position, Desc.Radius, PhysicsSettings.HitboxLayerMask);
		}
	}
}
