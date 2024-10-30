using Dreambox.Core;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFactionAction : IAction<Entity>
	{
		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<Entity>))]
		public IAction<Entity> Self { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<Entity>))]
		public IAction<Entity> Ally { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<Entity>))]
		public IAction<Entity> Enemy { get; private set; }

		public void Perform(Entity actor, Entity target)
		{
			var action = Switch();
			action.Perform(actor, target);

			IAction<Entity> Switch()
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
