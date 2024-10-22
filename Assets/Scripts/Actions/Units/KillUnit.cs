namespace Omniverse.Actions
{
	public class KillUnit : Action<Unit, Unit>
	{
		public override void Perform(Unit actor, Unit target)
		{
			target.Die();
		}
	}
}
