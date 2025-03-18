using Dreambox.Core;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFactionAction : IAction<OmniverseEntity>
	{
		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<OmniverseEntity>))]
		public IAction<OmniverseEntity> Self { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<OmniverseEntity>))]
		public IAction<OmniverseEntity> Ally { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<OmniverseEntity>))]
		public IAction<OmniverseEntity> Enemy { get; private set; }

		public void Perform(OmniverseEntity actor, OmniverseEntity target)
		{
			var action = Switch();
			action.Perform(actor, target);

			IAction<OmniverseEntity> Switch()
			{
				if (actor == target)
				{
					return Self;
				}
				//TODO ECS
				//else if (actor.IsAllyFor(target))
				//{
					return Ally;
				//}
				//else
				//{
					//return Enemy;
				//}
			}
		}
	}
}
