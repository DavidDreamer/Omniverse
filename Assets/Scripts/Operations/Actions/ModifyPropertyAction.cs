using UnityEngine;

namespace Omniverse
{
	public class ModifyPropertyAction : IAction<Entity>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(Entity actor, Entity target)
		{
			target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
