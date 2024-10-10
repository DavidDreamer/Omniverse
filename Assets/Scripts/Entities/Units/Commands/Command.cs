
namespace Omniverse.Units
{
	public interface ICommand
	{
		void Start();

		bool Tick(float deltaTime);

		void Cleanup();
	}

	public abstract class Command : ICommand
	{
		protected Unit Unit { get; }

		protected Command(Unit unit)
		{
			Unit = unit;
		}

		public virtual void Start()
		{
		}

		public virtual bool Tick(float deltaTime)
		{
			return false;
		}

		public virtual void Cleanup()
		{
		}
	}
}
