namespace Omniverse
{
	public interface IAction<in TTarget>
	{
		public void Perform(OmniverseEntity actor, TTarget target);
	}
}
