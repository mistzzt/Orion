using Orion.Authorization;
using Orion.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Commands
{

	/// <summary>
	/// Interface that describes a command entry in the command list.  All command entry implementations
	/// must implement this interface to be recognized by the command system.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Gets a list of names the command may be referenced by.
		/// </summary>
		IEnumerable<string> Names { get; }

		/// <summary>
		/// Gets a list of parameters for this command.
		/// </summary>
		IEnumerable<IParameter> Parameters { get; }

		/// <summary>
		/// Gets a list of permissions required to run the command
		/// </summary>
		IEnumerable<IPermission> Permissions { get; }

		/// <summary>
		/// Gets a simple summary of the command and what it does.
		/// </summary>
		string Summary { get; }

		/// <summary>
		/// Executes a command and displays its output to the requesting player or console.
		/// </summary>
		/// <param name="player">
		/// A reference to the player (remote player or console) who executed the command
		/// </param>
		void Execute(IPlayer player);
	}
}
