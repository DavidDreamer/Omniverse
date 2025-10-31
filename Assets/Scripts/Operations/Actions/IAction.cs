using Unity.Entities;

namespace Omniverse
{
	public interface IAction<in TTarget>
	{
		public void Perform(EntityManager entityManager, Entity actor, TTarget target);
	}
}
