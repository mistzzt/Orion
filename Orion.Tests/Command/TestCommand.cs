using HQ.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Tests.Command
{
	[CommandClass]
	public class TestCommand
	{
		[CommandExecutor("Test command which does nothing", "test", HQ.RegexStringOptions.None)]
		public void Executor()
		{

		}
	}
}
