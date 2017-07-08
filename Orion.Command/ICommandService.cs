using System;

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
		/// <param name="commandType"></param>
		void RegisterCommand(Type commandType);
	}
}