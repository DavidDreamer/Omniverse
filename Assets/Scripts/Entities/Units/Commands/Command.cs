
namespace Omniverse.Units
{
	public interface ICommand
	{
		bool IsCompleted { get; }

		void Start();

		void Tick(float deltaTime);

		void Cleanup();
	}

	public abstract class Command : ICommand
	{
		protected Unit Unit { get; }

		public virtual bool IsCompleted => false;

		protected Command(Unit unit)
		{
			Unit = unit;
		}

		public virtual void Start()
		{
		}

		public virtual void Tick(float deltaTime)
		{
		}

		public virtual void Cleanup()
		{
		}
	}
}
