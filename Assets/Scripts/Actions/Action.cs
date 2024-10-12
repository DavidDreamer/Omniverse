using UnityEngine;

namespace Omniverse
{
	public abstract class Action : Operation
	{
		[field: SerializeField]
		[field: OperationPicker]
		public Operation Then { get; private set; }

		public override Operation Perform(OperationContext context)
		{
			PerformTemp(context);
			return Then;
		}

		public abstract void PerformTemp(OperationContext context);
	}
}
