using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class CollectUnitTargets : Action
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public abstract IEnumerable<Unit> GetUnits(Unit actor);

		public override void Perform(Unit actor)
		{
			foreach (Unit unit in GetUnits(actor))
			{
				if (actor != null)
				{
					if (!Filter.Match(actor, unit))
					{
						continue;
					}
				}

				//TODO
				//context.Entities.Add(unit);
			}
		}
	}
}
