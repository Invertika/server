using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ISL.Server.Network;

namespace invertika_account.Account
{
	/**
 * Stores address, maps, and statistics, of a connected game server.
 */
	public class GameServer : NetComputer
	{
		public GameServer(TcpClient peer): base(peer)
		{
			port=0;
		}

		//std::string address;
		//NetComputer *server;
		//ServerStatistics maps;
		short port;
	}
}
