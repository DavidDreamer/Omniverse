using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ModifyPropertyAction : IAction<Entity>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(EntityManager entityManager, Entity actor, Entity target)
		{
			//TODO ECS
			//target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
