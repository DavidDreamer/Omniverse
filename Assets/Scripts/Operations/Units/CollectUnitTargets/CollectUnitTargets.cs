using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public abstract class CollectUnitTargets : Action
	{
		[field: SerializeField]
		public FactiousFilter Filter { get; private set; }

		public abstract IEnumerable<Unit> GetUnits(OperationContext context);

		public override UniTask Perform(OperationContext context, CancellationToken token)
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

			return UniTask.CompletedTask;
		}
	}
}
