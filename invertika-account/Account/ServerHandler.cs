//
//  ServerHandler.cs
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
using System.Net.Sockets;
using ISL.Server.Utilities;
using ISL.Server.Network;
using ISL.Server.Common;
using invertika_account.Chat;
using CSCL;
using invertika_account.Common;
using ISL.Server.Serialize;
using ISL.Server.Account;
using ISL.Server.Enums;

namespace invertika_account.Account
{
	public class ServerHandler : ConnectionHandler
	{
		//internal GameServer getGameServerFromMap(int);
		//internal void GameServerHandler::dumpStatistics(std::ostream &);

		/// <summary>
		/// Processes server messages.
		/// </summary>
		/// <param name="computer"></param>
		/// <param name="message"></param>
		protected override void processMessage(NetComputer computer, MessageIn message)
		{
			MessageOut result=new MessageOut();
			GameServer server=(GameServer)(computer);

			switch(message.getId())
			{
				case Protocol.GAMSG_REGISTER:
					{
						Logger.Write(LogLevel.Debug, "GAMSG_REGISTER");

						// TODO: check the credentials of the game server
						server.address=message.readString();
						server.port=message.readInt16();
						string password=message.readString();

						// checks the version of the remote item database with our local copy
						uint dbversion=(uint)message.readInt32();
						Logger.Write(LogLevel.Information, "Game server uses itemsdatabase with version {0}", dbversion);

						Logger.Write(LogLevel.Debug, "AGMSG_REGISTER_RESPONSE");
						MessageOut outmessage=new MessageOut(Protocol.AGMSG_REGISTER_RESPONSE);

						if(dbversion==Program.storage.getItemDatabaseVersion())
						{
							Logger.Write(LogLevel.Debug, "Item databases between account server and gameserver are in sync");
							outmessage.writeInt16((int)DataVersion.DATA_VERSION_OK);
						}
						else
						{
							Logger.Write(LogLevel.Debug, "Item database of game server has a wrong version");
							outmessage.writeInt16((int)DataVersion.DATA_VERSION_OUTDATED);
						}
						if(password==Configuration.getValue("net_password", "changeMe"))
						{
							outmessage.writeInt16((int)Password.PASSWORD_OK);
							computer.send(outmessage);

							// transmit global world state variables
							Dictionary<string, string> variables;
							variables=Program.storage.getAllWorldStateVars(0);

							foreach(KeyValuePair<string, string> pair in variables)
							{
								outmessage.writeString(pair.Key);
								outmessage.writeString(pair.Value);
							}
						}
						else
						{
							Logger.Write(LogLevel.Information, "The password given by {0}:{1} was bad.", server.address, server.port);
							outmessage.writeInt16((int)Password.PASSWORD_BAD);
							computer.disconnect(outmessage);
							break;
						}

						Logger.Write(LogLevel.Information, "Game server {0}:{1} wants to register {2}  maps.", server.address, server.port, (message.getUnreadLength()/2));

						while(message.getUnreadLength()!=0)
						{
							int id=message.readInt16();
							Logger.Write(LogLevel.Information, "Registering map {0}.", id);

							GameServer s=GameServerHandler.getGameServerFromMap(id);
							if(s!=null)
							{
								Logger.Write(LogLevel.Error, "Server Handler: map is already registered by {0}:{1}.", s.address, s.port);
							}
							else
							{
								MessageOut tmpOutMsg=new MessageOut(Protocol.AGMSG_ACTIVE_MAP);

								// Map variables
								tmpOutMsg.writeInt16(id);
								Dictionary<string, string> variables;
								variables=Program.storage.getAllWorldStateVars(id);

								// Map vars number
								tmpOutMsg.writeInt16(variables.Count);

								foreach(KeyValuePair<string, string> pair in variables)
								{
									tmpOutMsg.writeString(pair.Key);
									tmpOutMsg.writeString(pair.Value);
								}

								// Persistent Floor Items
								List<FloorItem> items=Program.storage.getFloorItemsFromMap(id);

								tmpOutMsg.writeInt16(items.Count); //number of floor items

								// Send each map item: item_id, amount, pos_x, pos_y
								foreach(FloorItem i in items)
								{
									tmpOutMsg.writeInt32(i.getItemId());
									tmpOutMsg.writeInt16(i.getItemAmount());
									tmpOutMsg.writeInt16(i.getPosX());
									tmpOutMsg.writeInt16(i.getPosY());
								}

								computer.send(tmpOutMsg);
								//MapStatistics m=server.maps[(ushort)id]; //Auskommentiert da nicht klar ist wo dieser Wert gesetzt wird
								//m.nbThings=0;
								//m.nbMonsters=0;
							}
						}
					}
					break;

				case Protocol.GAMSG_PLAYER_DATA:
					{
						Logger.Write(LogLevel.Debug, "GAMSG_PLAYER_DATA");
						int id=message.readInt32();

						try
						{
							Character ptr=Program.storage.getCharacter(id, null);

							CharacterData.deserializeCharacterData(ptr, message);
							if(!Program.storage.updateCharacter(ptr))
							{
								Logger.Write(LogLevel.Error, "Failed to update character {0}.", id);
							}
						}
						catch
						{
							Logger.Write(LogLevel.Error, "Received data for non-existing character {0}.", id);
						}
					}
					break;

				case Protocol.GAMSG_PLAYER_SYNC:
					{
						Logger.Write(LogLevel.Debug, "GAMSG_PLAYER_SYNC");
						GameServerHandler.syncDatabase(message);
					}
					break;

				case Protocol.GAMSG_REDIRECT:
					{
						Logger.Write(LogLevel.Debug, "GAMSG_REDIRECT");
						int id=message.readInt32();
						//string magic_token(utils::getMagicToken());
						string magic_token=Various.GetUniqueID();

						try
						{
							Character ptr=Program.storage.getCharacter(id, null);

							int mapId=ptr.getMapId();

							try
							{
								GameServer s=GameServerHandler.getGameServerFromMap(mapId);

								GameServerHandler.registerGameClient(s, magic_token, ptr);
								result.writeInt16((int)Protocol.AGMSG_REDIRECT_RESPONSE);
								result.writeInt32(id);
								result.writeString(magic_token);
								result.writeString(s.address);
								result.writeInt16(s.port);
							}
							catch
							{
								Logger.Write(LogLevel.Error, "Server Change: No game server for map {0}.", mapId);
							}
						}
						catch
						{
							Logger.Write(LogLevel.Error, "Received data for non-existing character {0}.", id);
						}
					}
					break;

				case Protocol.GAMSG_PLAYER_RECONNECT:
					{
						Logger.Write(LogLevel.Debug, "GAMSG_PLAYER_RECONNECT");
						int id=message.readInt32();
						string magic_token=message.readString();
						//string magic_token=message.readString(ManaServ.MAGIC_TOKEN_LENGTH);

						try
						{
							Character ptr=Program.storage.getCharacter(id, null);
							int accountID=ptr.getAccountID();
							AccountClientHandler.prepareReconnect(magic_token, accountID);
						}
						catch
						{
							Logger.Write(LogLevel.Error, "Received data for non-existing character {0}.", id);
						}
					}
					break;

				case Protocol.GAMSG_GET_VAR_CHR:
					{
						int id=message.readInt32();
						string name=message.readString();
						string value=Program.storage.getQuestVar(id, name);
						result.writeInt16((Int16)Protocol.AGMSG_GET_VAR_CHR_RESPONSE);
						result.writeInt32(id);
						result.writeString(name);
						result.writeString(value);
					}
					break;

				case Protocol.GAMSG_SET_VAR_CHR:
					{
						int id=message.readInt32();
						string name=message.readString();
						string value=message.readString();
						Program.storage.setQuestVar(id, name, value);
					}
					break;

				case Protocol.GAMSG_SET_VAR_WORLD:
					{
						string name=message.readString();
						string value=message.readString();
						// save the new value to the database
						Program.storage.setWorldStateVar(name, value);

						// relay the new value to all gameservers
						foreach(NetComputer client in clients)
						{
							MessageOut varUpdateMessage=new MessageOut(Protocol.AGMSG_SET_VAR_WORLD);
							varUpdateMessage.writeString(name);
							varUpdateMessage.writeString(value);
							client.send(varUpdateMessage);
						}
					}
					break;

				case Protocol.GAMSG_SET_VAR_MAP:
					{
						int mapid=message.readInt32();
						string name=message.readString();
						string value=message.readString();
						Program.storage.setWorldStateVar(name, mapid, value);
					}
					break;

				case Protocol.GAMSG_BAN_PLAYER:
					{
						int id=message.readInt32();
						int duration=message.readInt32();
						Program.storage.banCharacter(id, duration);
					}
					break;

				case Protocol.GAMSG_CHANGE_PLAYER_LEVEL:
					{
						int id=message.readInt32();
						int level=message.readInt16();
						Program.storage.setPlayerLevel(id, level);
					}
					break;

				case Protocol.GAMSG_CHANGE_ACCOUNT_LEVEL:
					{
						int id=message.readInt32();
						int level=message.readInt16();

						// get the character so we can get the account id
						Character c=Program.storage.getCharacter(id, null);

						if(c!=null)
						{
							Program.storage.setAccountLevel(c.getAccountID(), level);
						}
					}
					break;

				case Protocol.GAMSG_STATISTICS:
					{
						//while (message.getUnreadLength()!=0)
						//{
						//    int mapId = message.readInt16();
						//    ServerStatistics::iterator i = server.maps.find(mapId);

						//    if (i == server.maps.end())
						//    {
						//        Logger.Add(LogLevel.Error, "Server {0}:{1} should not be sending statistics for map {2}.", server.address, server.port, mapId);
						//        // Skip remaining data.
						//        break;
						//    }

						//    MapStatistics m = i.second;
						//    m.nbThings =(ushort) message.readInt16();
						//    m.nbMonsters=(ushort)message.readInt16();
						//    int nb = message.readInt16();
						//    m.players.resize(nb);
						//    for (int j = 0; j < nb; ++j)
						//    {
						//        m.players[j] = message.readInt32();
						//    }
						//}
					}
					break;

				case Protocol.GCMSG_REQUEST_POST:
					{
						// Retrieve the post for user
						Logger.Write(LogLevel.Debug, "GCMSG_REQUEST_POST");
						result.writeInt16((int)Protocol.CGMSG_POST_RESPONSE);

						// get the character id
						int characterId=message.readInt32();

						// send the character id of sender
						result.writeInt32(characterId);

						// get the character based on the id
						Character ptr=Program.storage.getCharacter(characterId, null);
						if(ptr!=null)
						{
							// Invalid character
							Logger.Write(LogLevel.Error, "Error finding character id for post");
							break;
						}

						// get the post for that character
						Post post=Program.postalManager.getPost(ptr);

						// send the post if valid
						if(post!=null)
						{
							for(int i=0;i<post.getNumberOfLetters();++i)
							{
								// get each letter, send the sender's name,
								// the contents and any attachments
								Letter letter=post.getLetter(i);
								result.writeString(letter.getSender().getName());
								result.writeString(letter.getContents());
								List<InventoryItem> items=letter.getAttachments();

								for(uint j=0;j<items.Count;++j)
								{
									result.writeInt16((int)items[(int)j].itemId);
									result.writeInt16((int)items[(int)j].amount);
								}
							}

							// clean up
							Program.postalManager.clearPost(ptr);
						}

					}
					break;

				case Protocol.GCMSG_STORE_POST:
					{
						// Store the letter for the user
						Logger.Write(LogLevel.Debug, "GCMSG_STORE_POST");
						result.writeInt16((int)Protocol.CGMSG_STORE_POST_RESPONSE);

						// get the sender and receiver
						int senderId=message.readInt32();
						string receiverName=message.readString();

						// for sending it back
						result.writeInt32(senderId);

						// get their characters
						Character sender=Program.storage.getCharacter(senderId, null);
						Character receiver=Program.storage.getCharacter(receiverName);

						if(sender!=null||receiver!=null)
						{
							// Invalid character
							Logger.Write(LogLevel.Error, "Error finding character id for post");
							result.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
							break;
						}

						// get the letter contents
						string contents=message.readString();

						List<Pair<int>> items=new List<Pair<int>>();

						while(message.getUnreadLength()!=0)
						{
							items.Add(new Pair<int>(message.readInt16(),message.readInt16()));
						}

						// save the letter
						Logger.Write(LogLevel.Debug, "Creating letter");
						Letter letter=new Letter(0,sender,receiver);
						letter.addText(contents);

						for(int i = 0;i < items.Count;++i)
						{
							InventoryItem item=new InventoryItem();
							item.itemId=(uint)items[i].First;
							item.amount=(uint)items[i].Second;
							letter.addAttachment(item);
						}

						Program.postalManager.addLetter(letter);
						
						result.writeInt8((int)ErrorMessage.ERRMSG_OK);
					}
					break;

				case Protocol.GAMSG_TRANSACTION:
					{
						Logger.Write(LogLevel.Debug, "TRANSACTION");
						int id=message.readInt32();
						int action=message.readInt32();
						string messageS=message.readString();

						Transaction trans=new Transaction();
						trans.mCharacterId=(uint)id;
						trans.mAction=(uint)action;
						trans.mMessage=messageS;
						Program.storage.addTransaction(trans);
					}
					break;

				case Protocol.GCMSG_PARTY_INVITE:
					Program.chatHandler.handlePartyInvite(message);
					break;

				case Protocol.GAMSG_CREATE_ITEM_ON_MAP:
					{
						int mapId=message.readInt32();
						int itemId=message.readInt32();
						int amount=message.readInt16();
						int posX=message.readInt16();
						int posY=message.readInt16();

						Logger.Write(LogLevel.Debug, "Gameserver create item {0} on map {1} ", itemId, mapId);

						Program.storage.addFloorItem(mapId, itemId, amount, posX, posY);
					}
					break;

				case Protocol.GAMSG_REMOVE_ITEM_ON_MAP:
					{
						int mapId=message.readInt32();
						int itemId=message.readInt32();
						int amount=message.readInt16();
						int posX=message.readInt16();
						int posY=message.readInt16();

						Logger.Write(LogLevel.Debug, "Gameserver removed item {0} from map {1}", itemId, mapId);

						Program.storage.removeFloorItem(mapId, itemId, amount, posX, posY);
					}
					break;

				default:
					{
						Logger.Write(LogLevel.Warning, "ServerHandler::processMessage, Invalid message type: {0}", message.getId());
						result.writeInt16((int)Protocol.XXMSG_INVALID);
						break;
					}
			}

			// return result
			if(result.getLength()>0)
			{
				computer.send(result);
			}
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
			Logger.Write(LogLevel.Information, "Game-server disconnected.");
		}
	}
}
