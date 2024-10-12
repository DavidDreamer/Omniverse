using System.Linq;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFaction : Action
	{
		[field: SerializeField]
		[field: ActionPicker]
		public Action Self { get; private set; }

		[field: SerializeField]
		[field: ActionPicker]
		public Action Ally { get; private set; }

		[field: SerializeField]
		[field: ActionPicker]
		public Action Enemy { get; private set; }

		public override void Perform(ActionContext context)
		{
			var unit = context.Units().First();
			var caster = (IFactious)context.Actor;

			if (context.Actor == unit)
			{
				Self.Perform(context);
			}
			else if (caster.IsAllyFor(unit))
			{
				Ally.Perform(context);
			}
			else
			{
				Enemy.Perform(context);
			}
		}
	}
}
