using UnityEngine;

namespace Omniverse
{
	public abstract class Operation : ScriptableObject
	{
		public abstract void Perform(ActionContext context);
	}
}
