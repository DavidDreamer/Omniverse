using Unity.Entities;

namespace Omniverse
{
	public interface IImmediateCommand
	{
		void Execute(EntityManager entityManager);
	}

	public abstract class ImmediateCommand : IImmediateCommand
	{
		protected DynamicEntity Entity { get; }

		public ImmediateCommand(DynamicEntity entity)
		{
			Entity = entity;
		}

		public abstract void Execute(EntityManager entityManager);
	}
}
