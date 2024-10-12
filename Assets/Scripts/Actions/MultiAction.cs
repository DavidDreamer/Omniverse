using System;
using UnityEngine;

namespace Omniverse
{
	[Serializable]
	public class MultiAction
	{
		[field: SerializeField]
		[field: ActionPicker]
		public Action[] Actions { get; private set; }

		public void Perform(ActionContext context)
		{
			for (int i = 0; i < Actions.Length; ++i)
			{
				Actions[i].Perform(context);
			}
		}
	}
}
