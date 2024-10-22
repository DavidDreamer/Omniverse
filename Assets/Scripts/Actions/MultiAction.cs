using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class MultiAction
	{
		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject[] Actions { get; private set; }

		public void Perform(Entity actor)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, Unit>;
				action.Perform((Unit)actor, (Unit)actor);
			}
		}

		public void Perform<TTarget>(Entity actor, TTarget target)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, TTarget>;
				action.Perform((Unit)actor, target);
			}
		}
	}
}
