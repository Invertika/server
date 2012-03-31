//
//  AccountConnection.cs
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
using ISL.Server.Network;
using invertika_game.Network;
using ISL.Server.Common;
using ISL.Server.Utilities;

namespace invertika_game.Game
{
	public class AccountConnection : Connection
	{
		MessageOut mSyncBuffer;     /**< Message buffer to store sync data. */
		int mSyncMessages;           /**< Number of messages in the sync buffer. */

		/** Maximum size of sync buffer in bytes. */
		const uint SYNC_BUFFER_SIZE=1024;

		/** Maximum number of messages in sync buffer. */
		const int SYNC_BUFFER_LIMIT=20;

		public AccountConnection()
		{
			// mSyncBuffer(0),
			//mSyncMessages(0)
		}

		~AccountConnection()
		{
			//delete mSyncBuffer;
		}

		public bool start(int gameServerPort)
		{
			string accountServerAddress=Configuration.getValue("net_accountHost", "localhost");

			// When the accountListenToGamePort is set, we use it.
			// Otherwise, we use the accountListenToClientPort + 1 if the option is set.
			// If neither, the DEFAULT_SERVER_PORT + 1 is used.
			int alternativePort=Configuration.getValue("net_accountListenToClientPort", 0)+1;
			if(alternativePort==1) alternativePort=Configuration.DEFAULT_SERVER_PORT+1;

			int accountServerPort=Configuration.getValue("net_accountListenToGamePort", alternativePort);

			if(!start(accountServerAddress, accountServerPort))
			{
				Logger.Write(LogLevel.Information, "Unable to create a connection to an account server.");
				return false;
			}

			Logger.Write(LogLevel.Information, "Connection established to the account server.");


			string gameServerAddress=Configuration.getValue("net_gameHost", "localhost");
			string password=Configuration.getValue("net_password", "changeMe");

			// Register with the account server and send the list of maps we handle
			MessageOut msg=new MessageOut(Protocol.GAMSG_REGISTER);
			msg.writeString(gameServerAddress);
			msg.writeInt16(gameServerPort);
			msg.writeString(password);
			msg.writeInt32((int)Program.itemManager.getDatabaseVersion());

			Dictionary<int, MapComposite> m=MapManager.getMaps();

			foreach(int map in m.Keys)
			{
				msg.writeInt16(map);
			}

			send(msg);

			// initialize sync buffer
			if(mSyncBuffer==null) mSyncBuffer=new MessageOut(Protocol.GAMSG_PLAYER_SYNC);

			return true; //ssk
		}

		void sendCharacterData(Character p)
		{
			//MessageOut msg(GAMSG_PLAYER_DATA);
			//msg.writeInt32(p->getDatabaseID());
			//serializeCharacterData(*p, msg);
			//send(msg);
		}

		void processMessage(MessageIn msg)
		{
			//switch (msg.getId())
			//{
			//    case AGMSG_REGISTER_RESPONSE:
			//    {
			//        if (msg.readInt16() != DATA_VERSION_OK)
			//        {
			//            LOG_ERROR("Item database is outdated! Please update to "
			//                      "prevent inconsistencies");
			//            stop();  // Disconnect gracefully from account server.
			//            // Stop gameserver to prevent inconsistencies.
			//            exit(EXIT_DB_EXCEPTION);
			//        }
			//        else
			//        {
			//            LOG_DEBUG("Local item database is "
			//                      "in sync with account server.");
			//        }
			//        if (msg.readInt16() != PASSWORD_OK)
			//        {
			//            LOG_ERROR("This game server sent a invalid password");
			//            stop();
			//            exit(EXIT_BAD_CONFIG_PARAMETER);
			//        }

			//        // read world state variables
			//        while (msg.getUnreadLength())
			//        {
			//            std::string key = msg.readString();
			//            std::string value = msg.readString();
			//            if (!key.empty() && !value.empty())
			//            {
			//                GameState::setVariableFromDbserver(key, value);
			//            }
			//        }

			//    } break;

			//    case AGMSG_PLAYER_ENTER:
			//    {
			//        std::string token = msg.readString(MAGIC_TOKEN_LENGTH);
			//        Character *ptr = new Character(msg);
			//        gameHandler->addPendingCharacter(token, ptr);
			//    } break;

			//    case AGMSG_ACTIVE_MAP:
			//    {
			//        int mapId = msg.readInt16();
			//        if (MapManager::activateMap(mapId))
			//        {
			//            // Set map variables
			//            MapComposite *m = MapManager::getMap(mapId);
			//            int mapVarsNumber = msg.readInt16();
			//            for(int i = 0; i < mapVarsNumber; ++i)
			//            {
			//                std::string key = msg.readString();
			//                std::string value = msg.readString();
			//                if (!key.empty() && !value.empty())
			//                    m->setVariableFromDbserver(key, value);
			//            }

			//            // Recreate potential persistent floor items
			//            LOG_DEBUG("Recreate persistant items on map " << mapId);
			//            int floorItemsNumber = msg.readInt16();

			//            for(int i = 0; i < floorItemsNumber; i += 4)
			//            {
			//                int itemId = msg.readInt32();
			//                int amount = msg.readInt16();
			//                int posX = msg.readInt16();
			//                int posY = msg.readInt16();

			//                if (ItemClass *ic = itemManager->getItem(itemId))
			//                {
			//                    Item *item = new Item(ic, amount);
			//                    item->setMap(m);
			//                    Point dst(posX, posY);
			//                    item->setPosition(dst);

			//                    if (!GameState::insertOrDelete(item))
			//                    {
			//                        // The map is full.
			//                        LOG_WARN("Couldn't add floor item(s) " << itemId
			//                            << " into map " << mapId);
			//                        return;
			//                    }
			//                }
			//            }
			//        }
			//    } break;

			//    case AGMSG_SET_VAR_WORLD:
			//    {
			//        std::string key = msg.readString();
			//        std::string value = msg.readString();
			//        GameState::setVariableFromDbserver(key, value);
			//        LOG_DEBUG("Global variable \"" << key << "\" has changed to \""
			//                  << value << "\"");
			//    } break;

			//    case AGMSG_REDIRECT_RESPONSE:
			//    {
			//        int id = msg.readInt32();
			//        std::string token = msg.readString(MAGIC_TOKEN_LENGTH);
			//        std::string address = msg.readString();
			//        int port = msg.readInt16();
			//        gameHandler->completeServerChange(id, token, address, port);
			//    } break;

			//    case AGMSG_GET_VAR_CHR_RESPONSE:
			//    {
			//        int id = msg.readInt32();
			//        std::string name = msg.readString();
			//        std::string value = msg.readString();
			//        recoveredQuestVar(id, name, value);
			//    } break;

			//    case CGMSG_CHANGED_PARTY:
			//    {
			//        // Character DB id
			//        int charid = msg.readInt32();
			//        // Party id, 0 for none
			//        int partyid = msg.readInt32();
			//        gameHandler->updateCharacter(charid, partyid);
			//    } break;

			//    case CGMSG_POST_RESPONSE:
			//    {
			//        // get the character
			//        Character *character = postMan->getCharacter(msg.readInt32());

			//        // check character is still valid
			//        if (!character)
			//        {
			//            break;
			//        }

			//        std::string sender = msg.readString();
			//        std::string letter = msg.readString();

			//        postMan->gotPost(character, sender, letter);

			//    } break;

			//    case CGMSG_STORE_POST_RESPONSE:
			//    {
			//        // get character
			//        Character *character = postMan->getCharacter(msg.readInt32());

			//        // check character is valid
			//        if (!character)
			//        {
			//            break;
			//        }

			//        // TODO: Get NPC to tell character if the sending of post
			//        // was successful or not

			//    } break;

			//    default:
			//        LOG_WARN("Invalid message type");
			//        break;
			//}
		}

		void playerReconnectAccount(int id, string magic_token)
		{
			//LOG_DEBUG("Send GAMSG_PLAYER_RECONNECT.");
			//MessageOut msg(GAMSG_PLAYER_RECONNECT);
			//msg.writeInt32(id);
			//msg.writeString(magic_token, MAGIC_TOKEN_LENGTH);
			//send(msg);
		}

		void requestCharacterVar(Character ch, string name)
		{
			//MessageOut msg(GAMSG_GET_VAR_CHR);
			//msg.writeInt32(ch->getDatabaseID());
			//msg.writeString(name);
			//send(msg);
		}

		void updateCharacterVar(Character ch, string name, string value)
		{
			//MessageOut msg(GAMSG_SET_VAR_CHR);
			//msg.writeInt32(ch->getDatabaseID());
			//msg.writeString(name);
			//msg.writeString(value);
			//send(msg);
		}

		void updateMapVar(MapComposite map, string name, string value)
		{
			//MessageOut msg(GAMSG_SET_VAR_MAP);
			//msg.writeInt32(map->getID());
			//msg.writeString(name);
			//msg.writeString(value);
			//send(msg);
		}

		void updateWorldVar(string name, string value)
		{
			//MessageOut msg(GAMSG_SET_VAR_WORLD);
			//msg.writeString(name);
			//msg.writeString(value);
			//send(msg);
		}

		void banCharacter(Character ch, int duration)
		{
			//MessageOut msg(GAMSG_BAN_PLAYER);
			//msg.writeInt32(ch->getDatabaseID());
			//msg.writeInt32(duration);
			//send(msg);
		}

		public void sendStatistics()
		{
			//MessageOut msg(GAMSG_STATISTICS);
			//const MapManager::Maps &maps = MapManager::getMaps();
			//for (MapManager::Maps::const_iterator i = maps.begin(),
			//     i_end = maps.end(); i != i_end; ++i)
			//{
			//    MapComposite *m = i->second;
			//    if (!m->isActive()) continue;
			//    msg.writeInt16(i->first);
			//    int nbThings = 0, nbMonsters = 0;
			//    typedef std::vector< Thing * > Things;
			//    const Things &things = m->getEverything();
			//    std::vector< int > players;
			//    for (Things::const_iterator j = things.begin(),
			//         j_end = things.end(); j != j_end; ++j)
			//    {
			//        Thing *t = *j;
			//        switch (t->getType())
			//        {
			//            case OBJECT_CHARACTER:
			//                players.push_back
			//                    (static_cast< Character * >(t)->getDatabaseID());
			//                break;
			//            case OBJECT_MONSTER:
			//                ++nbMonsters;
			//                break;
			//            default:
			//                ++nbThings;
			//        }
			//    }
			//    msg.writeInt16(nbThings);
			//    msg.writeInt16(nbMonsters);
			//    msg.writeInt16(players.size());
			//    for (std::vector< int >::const_iterator j = players.begin(),
			//         j_end = players.end(); j != j_end; ++j)
			//    {
			//        msg.writeInt32(*j);
			//    }
			//}
			//send(msg);
		}

		void sendPost(Character c, MessageIn msg)
		{
			//// send message to account server with id of sending player,
			//// the id of receiving player, the letter receiver and contents, and attachments
			//LOG_DEBUG("Sending GCMSG_STORE_POST.");
			//MessageOut out(GCMSG_STORE_POST);
			//out.writeInt32(c->getDatabaseID());
			//out.writeString(msg.readString()); // name of receiver
			//out.writeString(msg.readString()); // content of letter
			//while (msg.getUnreadLength()) // attachments
			//{
			//    // write the item id and amount for each attachment
			//    out.writeInt32(msg.readInt16());
			//    out.writeInt32(msg.readInt16());
			//}
			//send(out);
		}

		void getPost(Character c)
		{
			//// let the postman know to expect some post for this character
			//postMan->addCharacter(c);

			//// send message to account server with id of retrieving player
			//LOG_DEBUG("Sending GCMSG_REQUEST_POST");
			//MessageOut out(GCMSG_REQUEST_POST);
			//out.writeInt32(c->getDatabaseID());
			//send(out);
		}

		void changeAccountLevel(Character c, int level)
		{
			//MessageOut msg(GAMSG_CHANGE_ACCOUNT_LEVEL);
			//msg.writeInt32(c->getDatabaseID());
			//msg.writeInt16(level);
			//send(msg);
		}

		public void syncChanges(bool force)
		{
			//if (mSyncMessages == 0)
			//    return;

			//// send buffer if:
			////    a.) forced by any process
			////    b.) every 10 seconds
			////    c.) buffer reaches size of 1kb
			////    d.) buffer holds more then 20 messages
			//if (force ||
			//    mSyncMessages > SYNC_BUFFER_LIMIT ||
			//    mSyncBuffer->getLength() > SYNC_BUFFER_SIZE )
			//{
			//    LOG_DEBUG("Sending GAMSG_PLAYER_SYNC with "
			//            << mSyncMessages << " messages." );

			//    send(*mSyncBuffer);
			//    delete mSyncBuffer;

			//    mSyncBuffer = new MessageOut(GAMSG_PLAYER_SYNC);
			//    mSyncMessages = 0;
			//}
			//else
			//{
			//    LOG_DEBUG("No changes to sync with account server.");
			//}
		}

		void updateCharacterPoints(int charId, int charPoints, int corrPoints)
		{
			//++mSyncMessages;
			//mSyncBuffer->writeInt8(SYNC_CHARACTER_POINTS);
			//mSyncBuffer->writeInt32(charId);
			//mSyncBuffer->writeInt32(charPoints);
			//mSyncBuffer->writeInt32(corrPoints);
			//syncChanges();
		}

		void updateAttributes(int charId, int attrId, double @base, double mod)
		{
			//++mSyncMessages;
			//mSyncBuffer->writeInt8(SYNC_CHARACTER_ATTRIBUTE);
			//mSyncBuffer->writeInt32(charId);
			//mSyncBuffer->writeInt32(attrId);
			//mSyncBuffer->writeDouble(base);
			//mSyncBuffer->writeDouble(mod);
			//syncChanges();
		}

		void updateExperience(int charId, int skillId, int skillValue)
		{
			//++mSyncMessages;
			//mSyncBuffer->writeInt8(SYNC_CHARACTER_SKILL);
			//mSyncBuffer->writeInt32(charId);
			//mSyncBuffer->writeInt8(skillId);
			//mSyncBuffer->writeInt32(skillValue);
			//syncChanges();
		}

		void updateOnlineStatus(int charId, bool online)
		{
			//++mSyncMessages;
			//mSyncBuffer->writeInt8(SYNC_ONLINE_STATUS);
			//mSyncBuffer->writeInt32(charId);
			//mSyncBuffer->writeInt8(online ? 1 : 0);
			//syncChanges();
		}

		void sendTransaction(int id, int action, string message)
		{
			//MessageOut msg(GAMSG_TRANSACTION);
			//msg.writeInt32(id);
			//msg.writeInt32(action);
			//msg.writeString(message);
			//send(msg);
		}

		void createFloorItems(int mapId, int itemId, int amount, int posX, int posY)
		{
			//MessageOut msg(GAMSG_CREATE_ITEM_ON_MAP);
			//msg.writeInt32(mapId);
			//msg.writeInt32(itemId);
			//msg.writeInt16(amount);
			//msg.writeInt16(posX);
			//msg.writeInt16(posY);
			//send(msg);
		}

		void removeFloorItems(int mapId, int itemId, int amount, int posX, int posY)
		{
			//MessageOut msg(GAMSG_REMOVE_ITEM_ON_MAP);
			//msg.writeInt32(mapId);
			//msg.writeInt32(itemId);
			//msg.writeInt16(amount);
			//msg.writeInt16(posX);
			//msg.writeInt16(posY);
			//send(msg);
		}
	}
}
