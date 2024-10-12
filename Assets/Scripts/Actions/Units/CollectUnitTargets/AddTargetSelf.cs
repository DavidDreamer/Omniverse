namespace Omniverse.Actions
{
	public class AddTargetSelf : Action
	{
		public override void PerformTemp(OperationContext context)
		{
			context.Entities.Add(context.Actor);
		}
	}
}
