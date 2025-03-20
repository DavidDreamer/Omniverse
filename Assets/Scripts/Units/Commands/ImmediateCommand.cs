using Unity.Entities;

namespace Omniverse
{
	public interface IImmediateCommand
	{
		void Execute();
	}

	public abstract class ImmediateCommand : IImmediateCommand
	{
		protected Entity Entity { get; }

		public ImmediateCommand(Entity entity)
		{
			Entity = entity;
		}

		public abstract void Execute();
	}
}
