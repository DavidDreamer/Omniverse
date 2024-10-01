using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ChangeProperty : Action
	{
		[field: SerializeField]
		public PropertyID PropertyID { get; private set; }

		[field: SerializeField]
		public int Amount { get; private set; }

		public override UniTask Perform(ExecutionContext context, CancellationToken token)
		{
			foreach (Unit unit in context.Units())
			{
				var data = new ChangePropertyData
				{
					ID = PropertyID,
					Source = context.Caster,
					Amount = Amount
				};

				unit.ChangeResource(data);
			}

			return UniTask.CompletedTask;
		}
	}
}
