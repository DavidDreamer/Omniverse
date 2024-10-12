using System.Collections.Generic;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class CollectUnitTargets : Action
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public abstract IEnumerable<Unit> GetUnits(ActionContext context);

		public override void Perform(ActionContext context)
		{
			//TODO
			var source = context.Actor as IFactious;

			foreach (Unit unit in GetUnits(context))
			{
				if (source != null)
				{
					if (!Filter.Match(source, unit))
					{
						continue;
					}
				}

				context.Entities.Add(unit);
			}
		}
	}
}
