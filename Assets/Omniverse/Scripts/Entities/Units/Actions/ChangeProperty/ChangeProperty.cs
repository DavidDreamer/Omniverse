using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Omniverse.Entities.Units;

namespace Omniverse.Actions
{
	[UsedImplicitly]
	public class ChangeProperty: Action<ChangePropertyDesc>
	{
		public ChangeProperty(ChangePropertyDesc desc): base(desc)
		{
		}

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			foreach (Unit unit in context.Units)
			{
				var data = new ChangePropertyData
				{
					ID = Desc.PropertyID,
					Source = context.Caster,
					Amount = Desc.Amount
				};

				unit.ChangeResource(data);
			}

			return UniTask.CompletedTask;
		}
	}
}
