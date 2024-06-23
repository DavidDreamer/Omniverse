using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Entities;
using Omniverse.Entities.Units;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public abstract class CollectUnitTargets<T>: Action<T> where T : CollectUnitTargetsDesc
	{
		protected CollectUnitTargets(T desc): base(desc)
		{
		}

		public abstract IEnumerable<Unit> GetUnits(ExecutionContext context);

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			bool selfCastAllowed = Desc.EntityTargetType.HasFlag(EntityTargetType.Self);
			bool allyCastAllowed = Desc.EntityTargetType.HasFlag(EntityTargetType.Ally);
			bool enemyCastAllowed = Desc.EntityTargetType.HasFlag(EntityTargetType.Enemy);

			foreach (Unit unit in GetUnits(context))
			{
				if (context.Caster != null)
				{
					if (unit == context.Caster && !selfCastAllowed)
					{
						continue;
					}

					bool hasSameFaction = unit.FactionID == context.Caster.FactionID;
					switch (hasSameFaction)
					{
						case true when !allyCastAllowed:
						case false when !enemyCastAllowed:
							continue;
					}
				}
				
				context.Entities.Add(unit);
			}

			return UniTask.CompletedTask;
		}
	}
}
