using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Utilities;
using ISL.Server.Utilities;
using System.IO;
using ISL.Server.Network;
using ISL.Server.Common;

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
		}

		public static void process()
		{
			serverHandler.process(50);
		}


		public static void registerGameClient(GameServer s, string token, Character ptr)
		{
			MessageOut msg=new MessageOut(Protocol.AGMSG_PLAYER_ENTER);
			msg.writeString(token);
			msg.writeInt32(ptr.getDatabaseID());
			msg.writeString(ptr.getName());
			//serializeCharacterData(ptr, msg); //TODO wieder einbauen
			s.send(msg);
		}

		public static GameServer getGameServerFromMap(int mapId)
		{
			//foreach(NetComputer client in clients)
			//{

			//for (ServerHandler::NetComputers::const_iterator
			//     i = serverHandler->clients.begin(),
			//     i_end = serverHandler->clients.end(); i != i_end; ++i)
			//{
			//    GameServer *server = static_cast< GameServer * >(*i);
			//    ServerStatistics::const_iterator i = server->maps.find(mapId);
			//    if (i == server->maps.end()) continue;
			//    return server;
			//}
			return null;
		}

		public static bool getGameServerFromMap(int mapId, string address, int port)
		{
			//if (GameServer *s = ::getGameServerFromMap(mapId))
			//{
			//    address = s->address;
			//    port = s->port;
			//    return true;
			//}
			return false;
		}

		public static void registerClient(string token, Character ptr)
		{
			//GameServer *s = ::getGameServerFromMap(ptr->getMapId());
			//assert(s);
			//registerGameClient(s, token, ptr);
		}


		public static void sendPartyChange(Character ptr, int partyId)
		{
			//GameServer *s = ::getGameServerFromMap(ptr->getMapId());
			//if (s)
			//{
			//    MessageOut msg(CGMSG_CHANGED_PARTY);
			//    msg.writeInt32(ptr->getDatabaseID());
			//    msg.writeInt32(partyId);
			//    s->send(msg);
			//}
		}

		public static void syncDatabase(MessageIn msg)
		{
			//// It is safe to perform the following updates in a transaction
			//dal::PerformTransaction transaction(storage->database());

			//while (msg.getUnreadLength() > 0)
			//{
			//    int msgType = msg.readInt8();
			//    switch (msgType)
			//    {
			//        case SYNC_CHARACTER_POINTS:
			//        {
			//            LOG_DEBUG("received SYNC_CHARACTER_POINTS");
			//            int charId = msg.readInt32();
			//            int charPoints = msg.readInt32();
			//            int corrPoints = msg.readInt32();
			//            storage->updateCharacterPoints(charId, charPoints, corrPoints);
			//        } break;

			//        case SYNC_CHARACTER_ATTRIBUTE:
			//        {
			//            LOG_DEBUG("received SYNC_CHARACTER_ATTRIBUTE");
			//            int    charId = msg.readInt32();
			//            int    attrId = msg.readInt32();
			//            double base   = msg.readDouble();
			//            double mod    = msg.readDouble();
			//            storage->updateAttribute(charId, attrId, base, mod);
			//        } break;

			//        case SYNC_CHARACTER_SKILL:
			//        {
			//            LOG_DEBUG("received SYNC_CHARACTER_SKILL");
			//            int charId = msg.readInt32();
			//            int skillId = msg.readInt8();
			//            int skillValue = msg.readInt32();
			//            storage->updateExperience(charId, skillId, skillValue);
			//        } break;

			//        case SYNC_ONLINE_STATUS:
			//        {
			//            LOG_DEBUG("received SYNC_ONLINE_STATUS");
			//            int charId = msg.readInt32();
			//            bool online = (msg.readInt8() == 1);
			//            storage->setOnlineStatus(charId, online);
			//        }
			//    }
			//}

			//transaction.commit();
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
			//        const MapStatistics m = j->second;
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
