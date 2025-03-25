using Unity.Entities;
using UnityEngine;

namespace Omniverse
{
	public class ModifyPropertyAction : IAction<DynamicEntity>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(EntityManager entityManager, DynamicEntity actor, DynamicEntity target)
		{
			//TODO ECS
			//target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
