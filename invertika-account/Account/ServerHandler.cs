using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Utilities;
using System.Net.Sockets;
using ISL.Server.Utilities;
using ISL.Server.Network;

namespace invertika_account.Account
{
	public class ServerHandler : ConnectionHandler
	{
		//    internal GameServer getGameServerFromMap(int);
		//internal void GameServerHandler::dumpStatistics(std::ostream &);

		/**
		 * Processes server messages.
		 */
		protected override void processMessage(NetComputer computer, MessageIn message)
		{
			//         MessageOut result;
			//GameServer *server = static_cast<GameServer *>(comp);

			//switch (msg.getId())
			//{
			//    case GAMSG_REGISTER:
			//    {
			//        LOG_DEBUG("GAMSG_REGISTER");
			//        // TODO: check the credentials of the game server
			//        server->address = msg.readString();
			//        server->port = msg.readInt16();
			//        const std::string password = msg.readString();

			//        // checks the version of the remote item database with our local copy
			//        unsigned int dbversion = msg.readInt32();
			//        LOG_INFO("Game server uses itemsdatabase with version " << dbversion);

			//        LOG_DEBUG("AGMSG_REGISTER_RESPONSE");
			//        MessageOut outMsg(AGMSG_REGISTER_RESPONSE);
			//        if (dbversion == storage->getItemDatabaseVersion())
			//        {
			//            LOG_DEBUG("Item databases between account server and "
			//                "gameserver are in sync");
			//            outMsg.writeInt16(DATA_VERSION_OK);
			//        }
			//        else
			//        {
			//            LOG_DEBUG("Item database of game server has a wrong version");
			//            outMsg.writeInt16(DATA_VERSION_OUTDATED);
			//        }
			//        if (password == Configuration::getValue("net_password", "changeMe"))
			//        {
			//            outMsg.writeInt16(PASSWORD_OK);
			//            comp->send(outMsg);

			//            // transmit global world state variables
			//            std::map<std::string, std::string> variables;
			//            variables = storage->getAllWorldStateVars(0);
			//            for (std::map<std::string, std::string>::iterator i = variables.begin();
			//                 i != variables.end();
			//                 i++)
			//            {
			//                outMsg.writeString(i->first);
			//                outMsg.writeString(i->second);
			//            }
			//        }
			//        else
			//        {
			//            LOG_INFO("The password given by " << server->address << ':' << server->port << " was bad.");
			//            outMsg.writeInt16(PASSWORD_BAD);
			//            comp->disconnect(outMsg);
			//            break;
			//        }

			//        LOG_INFO("Game server " << server->address << ':' << server->port
			//                 << " wants to register " << (msg.getUnreadLength() / 2)
			//                 << " maps.");

			//        while (msg.getUnreadLength())
			//        {
			//            int id = msg.readInt16();
			//            LOG_INFO("Registering map " << id << '.');
			//            if (GameServer *s = getGameServerFromMap(id))
			//            {
			//                LOG_ERROR("Server Handler: map is already registered by "
			//                          << s->address << ':' << s->port << '.');
			//            }
			//            else
			//            {
			//                MessageOut outMsg(AGMSG_ACTIVE_MAP);

			//                // Map variables
			//                outMsg.writeInt16(id);
			//                std::map<std::string, std::string> variables;
			//                variables = storage->getAllWorldStateVars(id);

			//                 // Map vars number
			//                outMsg.writeInt16(variables.size());

			//                for (std::map<std::string, std::string>::iterator i = variables.begin();
			//                     i != variables.end();
			//                     i++)
			//                {
			//                    outMsg.writeString(i->first);
			//                    outMsg.writeString(i->second);
			//                }

			//                // Persistent Floor Items
			//                std::list<FloorItem> items;
			//                items = storage->getFloorItemsFromMap(id);

			//                outMsg.writeInt16(items.size()); //number of floor items

			//                // Send each map item: item_id, amount, pos_x, pos_y
			//                for (std::list<FloorItem>::iterator i = items.begin();
			//                     i != items.end(); ++i)
			//                {
			//                    outMsg.writeInt32(i->getItemId());
			//                    outMsg.writeInt16(i->getItemAmount());
			//                    outMsg.writeInt16(i->getPosX());
			//                    outMsg.writeInt16(i->getPosY());
			//                }

			//                comp->send(outMsg);
			//                MapStatistics &m = server->maps[id];
			//                m.nbThings = 0;
			//                m.nbMonsters = 0;
			//            }
			//        }
			//    } break;

			//    case GAMSG_PLAYER_DATA:
			//    {
			//        LOG_DEBUG("GAMSG_PLAYER_DATA");
			//        int id = msg.readInt32();
			//        if (Character *ptr = storage->getCharacter(id, NULL))
			//        {
			//            deserializeCharacterData(*ptr, msg);
			//            if (!storage->updateCharacter(ptr))
			//            {
			//                LOG_ERROR("Failed to update character "
			//                          << id << '.');
			//            }
			//            delete ptr;
			//        }
			//        else
			//        {
			//            LOG_ERROR("Received data for non-existing character "
			//                      << id << '.');
			//        }
			//    } break;

			//    case GAMSG_PLAYER_SYNC:
			//    {
			//        LOG_DEBUG("GAMSG_PLAYER_SYNC");
			//        GameServerHandler::syncDatabase(msg);
			//    } break;

			//    case GAMSG_REDIRECT:
			//    {
			//        LOG_DEBUG("GAMSG_REDIRECT");
			//        int id = msg.readInt32();
			//        std::string magic_token(utils::getMagicToken());
			//        if (Character *ptr = storage->getCharacter(id, NULL))
			//        {
			//            int mapId = ptr->getMapId();
			//            if (GameServer *s = getGameServerFromMap(mapId))
			//            {
			//                registerGameClient(s, magic_token, ptr);
			//                result.writeInt16(AGMSG_REDIRECT_RESPONSE);
			//                result.writeInt32(id);
			//                result.writeString(magic_token, MAGIC_TOKEN_LENGTH);
			//                result.writeString(s->address);
			//                result.writeInt16(s->port);
			//            }
			//            else
			//            {
			//                LOG_ERROR("Server Change: No game server for map " <<
			//                          mapId << '.');
			//            }
			//            delete ptr;
			//        }
			//        else
			//        {
			//            LOG_ERROR("Received data for non-existing character "
			//                      << id << '.');
			//        }
			//    } break;

			//    case GAMSG_PLAYER_RECONNECT:
			//    {
			//        LOG_DEBUG("GAMSG_PLAYER_RECONNECT");
			//        int id = msg.readInt32();
			//        std::string magic_token = msg.readString(MAGIC_TOKEN_LENGTH);

			//        if (Character *ptr = storage->getCharacter(id, NULL))
			//        {
			//            int accountID = ptr->getAccountID();
			//            AccountClientHandler::prepareReconnect(magic_token, accountID);
			//            delete ptr;
			//        }
			//        else
			//        {
			//            LOG_ERROR("Received data for non-existing character "
			//                      << id << '.');
			//        }
			//    } break;

			//    case GAMSG_GET_VAR_CHR:
			//    {
			//        int id = msg.readInt32();
			//        std::string name = msg.readString();
			//        std::string value = storage->getQuestVar(id, name);
			//        result.writeInt16(AGMSG_GET_VAR_CHR_RESPONSE);
			//        result.writeInt32(id);
			//        result.writeString(name);
			//        result.writeString(value);
			//    } break;

			//    case GAMSG_SET_VAR_CHR:
			//    {
			//        int id = msg.readInt32();
			//        std::string name = msg.readString();
			//        std::string value = msg.readString();
			//        storage->setQuestVar(id, name, value);
			//    } break;

			//    case GAMSG_SET_VAR_WORLD:
			//    {
			//        std::string name = msg.readString();
			//        std::string value = msg.readString();
			//        // save the new value to the database
			//        storage->setWorldStateVar(name, value);
			//        // relay the new value to all gameservers
			//        for (ServerHandler::NetComputers::iterator i = clients.begin();
			//            i != clients.end();
			//            i++)
			//        {
			//            MessageOut varUpdateMessage(AGMSG_SET_VAR_WORLD);
			//            varUpdateMessage.writeString(name);
			//            varUpdateMessage.writeString(value);
			//            (*i)->send(varUpdateMessage);
			//        }
			//    } break;

			//    case GAMSG_SET_VAR_MAP:
			//    {
			//        int mapid = msg.readInt32();
			//        std::string name = msg.readString();
			//        std::string value = msg.readString();
			//        storage->setWorldStateVar(name, mapid, value);
			//    } break;

			//    case GAMSG_BAN_PLAYER:
			//    {
			//        int id = msg.readInt32();
			//        int duration = msg.readInt32();
			//        storage->banCharacter(id, duration);
			//    } break;

			//    case GAMSG_CHANGE_PLAYER_LEVEL:
			//    {
			//        int id = msg.readInt32();
			//        int level = msg.readInt16();
			//        storage->setPlayerLevel(id, level);
			//    } break;

			//    case GAMSG_CHANGE_ACCOUNT_LEVEL:
			//    {
			//        int id = msg.readInt32();
			//        int level = msg.readInt16();

			//        // get the character so we can get the account id
			//        Character *c = storage->getCharacter(id, NULL);
			//        if (c)
			//        {
			//            storage->setAccountLevel(c->getAccountID(), level);
			//        }
			//    } break;

			//    case GAMSG_STATISTICS:
			//    {
			//        while (msg.getUnreadLength())
			//        {
			//            int mapId = msg.readInt16();
			//            ServerStatistics::iterator i = server->maps.find(mapId);
			//            if (i == server->maps.end())
			//            {
			//                LOG_ERROR("Server " << server->address << ':'
			//                          << server->port << " should not be sending stati"
			//                          "stics for map " << mapId << '.');
			//                // Skip remaining data.
			//                break;
			//            }
			//            MapStatistics &m = i->second;
			//            m.nbThings = msg.readInt16();
			//            m.nbMonsters = msg.readInt16();
			//            int nb = msg.readInt16();
			//            m.players.resize(nb);
			//            for (int j = 0; j < nb; ++j)
			//            {
			//                m.players[j] = msg.readInt32();
			//            }
			//        }
			//    } break;

			//    case GCMSG_REQUEST_POST:
			//    {
			//        // Retrieve the post for user
			//        LOG_DEBUG("GCMSG_REQUEST_POST");
			//        result.writeInt16(CGMSG_POST_RESPONSE);

			//        // get the character id
			//        int characterId = msg.readInt32();

			//        // send the character id of sender
			//        result.writeInt32(characterId);

			//        // get the character based on the id
			//        Character *ptr = storage->getCharacter(characterId, NULL);
			//        if (!ptr)
			//        {
			//            // Invalid character
			//            LOG_ERROR("Error finding character id for post");
			//            break;
			//        }

			//        // get the post for that character
			//        Post *post = postalManager->getPost(ptr);

			//        // send the post if valid
			//        if (post)
			//        {
			//            for (unsigned int i = 0; i < post->getNumberOfLetters(); ++i)
			//            {
			//                // get each letter, send the sender's name,
			//                // the contents and any attachments
			//                Letter *letter = post->getLetter(i);
			//                result.writeString(letter->getSender()->getName());
			//                result.writeString(letter->getContents());
			//                std::vector<InventoryItem> items = letter->getAttachments();
			//                for (unsigned int j = 0; j < items.size(); ++j)
			//                {
			//                    result.writeInt16(items[j].itemId);
			//                    result.writeInt16(items[j].amount);
			//                }
			//            }

			//            // clean up
			//            postalManager->clearPost(ptr);
			//        }

			//    } break;

			//    case GCMSG_STORE_POST:
			//    {
			//        // Store the letter for the user
			//        LOG_DEBUG("GCMSG_STORE_POST");
			//        result.writeInt16(CGMSG_STORE_POST_RESPONSE);

			//        // get the sender and receiver
			//        int senderId = msg.readInt32();
			//        std::string receiverName = msg.readString();

			//        // for sending it back
			//        result.writeInt32(senderId);

			//        // get their characters
			//        Character *sender = storage->getCharacter(senderId, NULL);
			//        Character *receiver = storage->getCharacter(receiverName);
			//        if (!sender || !receiver)
			//        {
			//            // Invalid character
			//            LOG_ERROR("Error finding character id for post");
			//            result.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//            break;
			//        }

			//        // get the letter contents
			//        std::string contents = msg.readString();

			//        std::vector< std::pair<int, int> > items;
			//        while (msg.getUnreadLength())
			//        {
			//            items.push_back(std::pair<int, int>(msg.readInt16(), msg.readInt16()));
			//        }

			//        // save the letter
			//        LOG_DEBUG("Creating letter");
			//        Letter *letter = new Letter(0, sender, receiver);
			//        letter->addText(contents);
			//        for (unsigned int i = 0; i < items.size(); ++i)
			//        {
			//            InventoryItem item;
			//            item.itemId = items[i].first;
			//            item.amount = items[i].second;
			//            letter->addAttachment(item);
			//        }
			//        postalManager->addLetter(letter);

			//        result.writeInt8(ERRMSG_OK);
			//    } break;

			//    case GAMSG_TRANSACTION:
			//    {
			//        LOG_DEBUG("TRANSACTION");
			//        int id = msg.readInt32();
			//        int action = msg.readInt32();
			//        std::string message = msg.readString();

			//        Transaction trans;
			//        trans.mCharacterId = id;
			//        trans.mAction = action;
			//        trans.mMessage = message;
			//        storage->addTransaction(trans);
			//    } break;

			//    case GCMSG_PARTY_INVITE:
			//        chatHandler->handlePartyInvite(msg);
			//        break;

			//    case GAMSG_CREATE_ITEM_ON_MAP:
			//    {
			//        int mapId = msg.readInt32();
			//        int itemId = msg.readInt32();
			//        int amount = msg.readInt16();
			//        int posX = msg.readInt16();
			//        int posY = msg.readInt16();

			//        LOG_DEBUG("Gameserver create item " << itemId
			//            << " on map " << mapId);

			//        storage->addFloorItem(mapId, itemId, amount, posX, posY);
			//    } break;

			//    case GAMSG_REMOVE_ITEM_ON_MAP:
			//    {
			//        int mapId = msg.readInt32();
			//        int itemId = msg.readInt32();
			//        int amount = msg.readInt16();
			//        int posX = msg.readInt16();
			//        int posY = msg.readInt16();

			//        LOG_DEBUG("Gameserver removed item " << itemId
			//            << " from map " << mapId);

			//        storage->removeFloorItem(mapId, itemId, amount, posX, posY);
			//    } break;

			//    default:
			//        LOG_WARN("ServerHandler::processMessage, Invalid message type: "
			//                 << msg.getId());
			//        result.writeInt16(XXMSG_INVALID);
			//        break;
			//}

			//// return result
			//if (result.getLength() > 0)
			//    comp->send(result);
		}

		/**
		 * Called when a game server connects. Initializes a simple NetComputer
		 * as these connections are stateless.
		 */
		protected override NetComputer computerConnected(TcpClient peer)
		{
			return new GameServer(peer);
		}

		/**
		 * Called when a game server disconnects.
		 */
		protected override void computerDisconnected(NetComputer comp)
		{
			Logger.Add(LogLevel.Information, "Game-server disconnected.");
		}
	}
}