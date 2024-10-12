namespace Omniverse.Actions
{
	public class AddTargetSelf : Action
	{
		public override void Perform(ActionContext context)
		{
			context.Entities.Add(context.Actor);
		}
	}
}
