using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Units;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public abstract class CollectUnitTargets<T> : Action<T> where T : CollectUnitTargetsDesc
	{
		protected CollectUnitTargets(T desc) : base(desc)
		{
		}

		public abstract IEnumerable<Unit> GetUnits(ExecutionContext context);

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			//TODO
			var source = context.Caster as IFactious;

			foreach (Unit unit in GetUnits(context))
			{
				if (source != null)
				{
					if (!Desc.Filter.Match(source, unit))
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
