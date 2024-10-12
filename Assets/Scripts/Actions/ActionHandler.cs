using VContainer;

namespace Omniverse
{
	public class ActionHandler
	{
		[Inject]
		public IObjectResolver ObjectResolver { get; set; }

		public void Perform(Operation operation, Entity actor, ActionContext context)
		{
			ObjectResolver.Inject(context);

			do
			{
				operation.Perform(context);

				//TEMP
				if (operation is Action action)
				{
					operation = action.Then;
				}
				else
				{
					operation = null;
				}
			}
			while (operation != null);
		}
	}
}
