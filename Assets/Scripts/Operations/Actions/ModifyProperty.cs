using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class ModifyProperty : IAction<Unit>
	{
		[field: SerializeField]
		public PropertyID Property { get; private set; }

		[field: SerializeField]
		public PropertyModifier Modifier { get; private set; }

		public void Perform(Entity actor, Unit target)
		{
			target.ModifyProperty(Property, Modifier, actor);
		}
	}
}
