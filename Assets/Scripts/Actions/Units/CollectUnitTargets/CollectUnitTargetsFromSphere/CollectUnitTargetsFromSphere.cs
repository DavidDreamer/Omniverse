using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Omniverse.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class CollectUnitTargetsFromSphere : CollectUnitTargets<CollectUnitTargetsFromSphereDesc>
	{
		[Inject]
		private PhysicsSettings PhysicsSettings { get; set; }

		public CollectUnitTargetsFromSphere(CollectUnitTargetsFromSphereDesc desc) : base(desc)
		{
		}

		public override IEnumerable<Unit> GetUnits(ExecutionContext context)
		{
			//TODO
			IFactious caster = context.Caster as IFactious;
			Vector3 position = context.Points.First();
			return PhysicsHelper.GetUnitsInSphere(position, Desc.Radius, PhysicsSettings.HitboxLayerMask).Where(unit => Desc.Filter.Match(caster, unit));
		}
	}
}
