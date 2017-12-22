using System;
using Multiplicity.Packets;
using Orion.Networking.Events;
using Orion.Framework;

namespace Orion.Networking
{
	/// <summary>
	/// Provides a mechanism for managing the network.
	/// </summary>
	public interface INetworkService : ISharedService
	{
		/// <summary>
		/// Occurs when a packet was received.
		/// </summary>
		event EventHandler<ReceivedPacketEventArgs> ReceivedPacket;
		
		/// <summary>
		/// Occurs when a packet is being received.
		/// </summary>
		event EventHandler<ReceivingPacketEventArgs> ReceivingPacket; 

		/// <summary>
		/// Sends the specified packet to the target.
		/// </summary>
		/// <param name="target">The target player id.</param>
		/// <param name="packet">The packet to send to the targeted player.</param>
		void SendPacket(int target, TerrariaPacket packet);

		/// <summary>
		/// Sends the specified packet to all players.
		/// </summary>
		/// <param name="packet">The packet to send to all players.</param>
		void BroadcastPacket(TerrariaPacket packet);
	}
}
