using VContainer;

namespace Omniverse
{
	public class ActionHandler
	{
		[Inject]
		public IObjectResolver ObjectResolver { get; set; }

		public void Perform(MultiAction multiAction, Entity actor, ActionContext context)
		{
			ObjectResolver.Inject(context);
			multiAction.Perform(context);
		}
	}
}
