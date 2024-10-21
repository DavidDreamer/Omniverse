using System.Collections.Generic;

namespace Omniverse.Units
{
	public class CommandModule
	{
		private Unit Unit { get; }

		private Queue<ICommand> Commands { get; } = new();

		private Queue<IImmediateCommand> ImmediateCommands { get; } = new();

		public ICommand Command { get; private set; }

		public CommandModule(Unit unit)
		{
			Unit = unit;
		}

		public void Tick(float deltaTime)
		{
			while (ImmediateCommands.Count > 0)
			{
				IImmediateCommand command = ImmediateCommands.Dequeue();
				command.Execute();
			}

			if (Command == null)
			{
				if (Commands.Count == 0)
				{
					return;
				}
				else
				{
					Command = Commands.Dequeue();
					Command.Start();
				}
			}

			while (true)
			{
				bool completed = Command.Tick(deltaTime);
				if (completed)
				{
					Command.Cleanup();

					if (Commands.Count > 0)
					{
						Command = Commands.Dequeue();
						Command.Start();
					}
					else
					{
						Command = null;
						break;
					}
				}
				else
				{
					break;
				}
			}
		}

		public void Add(IImmediateCommand command) => ImmediateCommands.Enqueue(command);

		public void Add(ICommand command) => Commands.Enqueue(command);

		public void Reset()
		{
			if (Command != null)
			{
				Command.Cleanup();
				Command = null;
			}

			Commands.Clear();
		}
	}
}
