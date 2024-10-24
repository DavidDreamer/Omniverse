namespace Omniverse
{
	public interface IAction<in TTarget>
	{
		public void Perform(Entity actor, TTarget target);
	}

}
