using System;
using UnityEngine;

namespace Omniverse.Actions
{
	[Serializable]
	public abstract class CollectUnitTargetsDesc : IActionDesc
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public abstract IAction Build();
	}
}
