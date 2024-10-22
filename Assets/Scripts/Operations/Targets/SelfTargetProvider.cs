namespace Omniverse
{
	public class SelfTargetProvider : ITargetProvider<Unit>
	{
		public Unit Get(Unit actor) => actor;
	}
}
