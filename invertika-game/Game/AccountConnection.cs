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
using ISL.Server.Enums;

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
			mSyncBuffer=new MessageOut();
			mSyncMessages=0;
		}

		public bool start(int gameServerPort)
		{
			string accountServerAddress=Configuration.getValue("net_accountHost", "localhost");

			// When the accountListenToGamePort is set, we use it.
			// Otherwise, we use the accountListenToClientPort + 1 if the option is set.
			// If neither, the DEFAULT_SERVER_PORT + 1 is used.
			int alternativePort=Configuration.getValue("net_accountListenToClientPort", 0)+1;
			if(alternativePort==1)
				alternativePort=Configuration.DEFAULT_SERVER_PORT+1;

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
			if(mSyncBuffer==null)
				mSyncBuffer=new MessageOut(Protocol.GAMSG_PLAYER_SYNC);

			return true;
		}

		void sendCharacterData(Character p)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_PLAYER_DATA);
			msg.writeInt32(p.getDatabaseID());
			//serializeCharacterData(p, msg); //TODO noch einbauen
			send(msg);
		}

		void processMessage(MessageIn msg)
		{
			switch(msg.getId())
			{
				case Protocol.AGMSG_REGISTER_RESPONSE:
					{
						if(msg.readInt16()!=(short)DataVersion.DATA_VERSION_OK)
						{
							Logger.Write(LogLevel.Error, "Item database is outdated! Please update to prevent inconsistencies");
							stop();   //Disconnect gracefully from account server.
							//Stop gameserver to prevent inconsistencies.
							System.Environment.Exit((int)ExitValue.EXIT_DB_EXCEPTION);
						}
						else
						{
							Logger.Write(LogLevel.Debug, "Local item database is in sync with account server.");
						}
					
						if(msg.readInt16()!=(short)Password.PASSWORD_OK)
						{
							Logger.Write(LogLevel.Error, "This game server sent a invalid password");
							stop();
							System.Environment.Exit((int)ExitValue.EXIT_BAD_CONFIG_PARAMETER);
						}

						//read world state variables
						while(msg.getUnreadLength()>0)
						{
							string key=msg.readString();
							string @value=msg.readString();
						
							if(key!=""&&@value!="")
							{
								GameState.setVariableFromDbserver(key, @value);
							}
						}

					}
					break;

				case Protocol.AGMSG_PLAYER_ENTER:
					{
						string token=msg.readString();
						Character ptr=new Character(msg);
						Program.gameHandler.addPendingCharacter(token, ptr);
					}
					break;

				case Protocol.AGMSG_ACTIVE_MAP:
					{
						int mapId=msg.readInt16();
						if(MapManager.activateMap(mapId))
						{
							// Set map variables
							MapComposite m=MapManager.getMap(mapId);
							int mapVarsNumber=msg.readInt16();
							for(int i = 0;i < mapVarsNumber;++i)
							{
								string key=msg.readString();
								string @value=msg.readString();
								if(key!=""&&@value!="")
								{
									m.setVariableFromDbserver(key, @value);
								}
							}

							//Recreate potential persistent floor items
							Logger.Write(LogLevel.Debug, "Recreate persistant items on map {0}", mapId);
							int floorItemsNumber=msg.readInt16();

							for(int i = 0;i < floorItemsNumber;i += 4)
							{
								int itemId=msg.readInt32();
								int amount=msg.readInt16();
								int posX=msg.readInt16();
								int posY=msg.readInt16();
							
								ItemClass ic=Program.itemManager.getItem(itemId);
							
								if(ic!=null)
								{
									Item item=new Item(ic,amount);
									item.setMap(m);
									Point dst=new Point(posX,posY);
									item.setPosition(dst);

									if(!GameState.insertOrDelete((Thing)item))
									{
										// The map is full.
										Logger.Write(LogLevel.Debug, "Couldn't add floor item(s) {0}  into map {1}", itemId, mapId);
										return;
									}
								}
							}
						}
					}
					break;

				case Protocol.AGMSG_SET_VAR_WORLD:
					{
						string key=msg.readString();
						string @value=msg.readString();
						GameState.setVariableFromDbserver(key, value);
						Logger.Write(LogLevel.Debug, "Global variable \"{0}\" has changed to \"{1}\"", key, @value);
					}
					break;

				case Protocol.AGMSG_REDIRECT_RESPONSE:
					{
						int id=msg.readInt32();
						string token=msg.readString();
						string address=msg.readString();
						int port=msg.readInt16();
						Program.gameHandler.completeServerChange(id, token, address, port);
					}
					break;

				case Protocol.AGMSG_GET_VAR_CHR_RESPONSE:
					{
						int id=msg.readInt32();
						string name=msg.readString();
						string @value=msg.readString();
						Quest.recoveredQuestVar(id, name, @value);
					}
					break;

				case Protocol.CGMSG_CHANGED_PARTY:
					{
						// Character DB id
						int charid=msg.readInt32();
						// Party id, 0 for none
						int partyid=msg.readInt32();
						Program.gameHandler.updateCharacter(charid, partyid);
					}
					break;

				case Protocol.CGMSG_POST_RESPONSE:
					{
						// get the character
						Character character=Program.postMan.getCharacter(msg.readInt32());

						// check character is still valid
						if(character==null)
						{
							break;
						}

						string sender=msg.readString();
						string letter=msg.readString();

						Program.postMan.gotPost(character, sender, letter);

					}
					break;

				case Protocol.CGMSG_STORE_POST_RESPONSE:
					{
						// get character
						Character character=Program.postMan.getCharacter(msg.readInt32());

						// check character is valid
						if(character==null)
						{
							break;
						}

						//TODO: Get NPC to tell character if the sending of post
						//was successful or not

					}
					break;

				default:
					{
						Logger.Write(LogLevel.Warning, "Invalid message type");
						break;
					}
			}
		}

		void playerReconnectAccount(int id, string magic_token)
		{
			Logger.Write(LogLevel.Debug, "Send GAMSG_PLAYER_RECONNECT.");
			MessageOut msg=new MessageOut(Protocol.GAMSG_PLAYER_RECONNECT);
			msg.writeInt32(id);
			msg.writeString(magic_token);
			send(msg);
		}

		void requestCharacterVar(Character ch, string name)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_GET_VAR_CHR);
			msg.writeInt32(ch.getDatabaseID());
			msg.writeString(name);
			send(msg);
		}

		void updateCharacterVar(Character ch, string name, string value)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_SET_VAR_CHR);
			msg.writeInt32(ch.getDatabaseID());
			msg.writeString(name);
			msg.writeString(value);
			send(msg);
		}

		void updateMapVar(MapComposite map, string name, string value)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_SET_VAR_MAP);
			msg.writeInt32(map.getID());
			msg.writeString(name);
			msg.writeString(value);
			send(msg);
		}

		void updateWorldVar(string name, string @value)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_SET_VAR_WORLD);
			msg.writeString(name);
			msg.writeString(@value);
			send(msg);
		}

		void banCharacter(Character ch, int duration)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_BAN_PLAYER);
			msg.writeInt32(ch.getDatabaseID());
			msg.writeInt32(duration);
			send(msg);
		}

		public void sendStatistics()  //TODO Implementieren
		{
			//MessageOut msg(GAMSG_STATISTICS);
			//const MapManager::Maps &maps = MapManager::getMaps();
			//for (MapManager::Maps::const_iterator i = maps.begin(),
			//     i_end = maps.end(); i != i_end; ++i)
			//{
			//    MapComposite *m = i.second;
			//    if (!m.isActive()) continue;
			//    msg.writeInt16(i.first);
			//    int nbThings = 0, nbMonsters = 0;
			//    typedef std::vector< Thing * > Things;
			//    const Things &things = m.getEverything();
			//    std::vector< int > players;
			//    for (Things::const_iterator j = things.begin(),
			//         j_end = things.end(); j != j_end; ++j)
			//    {
			//        Thing *t = *j;
			//        switch (t.getType())
			//        {
			//            case OBJECT_CHARACTER:
			//                players.push_back
			//                    (static_cast< Character * >(t).getDatabaseID());
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
			// send message to account server with id of sending player,
			// the id of receiving player, the letter receiver and contents, and attachments
			Logger.Write(LogLevel.Debug, "Sending GCMSG_STORE_POST.");
			MessageOut outmsg=new MessageOut(Protocol.GCMSG_STORE_POST);
			outmsg.writeInt32(c.getDatabaseID());
			outmsg.writeString(msg.readString()); // name of receiver
			outmsg.writeString(msg.readString()); // content of letter
			
			while(msg.getUnreadLength()>0) // attachments
			{
				// write the item id and amount for each attachment
				outmsg.writeInt32(msg.readInt16());
				outmsg.writeInt32(msg.readInt16());
			}
			
			send(outmsg);
		}

		void getPost(Character c)
		{
			// let the postman know to expect some post for this character
			Program.postMan.addCharacter(c);

			// send message to account server with id of retrieving player
			Logger.Write(LogLevel.Debug, "Sending GCMSG_REQUEST_POST");
			MessageOut outmsg=new MessageOut(Protocol.GCMSG_REQUEST_POST);
			outmsg.writeInt32(c.getDatabaseID());
			send(outmsg);
		}

		void changeAccountLevel(Character c, int level)
		{
			MessageOut msg=new MessageOut(Protocol.GAMSG_CHANGE_ACCOUNT_LEVEL);
			msg.writeInt32(c.getDatabaseID());
			msg.writeInt16(level);
			send(msg);
		}
		
		/**
         * Sends all changed player data to the account server to minimize
         * dataloss due to failure of one server component.
         *
         * The gameserver holds a buffer with all changes made by a character.
         * The changes are added at the time they occur. When the buffer
         * reaches one of the following limits, the buffer is sent to the
         * account server and applied to the database.
         *
         * The sync buffer is sent when:
         * - forced by any process (param force = true)
         * - every 10 seconds
         * - buffer reaches size of 1kb (SYNC_BUFFER_SIZE)
         * - buffer holds more then 20 messages (SYNC_BUFFER_LIMIT)
         *
         * @param force Send changes even if buffer hasn't reached its size
         *              or message limit. (used to send in timed schedules)
         */
		void syncChanges()
		{
			syncChanges(false);
		}

		public void syncChanges(bool force)
		{
			if(mSyncMessages==0)
				return;

			// send buffer if:
			//    a.) forced by any process
			//    b.) every 10 seconds
			//    c.) buffer reaches size of 1kb
			//    d.) buffer holds more then 20 messages
			if(force||
			    mSyncMessages>SYNC_BUFFER_LIMIT||
			    mSyncBuffer.getLength()>SYNC_BUFFER_SIZE)
			{
				Logger.Write(LogLevel.Debug, "Sending GAMSG_PLAYER_SYNC with {0} messages.", mSyncMessages);
			
				send(mSyncBuffer);

				mSyncBuffer=new MessageOut(Protocol.GAMSG_PLAYER_SYNC);
				mSyncMessages=0;
			}
			else
			{
				Logger.Write(LogLevel.Debug, "No changes to sync with account server.");
			}
		}

		void updateCharacterPoints(int charId, int charPoints, int corrPoints)
		{
			++mSyncMessages;
			mSyncBuffer.writeInt8((int)Sync.SYNC_CHARACTER_POINTS);
			mSyncBuffer.writeInt32(charId);
			mSyncBuffer.writeInt32(charPoints);
			mSyncBuffer.writeInt32(corrPoints);
			syncChanges();
		}

		void updateAttributes(int charId, int attrId, double @base, double mod)
		{
			++mSyncMessages;
			mSyncBuffer.writeInt8((int)Sync.SYNC_CHARACTER_ATTRIBUTE);
			mSyncBuffer.writeInt32(charId);
			mSyncBuffer.writeInt32(attrId);
			mSyncBuffer.writeDouble(@base);
			mSyncBuffer.writeDouble(mod);
			syncChanges();
		}

		void updateExperience(int charId, int skillId, int skillValue)
		{
			++mSyncMessages;
			mSyncBuffer.writeInt8((int)Sync.SYNC_CHARACTER_SKILL);
			mSyncBuffer.writeInt32(charId);
			mSyncBuffer.writeInt8(skillId);
			mSyncBuffer.writeInt32(skillValue);
			syncChanges();
		}

		void updateOnlineStatus(int charId, bool online)
		{
			++mSyncMessages;
			mSyncBuffer.writeInt8((int)Sync.SYNC_ONLINE_STATUS);
			mSyncBuffer.writeInt32(charId);
			mSyncBuffer.writeInt8(online?1:0);
			syncChanges();
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
