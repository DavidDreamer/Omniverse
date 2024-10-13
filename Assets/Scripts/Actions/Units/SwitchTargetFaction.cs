using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFaction : ScriptableObject, IAction<Unit, Unit>
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

		public void Perform(Unit actor, Unit target)
		{
			var action = (IAction<Unit, Unit>)Switch();
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
