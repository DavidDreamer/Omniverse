using System;
using Omniverse.Entities.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public abstract class CollectUnitTargetsDesc: IActionDesc
	{
		[field: SerializeField]
		public UnitTargetTypeFlags UnitTargetTypeFlags { get; private set; }
	}
}
