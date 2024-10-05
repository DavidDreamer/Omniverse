using System.Threading;
using Cysharp.Threading.Tasks;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ModifyProperty : Action
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public override UniTask Perform(OperationContext context, CancellationToken token)
		{
			foreach (Unit unit in context.Units())
			{
				unit.ModifyProperty(Property, Modifier, context.Actor);
			}

			return UniTask.CompletedTask;
		}
	}
}
