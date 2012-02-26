using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Utilities;
using ISL.Server.Utilities;
using System.IO;

namespace invertika_account.Account
{
	public static class GameServerHandler
	{
		static ServerHandler serverHandler;

		public static bool initialize(int port, string host)
		{
			serverHandler=new ServerHandler();
			Logger.Add(LogLevel.Information, "Game server handler started:");
			return serverHandler.startListen((ushort)port, host);
		}

		public static void deinitialize()
		{
			serverHandler.stopListen();
			//delete serverHandler;
		}

		public static void process()
		{
			serverHandler.process(50);
		}


		public static void dumpStatistics(StreamWriter os)
		{
			//for (ServerHandler::NetComputers::const_iterator
			//     i = serverHandler->clients.begin(),
			//     i_end = serverHandler->clients.end(); i != i_end; ++i)
			//{
			//    GameServer *server = static_cast< GameServer * >(*i);
			//    if (!server->port)
			//        continue;

			//    os << "<gameserver address=\"" << server->address << "\" port=\""
			//       << server->port << "\">\n";

			//    for (ServerStatistics::const_iterator j = server->maps.begin(),
			//         j_end = server->maps.end(); j != j_end; ++j)
			//    {
			//        const MapStatistics &m = j->second;
			//        os << "<map id=\"" << j->first << "\" nb_things=\"" << m.nbThings
			//           << "\" nb_monsters=\"" << m.nbMonsters << "\">\n";
			//        for (std::vector< int >::const_iterator k = m.players.begin(),
			//             k_end = m.players.end(); k != k_end; ++k)
			//        {
			//            os << "<character id=\"" << *k << "\"/>\n";
			//        }
			//        os << "</map>\n";
			//    }
			//    os << "</gameserver>\n";
			//}
		}
	}
}
