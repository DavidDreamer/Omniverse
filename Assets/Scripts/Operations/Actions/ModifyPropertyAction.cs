using UnityEngine;

namespace Omniverse
{
	public class ModifyPropertyAction : IAction<OmniverseEntity>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(OmniverseEntity actor, OmniverseEntity target)
		{
			target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
