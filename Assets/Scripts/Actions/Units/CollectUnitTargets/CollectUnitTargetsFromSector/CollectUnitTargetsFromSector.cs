using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Entities.Units;
using UnityEngine;
using VContainer;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class CollectUnitTargetsFromSector: CollectUnitTargets<CollectUnitTargetsFromSectorDesc>
	{
		[Inject]
		private PhysicsSettings PhysicsSettings { get; set; }
		
		public CollectUnitTargetsFromSector(CollectUnitTargetsFromSectorDesc desc): base(desc)
		{
		}

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
