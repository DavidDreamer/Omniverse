using System;
using Omniverse.Units;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class MultiAction
	{
		[field: SerializeField]
		[field: ActionPicker]
		public ScriptableObject[] Actions { get; private set; }

		public void Perform(Unit actor)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, Unit>;
				action.Perform(actor, actor);
			}
		}

		public void Perform<TTarget>(Unit actor, TTarget target)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				var action = Actions[i] as Action<Unit, TTarget>;
				action.Perform(actor, target);
			}
		}
	}
}
