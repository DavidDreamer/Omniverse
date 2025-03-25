using Unity.Entities;

namespace Omniverse
{
	public interface ICommand
	{
		void Start(ref SystemState state);

		bool IsRepeatable { get; }

		bool Tick(ref SystemState state);

		void Cleanup(ref SystemState state);
	}

	public abstract class Command : ICommand
	{
		protected DynamicEntity Entity { get; }

		public virtual bool IsRepeatable => false;

		protected Command(DynamicEntity entity)
		{
			Entity = entity;
		}

		public virtual void Start(ref SystemState state)
		{
		}

		public virtual bool Tick(ref SystemState state)
		{
			return false;
		}

		public virtual void Cleanup(ref SystemState state)
		{
		}
	}
}
