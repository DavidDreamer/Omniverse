using UnityEngine;

namespace Omniverse.Actions
{
	public class ModifyProperty : Action<Unit, Unit>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public override void Perform(Unit actor, Unit target)
		{
			target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
