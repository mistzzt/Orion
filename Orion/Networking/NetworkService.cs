using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiplicity.Packets;
using Orion.Networking.Events;
using OTAPI;
using Terraria;
using Terraria.Localization;
using Terraria.Net.Sockets;
using System.IO;

namespace Orion.Networking
{
	/// <inheritdoc/>
	public class NetworkService : INetworkService
	{
		/// <inheritdoc/>
		public event EventHandler<ReceivedPacketEventArgs> ReceivedPacket;

		/// <inheritdoc/>
		public event EventHandler<ReceivingPacketEventArgs> ReceivingPacket;

		/// <summary>
		/// Constructs the NetworkService and registers OTAPI hooks.
		/// </summary>
		public NetworkService()
		{
			Hooks.Net.ReceiveData += HandleReceiveData;
		}

		private HookResult HandleReceiveData(MessageBuffer buffer,  
			                                 ref byte packetId, 
											 ref int readOffset, 
											 ref int start, 
											 ref int length)
		{
			if (ReceivingPacket == null)
				return HookResult.Continue;

			// Make a copy of the data since I don't actually want to edit the data.
			byte[] dataCopy = buffer.readBuffer.Skip(start).Take(length).ToArray();
			TerrariaPacket resultPacket;

			using (var ms = new MemoryStream(dataCopy))
			using (var br = new BinaryReader(ms))
			{
				resultPacket = TerrariaPacket.Deserialize(br);
			}

			var eventArgs = new ReceivingPacketEventArgs(Netplay.Clients[buffer.whoAmI], resultPacket);

			ReceivingPacket(this, eventArgs);

			return HookResult.Continue;
		}

		/// <inheritdoc/>
		public void SendPacket(int target, TerrariaPacket packet)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public void BroadcastPacket(TerrariaPacket packet)
		{
			throw new NotImplementedException();
		}
	}
}
