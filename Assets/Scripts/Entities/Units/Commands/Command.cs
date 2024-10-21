
namespace Omniverse.Units
{
	public interface ICommand
	{
		void Start();

		bool IsRepeatable { get; }

		bool Tick(float deltaTime);

		void Cleanup();
	}

	public abstract class Command : ICommand
	{
		protected Unit Unit { get; }

		public virtual bool IsRepeatable => false;

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
