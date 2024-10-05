using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFaction : Operation
	{
		[field: SerializeField]
		[field: OperationPicker]
		public Operation Self { get; private set; }

		[field: SerializeField]
		[field: OperationPicker]
		public Operation Ally { get; private set; }

		[field: SerializeField]
		[field: OperationPicker]
		public Operation Enemy { get; private set; }

		public override async UniTask<Operation> PerformAsync(OperationContext context, System.Threading.CancellationToken token)
		{
			await UniTask.CompletedTask;

			var unit = context.Units().First();
			var caster = (IFactious)context.Actor;

			if (context.Actor == unit)
			{
				return Self;
			}
			else if (caster.IsAllyFor(unit))
			{
				return Ally;
			}
			else
			{
				return Enemy;
			}
		}
	}
}
