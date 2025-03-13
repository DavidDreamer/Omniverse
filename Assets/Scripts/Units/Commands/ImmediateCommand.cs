namespace Omniverse
{
	public interface IImmediateCommand
	{
		void Execute();
	}

	public abstract class ImmediateCommand : IImmediateCommand
	{
		protected UnitObsolete Unit { get; }

		public ImmediateCommand(UnitObsolete unit)
		{
			Unit = unit;
		}

		public abstract void Execute();
	}
}
