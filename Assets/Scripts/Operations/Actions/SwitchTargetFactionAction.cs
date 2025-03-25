using Dreambox.Core;
using Unity.Entities;
using UnityEngine;

namespace Omniverse.Actions
{
	public class SwitchTargetFactionAction : IAction<DynamicEntity>
	{
		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<DynamicEntity>))]
		public IAction<DynamicEntity> Self { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<DynamicEntity>))]
		public IAction<DynamicEntity> Ally { get; private set; }

		[field: SerializeReference]
		[field: VersatileOptional(typeof(IAction<DynamicEntity>))]
		public IAction<DynamicEntity> Enemy { get; private set; }

		public void Perform(EntityManager entityManager, DynamicEntity actor, DynamicEntity target)
		{
			var action = Switch();
			action.Perform(entityManager, actor, target);

			IAction<DynamicEntity> Switch()
			{
				if (actor.Entity == target.Entity)
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
