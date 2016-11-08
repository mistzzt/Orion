using Orion.Players;

namespace Orion.Commands
{
	public class CommandContext
	{
		/// <summary>
		/// Gets a reference to the command that was executed in this context.
		/// </summary>
		public ICommand Command { get; }

		/// <summary>
		/// Gets a reference to the source IPlayer (remote or console) executing the command
		/// in this context.
		/// </summary>
		public IPlayer Source { get; }

		/// <summary>
		/// Gets a full representation of the command line that was typed by the <see cref="Source"/>.
		/// </summary>
		public string CommandLine { get; internal set; }
	}
}
