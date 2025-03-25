using Unity.Entities;

namespace Omniverse
{
	public interface IAction<in TTarget>
	{
		public void Perform(EntityManager entityManager, DynamicEntity actor, TTarget target);
	}
}
