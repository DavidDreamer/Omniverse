using Omniverse.Units;
using UnityEngine;

namespace Omniverse.Actions
{
	public class ModifyProperty : ScriptableObject, IAction<Unit, Unit>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(Unit actor, Unit target)
		{
			target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
