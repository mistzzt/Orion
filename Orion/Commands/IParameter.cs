using System;

namespace Orion.Commands
{
	/// <summary>
	/// Interface that describes a command parameter.
	/// </summary>
	public interface IParameter
	{
		/// <summary>
		/// Gets the internal name of the parameter.  Used in named parameters where the name
		/// must be referenced by a switch character (eg. -test)
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets the type of parameter, whether positional or named.
		/// </summary>
		ParameterType ParameterType { get; }

		/// <summary>
		/// Gets the .NET type of the value of the parameter.
		/// </summary>
		Type ValueType { get; }


		/// <summary>
		/// Attempts to cast the value of this parameter (as bound by the model binder) to the type
		/// specified by <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">
		/// T is a the type of the parameter to cast the value of the parameter as
		/// </typeparam>
		/// <returns>
		/// The value of the parameter cast to T.
		/// </returns>
		/// <exception cref="InvalidCastException">
		/// Thrown when the CLR cannot cast the value of the parameter to 
		/// </exception>
		T ValueAs<T>();
	}
}
