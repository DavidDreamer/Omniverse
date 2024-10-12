using UnityEngine;

namespace Omniverse
{
	public abstract class Action : ScriptableObject
	{
		public abstract void Perform(ActionContext context);
	}
}
