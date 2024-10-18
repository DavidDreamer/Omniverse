using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFaction : Action<Unit, Unit>
	{
		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject Self { get; private set; }

		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject Ally { get; private set; }

		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject Enemy { get; private set; }

		public override void Perform(Unit actor, Unit target)
		{
			var action = (Action<Unit, Unit>)Switch();
			action.Perform(actor, target);

			ScriptableObject Switch()
			{
				if (actor == target)
				{
					return Self;
				}
				else if (actor.IsAllyFor(target))
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
}
