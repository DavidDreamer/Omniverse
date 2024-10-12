using UnityEngine;

namespace Omniverse
{
	public abstract class Operation : ScriptableObject
	{
		public abstract Operation Perform(OperationContext context);
	}
}
