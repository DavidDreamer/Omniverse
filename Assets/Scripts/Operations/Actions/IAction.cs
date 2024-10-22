namespace Omniverse
{
	public interface IAction<TTarget>
	{
		public void Perform(Entity actor, TTarget target);
	}

}
