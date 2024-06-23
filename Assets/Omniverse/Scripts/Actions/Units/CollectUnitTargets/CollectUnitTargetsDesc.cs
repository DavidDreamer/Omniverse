using System;
using Omniverse.Entities;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public abstract class CollectUnitTargetsDesc: IActionDesc
	{
		[field: SerializeField]
		public EntityTargetType EntityTargetType { get; private set; }

		public abstract IAction Build();
	}
}
