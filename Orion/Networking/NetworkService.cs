using System;
using Multiplicity.Packets;
using Orion.Networking.Events;
using OTAPI;
using Terraria;
using Terraria.Net.Sockets;
using System.IO;
using Orion.Framework;

namespace Orion.Networking
{
	/// <inheritdoc/>
	[Service("Network Service", Author = "Nyx Studios")]
	public class NetworkService : SharedService, INetworkService
	{
		/// <inheritdoc/>
		public event EventHandler<ReceivedPacketEventArgs> ReceivedPacket;

		/// <inheritdoc/>
		public event EventHandler<ReceivingPacketEventArgs> ReceivingPacket;

		/// <summary>
		/// Constructs the NetworkService and registers OTAPI hooks.
		/// </summary>
		public NetworkService(Orion orion) : base(orion)
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

			// ArraySegments aren't data copies. I initially made a copy of the readBuffer but realized
			// that was stupid since I wasn't editing the original data.
			ArraySegment<byte> dataSegment = new ArraySegment<byte>(buffer.readBuffer, start, length);

			TerrariaPacket resultPacket;
			using (var ms = new MemoryStream(dataSegment.Array))
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
			ISocket clientSocket = Netplay.Clients[target]?.Socket;
			byte[] payload = packet.ToArray();

			//  TODO: Determine if an exception is more appropriate here
			if (clientSocket == null)
			{
				return;
			}

			clientSocket.AsyncSend(payload, 0, payload.Length, null);
		}

		/// <inheritdoc/>
		public void BroadcastPacket(TerrariaPacket packet)
		{
			byte[] payload = packet.ToArray();

			foreach (RemoteClient client in Netplay.Clients)
			{
				ISocket clientSocket = client?.Socket;

				if (clientSocket == null)
				{
					continue;
				}

				clientSocket.AsyncSend(payload, 0, payload.Length, null);
			}
		}
	}
}
