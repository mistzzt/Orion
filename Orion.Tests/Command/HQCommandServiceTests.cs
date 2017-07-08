using NUnit.Framework;
using Orion.Command;
using Orion.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Tests.Command
{
	[TestFixture]
	public class HQCommandServiceTests
	{
		public void CommandService_RegisterCommand()
		{
			using (Orion orion = new Orion())
			{
				ICommandService commandService = orion.GetService<HQCommandService>();
				commandService.RegisterCommand(typeof(TestCommand));
			}
		}

		[TestCase("/test")]
		public void CommandService_HandleInput(string input)
		{
			var testPlayer = new Terraria.Player() { name = "Test" };
			using (Orion orion = new Orion())
			{
				ICommandService commandService = orion.GetService<HQCommandService>();
				commandService.RegisterCommand(typeof(TestCommand));

				commandService.HandleInput(input, new Player(testPlayer));
			}
		}
	}
}
