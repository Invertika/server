//
//  GameServerHandler.cs
//
//  This file is part of Invertika (http://invertika.org)
// 
//  Based on The Mana Server (http://manasource.org)
//  Copyright (C) 2004-2012  The Mana World Development Team 
//
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by Invertika Development Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Utilities;
using ISL.Server.Utilities;
using System.IO;
using ISL.Server.Network;
using ISL.Server.Common;
using System.Diagnostics;
using ISL.Server.Serialize;
using ISL.Server.Account;
using ISL.Server.Enums;

namespace invertika_account.Account
{
	public static class GameServerHandler
	{
		static ServerHandler serverHandler;

		public static bool initialize(int port, string host)
		{
			serverHandler=new ServerHandler();
			Logger.Write(LogLevel.Information, "Game server handler started:");
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
			CharacterData.serializeCharacterData(ptr, msg);
			s.send(msg);
		}

		public static GameServer getGameServerFromMap(int mapId)
		{
			foreach(NetComputer client in serverHandler.clients)
			{
				GameServer server=(GameServer)(client);

				if(server.maps.ContainsKey((ushort)mapId))
				{
					return server;
				}
			}
			
			return null;
		}

		public static bool getGameServerFromMap(int mapId, out string address, out int port)
		{
			GameServer s=getGameServerFromMap(mapId);

			if(s!=null)
			{
				address=s.address;
				port=s.port;
				return true;
			}

			address="";
			port=0;

			return false;
		}

		public static void registerClient(string token, Character ptr)
		{
			GameServer s=getGameServerFromMap(ptr.getMapId());
			registerGameClient(s, token, ptr);
		}

		public static void sendPartyChange(Character ptr, int partyId)
		{
			GameServer s=getGameServerFromMap(ptr.getMapId());

			if(s!=null)
			{
				MessageOut msg=new MessageOut(Protocol.CGMSG_CHANGED_PARTY);
				msg.writeInt32(ptr.getDatabaseID());
				msg.writeInt32(partyId);
				s.send(msg);
			}
		}

		public static void syncDatabase(MessageIn msg)
		{
			// It is safe to perform the following updates in a transaction
			//dal::PerformTransaction transaction(storage.database());

			while(msg.getUnreadLength()>0)
			{
				int msgType=msg.readInt8();
				switch((Sync)msgType)
				{
					case Sync.SYNC_CHARACTER_POINTS:
						{
							Logger.Write(LogLevel.Debug, "received SYNC_CHARACTER_POINTS");
							int charId=msg.readInt32();
							int charPoints=msg.readInt32();
							int corrPoints=msg.readInt32();
							Program.storage.updateCharacterPoints(charId, charPoints, corrPoints);
							break;
						}
					case Sync.SYNC_CHARACTER_ATTRIBUTE:
						{
							Logger.Write(LogLevel.Debug, "received SYNC_CHARACTER_ATTRIBUTE");
							int charId=msg.readInt32();
							int attrId=msg.readInt32();
							double @base=msg.readDouble();
							double mod=msg.readDouble();
							Program.storage.updateAttribute(charId, (uint)attrId, @base, mod);
							break;
						}
					case Sync.SYNC_CHARACTER_SKILL:
						{
							Logger.Write(LogLevel.Debug, "received SYNC_CHARACTER_SKILL");
							int charId=msg.readInt32();
							int skillId=msg.readInt8();
							int skillValue=msg.readInt32();
							Program.storage.updateExperience(charId, skillId, skillValue);
							break;
						}
					case Sync.SYNC_ONLINE_STATUS:
						{
							Logger.Write(LogLevel.Debug, "received SYNC_ONLINE_STATUS");
							int charId=msg.readInt32();
							bool online=(msg.readInt8()==1);
							Program.storage.setOnlineStatus(charId, online);
							break;
						}
				}
			}

			//transaction.commit();
		}

		public static void dumpStatistics(StreamWriter os)
		{
			//for (ServerHandler::NetComputers::const_iterator
			//     i = serverHandler.clients.begin(),
			//     i_end = serverHandler.clients.end(); i != i_end; ++i)
			//{
			//    GameServer *server = static_cast< GameServer * >(*i);
			//    if (!server.port)
			//        continue;

			//    os << "<gameserver address=\"" << server.address << "\" port=\""
			//       << server.port << "\">\n";

			//    for (ServerStatistics::const_iterator j = server.maps.begin(),
			//         j_end = server.maps.end(); j != j_end; ++j)
			//    {
			//        const MapStatistics m = j.second;
			//        os << "<map id=\"" << j.first << "\" nb_things=\"" << m.nbThings
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
