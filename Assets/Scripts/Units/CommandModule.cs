using System.Collections.Generic;
using Unity.Entities;

namespace Omniverse
{
	public partial struct ProcessCommandsSystem : ISystem
	{
		public void OnUpdate(ref SystemState state)
		{
			foreach (var commandModule in SystemAPI.Query<CommandModule>())
			{
				commandModule.Tick(ref state);
			}
		}
	}

	public class CommandModule : IComponentData
	{
		private Queue<ICommand> Commands { get; } = new();

		private Queue<IImmediateCommand> ImmediateCommands { get; } = new();

		public ICommand Command { get; private set; }

		public void Tick(ref SystemState state)
		{
			while (ImmediateCommands.Count > 0)
			{
				IImmediateCommand command = ImmediateCommands.Dequeue();
				command.Execute(state.EntityManager);
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
					Command.Start(ref state);
				}
			}

			while (true)
			{
				bool completed = Command.Tick(ref state);
				if (completed)
				{
					Command.Cleanup(ref state);

					if (Command.IsRepeatable)
					{
						Command.Start(ref state);
						break;
					}
					{
						if (Commands.Count > 0)
						{
							Command = Commands.Dequeue();
							Command.Start(ref state);
						}
						else
						{
							Command = null;
							break;
						}
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

		public void Reset(ref SystemState state)
		{
			if (Command != null)
			{
				Command.Cleanup(ref state);
				Command = null;
			}

			Commands.Clear();
		}
	}
}
