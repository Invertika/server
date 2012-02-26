using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Network;
using System.Net.Sockets;

namespace invertika_game.Game
{
	public class GameClient : NetComputer
	{
		public GameClient(TcpClient peer): base(peer)
		{
			//character(NULL), status(CLIENT_LOGIN) {}
		}

		Character character;
		int status;
	}
}
