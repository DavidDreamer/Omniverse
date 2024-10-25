using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFactionAction : IAction<Unit>
	{
		[field: SerializeReference]
		public IAction<Unit> Self { get; private set; }

		[field: SerializeReference]
		public IAction<Unit> Ally { get; private set; }

		[field: SerializeReference]
		public IAction<Unit> Enemy { get; private set; }

		public void Perform(Entity actor, Unit target)
		{
			var action = Switch();
			action.Perform(actor, target);

			IAction<Unit> Switch()
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
