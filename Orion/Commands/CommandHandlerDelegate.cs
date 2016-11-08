using Orion.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Commands
{
	/// <summary>
	/// Delegate that describes a command handler method signature.  These methods are registered into
	/// a command and are invoked one after another to provide implementations for a command.
	/// </summary>
	/// <param name="source">
	/// A reference to an <see cref="IPlayer"/> object (remote or console) who invoked the command
	/// </param>
	/// <param name="environment">
	/// A reference to the <see cref="CommandContext"/> which contains the parsed parameters for
	/// the command, and a reference to the output streams in which the command handlers are to write
	/// their output.
	/// </param>
	public delegate void CommandHandlerDelegate(IPlayer source, CommandContext environment);
}
