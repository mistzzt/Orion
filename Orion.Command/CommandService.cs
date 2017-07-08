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
	public class HQCommandService : SharedService, ICommandService
	{
		private readonly CommandRegistry _commandRegistry;
		private readonly INetworkService _networkService;
		private readonly IPlayerService _playerService;

		/// <summary>
		/// Constructs a new instance of <see cref="HQCommandService"/>
		public HQCommandService
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

		/// </inheritdoc>
		public void RegisterCommand(Type commandType)
		{
			_commandRegistry.AddCommand(commandType);
		}

		/// <summary>
		/// Registers the chat-packet listener with the network service.
		/// </summary>
		private void RegisterChatListener()
		{
			_networkService.ReceivedPacket += HandlePacket;
		}

		/// <summary>
		/// Constructs a context for use within a command execution.
		/// </summary>
		/// <param name="playerName">Player name who executed the command.</param>
		private ContextObject BuildContextObject(string playerName)
		{
			var context = new ContextObject(_commandRegistry);
			IPlayer player = _playerService.FindPlayers(x => x.WrappedPlayer.name == playerName).Single();
			context.Store<IPlayer>(player);
			return context;
		}

		/// <summary>
		/// Read a received packet and determine if it is a NetTextModule which represents
		/// all chat communication.
		/// </summary>
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

		public void HandleInput(string input, Player player)
		{
			_commandRegistry.HandleInput(input, BuildContextObject(player.Name), null);
		}
	}
}
