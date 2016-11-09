using Orion.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Commands
{
	/// <summary>
	/// Service definition : Command Pipeline Service
	/// 
	/// Command pipeline service is a shared component which contains the entire list of commands, and
	/// facilitates the add/removal of ICommand instances from various command services in Orion.
	/// </summary>
	public interface ICommandPipelineService : ISharedService
	{

		/// <summary>
		/// Finds a list of commands, optionally matching the specified filter predicate.
		/// </summary>
		/// <param name="filter">
		/// A LINQ-style predicate expression to filter the command list by
		/// </param>
		/// <returns>
		/// An <see cref="ICommand"/> instance containing the first command filtered by the predicate if one
		/// is provided, or every command if a filter is not provided.
		/// </returns>
		IEnumerable<ICommand> Find(Predicate<ICommand> filter = null);

		/// <summary>
		/// Finds a list of commands matching the specified <paramref name="name"/>, and optionally further
		/// filtering by a specified <paramref name="filter"/> predicate.
		/// </summary>
		/// <param name="name">
		/// A string containing the name or alias of a command to search the commandlist for.
		/// </param>
		/// <param name="filter">
		/// (optional) A LINQ-style filter expression to be used for further filtering on a command.
		/// </param>
		/// <returns>
		/// An IEnumerable containing a list of all the command instances matching the provided filters.
		/// If none are not found, the enumerable will be empty.
		/// </returns>
		IEnumerable<ICommand> FindByName(string name, Predicate<ICommand> filter = null);

		/// <summary>
		/// Appends a <see cref="CommandHandlerDelegate"/> handler to the end of the handler list for the
		/// specified <paramref name="command"/>.
		/// </summary>
		/// <param name="command">
		/// An ICommand instance to append a handler to
		/// </param>
		/// <param name="handler">
		/// A method matching the <see cref="CommandHandlerDelegate"/> signature to be called when the command
		/// is invoked
		/// </param>
		void AddHandler(ICommand command, CommandHandlerDelegate handler);

		/// <summary>
		/// Inserts a <see cref="CommandHandlerDelegate"/> method at the specifed <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// A number relating to the index of the handler list to insert at.  Use 0 to append to the start of the list.
		/// </param>
		/// <param name="command">
		/// An ICommand instance to append a handler to
		/// </param>
		/// <param name="handler">
		/// A method matching the <see cref="CommandHandlerDelegate"/> signature to be called when the command
		/// is invoked
		/// </param>
		void InsertHandler(ICommand command, int index, CommandHandlerDelegate handler);
	}
}
