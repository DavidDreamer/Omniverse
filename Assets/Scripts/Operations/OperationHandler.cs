using VContainer;

namespace Omniverse
{
	public class OperationHandler
	{
		[Inject]
		public IObjectResolver ObjectResolver { get; set; }

		public void Perform(Operation operation, Entity actor)
		{
			var context = new OperationContext(actor);
			ObjectResolver.Inject(context);

			do
			{
				operation = operation.Perform(context);
			}
			while (operation != null);
		}

		public void Perform(Operation operation, Entity actor, OperationContext context)
		{
			ObjectResolver.Inject(context);

			do
			{
				operation = operation.Perform(context);
			}
			while (operation != null);
		}
	}
}
