using System;
using Orion.Players;

namespace Orion.Command
{
	/// <summary>
	/// Interface defining a service which allows for the registration and execution of commands.
	/// </summary>
	public interface ICommandService
	{
		/// <summary>
		/// Register a command to the service.
		/// </summary>
		void RegisterCommand(Type commandType);

		/// <summary>
		/// Handles the provided text as command input.
		/// </summary>
		/// <param name="input">Text input.</param>
		/// <param name="caller">Calling player</param>
		void HandleInput(string input, Player caller);
	}
}