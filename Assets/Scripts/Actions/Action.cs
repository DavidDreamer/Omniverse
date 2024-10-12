using UnityEngine;

namespace Omniverse
{
	public abstract class Action : Operation
	{
		[field: SerializeField]
		[field: ActionPicker]
		public Operation Then { get; private set; }
	}
}
