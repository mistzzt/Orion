using HQ;
using Orion.Configuration;
using Orion.Framework;
using Orion.Networking;
using Orion.Networking.Events;
using System;
using Multiplicity.Packets;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Orion.Players;

namespace Orion.Command
{
	/// <summary>
	/// Command service which uses the Headquarters command library to register,
	/// parse, and execute commands.
	/// </summary>
	[Service("Headquarters Based Command Service", Author = "Nyx Studios")]
	public class CommandService : SharedService
	{
		private readonly CommandRegistry _commandRegistry;
		private readonly INetworkService _networkService;
		private readonly IPlayerService _playerService;

		public CommandService
			(
			Orion orion, 
			JsonFileConfigurationService<RegistrySettings> configuration,
			INetworkService networkService,
			IPlayerService playerService) 
			: base(orion)
		{
			_networkService = networkService;
			_playerService = playerService;
			RegistrySettings settings = configuration.Configuration;
			_commandRegistry = new CommandRegistry(settings);
		}

		public void RegisterCommand(Type commandType)
		{
			_commandRegistry.AddCommand(commandType);
		}

		private void RegisterChatListener()
		{
			_networkService.ReceivedPacket += HandlePacket;
		}

		private ContextObject BuildContextObject(string name)
		{
			var context = new ContextObject(_commandRegistry);
			IPlayer player = _playerService.FindPlayers(x => x.WrappedPlayer.name == name).Single();
			context.Store<IPlayer>(player);
			return context;
		}

		private void HandlePacket(object sender, ReceivedPacketEventArgs e)
		{
			if (e.Packet.PacketType == PacketTypes.LoadNetModule)
			{
				LoadNetModule packet = e.Packet as LoadNetModule;

				if (packet == null)
				{
					return;
				}

				if (packet.LoadedModule.ID == (int)NetworkModuleTypes.NetTextModule)
				{
					NetTextModule netModule = packet.LoadedModule as NetTextModule;

					if (netModule == null)
					{
						return;
					}

					if (String.Equals(netModule.ChatCommand, "say", StringComparison.InvariantCultureIgnoreCase))
					{
						ContextObject ctx = BuildContextObject(e.Sender.Name);
						_commandRegistry.HandleInput(netModule.ChatMessage, null, null);
					}
				}
			}
		}
	}
}
