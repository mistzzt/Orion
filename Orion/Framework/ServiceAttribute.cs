﻿using System;

namespace Orion.Framework
{
	/// <summary>
	/// Provides information about a service.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ServiceAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the service's author.
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Gets the service's name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceAttribute"/> class.
		/// </summary>
		/// <param name="name">The service's name.</param>
		public ServiceAttribute(string name)
		{
			Name = name;
		}
	}
}
