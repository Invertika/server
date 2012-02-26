using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ISL.Server.Network;

namespace invertika_account.Chat
{
	public class ChatClient : NetComputer
	{
		public ChatClient(TcpClient peer): base(peer)
		{
			// NetComputer(peer),
			//party(0),
			//accountLevel(0)
		}

        public string characterName;
        uint characterId;
        List<ChatChannel> channels;
        Party party;
        byte accountLevel;
        Dictionary<ChatChannel, string> userModes;
	}
}
