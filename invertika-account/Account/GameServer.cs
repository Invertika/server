using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ISL.Server.Network;

namespace invertika_account.Account
{
	/// <summary>
	/// Stores address, maps, and statistics, of a connected game server.
	/// </summary>
	public class GameServer : NetComputer
	{
		public GameServer(TcpClient peer) : base(peer)
		{
			port=0;
			maps=new Dictionary<ushort, MapStatistics>();
		}

		public string address;
		//NetComputer *server;
		public Dictionary<ushort, MapStatistics> maps;
		public short port;
	}
}
