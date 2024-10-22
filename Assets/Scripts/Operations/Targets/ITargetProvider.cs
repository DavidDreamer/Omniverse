namespace Omniverse
{
	public interface ITargetProvider<TTarget>
	{
		TTarget Get(Entity actor);
	}
}
