using Dreambox.Core;
using Unity.Entities;
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

		public void Perform(EntityManager entityManager, Entity actor, Entity target)
		{
			var action = Switch();
			action.Perform(entityManager, actor, target);

			IAction<Entity> Switch()
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
