namespace Omniverse.Units
{
	public interface IImmediateCommand
	{
		void Execute();
	}

	public abstract class ImmediateCommand : IImmediateCommand
	{
		protected Unit Unit { get; }

		public ImmediateCommand(Unit unit)
		{
			Unit = unit;
		}

		public abstract void Execute();
	}
}
