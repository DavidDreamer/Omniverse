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

		public override void PerformTemp(OperationContext context)
		{
			foreach (Unit unit in context.Units())
			{
				unit.ModifyProperty(Property, Modifier, context.Actor);
			}
		}
	}
}
