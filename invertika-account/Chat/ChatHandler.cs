//
//  ChatHandler.cs
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
using invertika_account.Common;
using System.Net.Sockets;
using invertika_account.Utilities;
using ISL.Server.Network;

namespace invertika_account.Chat
{
	public class ChatHandler : ConnectionHandler
	{
		TokenCollector<ChatHandler, ChatClient, Pending> mTokenCollector;
		//friend void registerChatClient(const std::string &, const std::string &, int);

		Dictionary<string, ChatClient> mPlayerMap;
		List<PartyInvite> mInvitations;
		//std::deque<PartyInvite> mInvitations;
		Dictionary<string, int> mNumInvites;

		public ChatHandler()
		{
			// mTokenCollector(this)
		}

		bool startListen(UInt16 port, string host)
		{
			//LOG_INFO("Chat handler started:");
			//return ConnectionHandler::startListen(port, host);

			return true; //ssk;
		}

		void deletePendingClient(ChatClient c)
		{
			//MessageOut msg=new MessageOut(ManaServ.CPMSG_CONNECT_RESPONSE);
			//msg.writeInt8(ManaServ.ERRMSG_TIME_OUT);

			//// The computer will be deleted when the disconnect event is processed
			//c.disconnect(msg);
		}

		void deletePendingConnect(Pending p)
		{
			//delete p;
		}

		void tokenMatched(ChatClient client, Pending p)
		{
			//MessageOut msg(CPMSG_CONNECT_RESPONSE);

			//client->characterName = p->character;
			//client->accountLevel = p->level;

			//Character *c = storage->getCharacter(p->character);

			//if (!c)
			//{
			//    // character wasnt found
			//    msg.writeInt8(ERRMSG_FAILURE);
			//}
			//else
			//{
			//    client->characterId = c->getDatabaseID();
			//    delete p;

			//    msg.writeInt8(ERRMSG_OK);

			//    // Add chat client to player map
			//    mPlayerMap.insert(DoublePair<string, ChatClient>(client.characterName, client));
			//}

			//client->send(msg);

		}

		protected override NetComputer computerConnected(TcpClient peer)
		{
			return new ChatClient(peer);
		}

		protected override void computerDisconnected(NetComputer comp)
		{
			ChatClient computer=(ChatClient)(comp);

			if(computer.characterName==null||computer.characterName=="")
			{
				// Not yet fully logged in, remove it from pending clients.
				mTokenCollector.deletePendingClient(computer);
			}
			else
			{
				// Remove user from all channels.
				//chatChannelManager.removeUserFromAllChannels(computer);

				// Remove user from party
				removeUserFromParty(computer);

				// Remove the character from the player map
				// need to do this after removing them from party
				// as that uses the player map
				mPlayerMap.Remove(computer.characterName);
			}

			//delete computer;
		}

		protected override void processMessage(NetComputer comp, MessageIn message)
		{
			//ChatClient &computer = *static_cast< ChatClient * >(comp);
			//MessageOut result;

			//if (computer.characterName.empty())
			//{
			//    if (message.getId() != PCMSG_CONNECT) return;

			//    std::string magic_token = message.readString(MAGIC_TOKEN_LENGTH);
			//    mTokenCollector.addPendingClient(magic_token, &computer);
			//    sendGuildRejoin(computer);
			//    return;
			//}

			//switch (message.getId())
			//{
			//    case PCMSG_CHAT:
			//        handleChatMessage(computer, message);
			//        break;

			//    case PCMSG_ANNOUNCE:
			//        handleAnnounceMessage(computer, message);
			//        break;

			//    case PCMSG_PRIVMSG:
			//        handlePrivMsgMessage(computer, message);
			//        break;

			//    case PCMSG_WHO:
			//        handleWhoMessage(computer);
			//        break;

			//    case PCMSG_ENTER_CHANNEL:
			//        handleEnterChannelMessage(computer, message);
			//        break;

			//    case PCMSG_USER_MODE:
			//        handleModeChangeMessage(computer, message);
			//        break;

			//    case PCMSG_KICK_USER:
			//        handleKickUserMessage(computer, message);

			//    case PCMSG_QUIT_CHANNEL:
			//        handleQuitChannelMessage(computer, message);
			//        break;

			//    case PCMSG_LIST_CHANNELS:
			//        handleListChannelsMessage(computer, message);
			//        break;

			//    case PCMSG_LIST_CHANNELUSERS:
			//        handleListChannelUsersMessage(computer, message);
			//        break;

			//    case PCMSG_TOPIC_CHANGE:
			//        handleTopicChange(computer, message);
			//        break;

			//    case PCMSG_DISCONNECT:
			//        handleDisconnectMessage(computer, message);
			//        break;

			//    case PCMSG_GUILD_CREATE:
			//        handleGuildCreate(computer, message);
			//        break;

			//    case PCMSG_GUILD_INVITE:
			//        handleGuildInvite(computer, message);
			//        break;

			//    case PCMSG_GUILD_ACCEPT:
			//        handleGuildAcceptInvite(computer, message);
			//        break;

			//    case PCMSG_GUILD_GET_MEMBERS:
			//        handleGuildGetMembers(computer, message);
			//        break;

			//    case PCMSG_GUILD_PROMOTE_MEMBER:
			//        handleGuildMemberLevelChange(computer, message);
			//        break;

			//    case PCMSG_GUILD_KICK_MEMBER:
			//        handleGuildKickMember(computer, message);

			//    case PCMSG_GUILD_QUIT:
			//        handleGuildQuit(computer, message);
			//        break;

			//    case PCMSG_PARTY_INVITE_ANSWER:
			//        handlePartyInviteAnswer(computer, message);
			//        break;

			//    case PCMSG_PARTY_QUIT:
			//        handlePartyQuit(computer);
			//        break;

			//    default:
			//        LOG_WARN("processMessage, Invalid message type"
			//                 << message.getId());
			//        result.writeInt16(XXMSG_INVALID);
			//        break;
			//}

			//if (result.getLength() > 0)
			//    computer.send(result);
		}

		void handleCommand(ChatClient computer, string command)
		{
			//LOG_INFO("Chat: Received unhandled command: " << command);
			//MessageOut result(CPMSG_ERROR);
			//result.writeInt8(CHAT_UNHANDLED_COMMAND);
			//computer.send(result);
		}

		void warnPlayerAboutBadWords(ChatClient computer)
		{
			//// We could later count if the player is really often unpolite.
			//MessageOut result(CPMSG_ERROR);
			//result.writeInt8(CHAT_USING_BAD_WORDS); // The Channel
			//computer.send(result);

			//LOG_INFO(computer.characterName << " says bad words.");
		}

		void handleChatMessage(ChatClient client, MessageIn msg)
		{
			//std::string text = msg.readString();

			//// Pass it through the slang filter (false when it contains bad words)
			//if (!stringFilter->filterContent(text))
			//{
			//    warnPlayerAboutBadWords(client);
			//    return;
			//}

			//short channelId = msg.readInt16();
			//ChatChannel *channel = chatChannelManager->getChannel(channelId);

			//if (channel)
			//{
			//    LOG_DEBUG(client.characterName << " says in channel " << channelId
			//              << ": " << text);

			//    MessageOut result(CPMSG_PUBMSG);
			//    result.writeInt16(channelId);
			//    result.writeString(client.characterName);
			//    result.writeString(text);
			//    sendInChannel(channel, result);
			//}

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_MSG_PUBLIC;
			//trans.mMessage = "User said " + text;
			//storage->addTransaction(trans);
		}

		void handleAnnounceMessage(ChatClient client, MessageIn msg)
		{
			//std::string text = msg.readString();

			//if (!stringFilter->filterContent(text))
			//{
			//    warnPlayerAboutBadWords(client);
			//    return;
			//}

			//if (client.accountLevel == AL_ADMIN || client.accountLevel == AL_GM)
			//{
			//    // TODO: b_lindeijer: Shouldn't announcements also have a sender?
			//    LOG_INFO("ANNOUNCE: " << text);
			//    MessageOut result(CPMSG_ANNOUNCEMENT);
			//    result.writeString(text);

			//    // We send the message to all players in the default channel as it is
			//    // an announcement.
			//    sendToEveryone(result);

			//    // log transaction
			//    Transaction trans;
			//    trans.mCharacterId = client.characterId;
			//    trans.mAction = TRANS_MSG_ANNOUNCE;
			//    trans.mMessage = "User announced " + text;
			//    storage->addTransaction(trans);
			//}
			//else
			//{
			//    MessageOut result(CPMSG_ERROR);
			//    result.writeInt8(ERRMSG_INSUFFICIENT_RIGHTS);
			//    client.send(result);
			//    LOG_INFO(client.characterName <<
			//        " couldn't make an announcement due to insufficient rights.");
			//}

		}

		void handlePrivMsgMessage(ChatClient client, MessageIn msg)
		{
			//string user = msg.readString();
			//std::string text = msg.readString();

			//if (!stringFilter->filterContent(text))
			//{
			//    warnPlayerAboutBadWords(client);
			//    return;
			//}

			//// We seek the player to whom the message is told and send it to her/him.
			//sayToPlayer(client, user, text);
		}

		void handleWhoMessage(ChatClient client)
		{
			//MessageOut reply(CPMSG_WHO_RESPONSE);

			//std::map<std::string, ChatClient*>::iterator itr, itr_end;
			//itr = mPlayerMap.begin();
			//itr_end = mPlayerMap.end();

			//while (itr != itr_end)
			//{
			//    reply.writeString(itr->first);
			//    ++itr;
			//}

			//client.send(reply);
		}

		void handleEnterChannelMessage(ChatClient client, MessageIn msg)
		{
			//MessageOut reply(CPMSG_ENTER_CHANNEL_RESPONSE);

			//std::string channelName = msg.readString();
			//std::string givenPassword = msg.readString();
			//ChatChannel *channel = NULL;
			//if (chatChannelManager->channelExists(channelName) ||
			//    chatChannelManager->tryNewPublicChannel(channelName))
			//{
			//    channel = chatChannelManager->getChannel(channelName);
			//}

			//if (!channel)
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (!channel->getPassword().empty() &&
			//        channel->getPassword() != givenPassword)
			//{
			//    // Incorrect password (should probably have its own return value)
			//    reply.writeInt8(ERRMSG_INSUFFICIENT_RIGHTS);
			//}
			//else if (!channel->canJoin())
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else
			//{
			//    if (channel->addUser(&client))
			//    {
			//        reply.writeInt8(ERRMSG_OK);
			//        // The user entered the channel, now give him the channel
			//        // id, the announcement string and the user list.
			//        reply.writeInt16(channel->getId());
			//        reply.writeString(channelName);
			//        reply.writeString(channel->getAnnouncement());
			//        const ChatChannel::ChannelUsers &users = channel->getUserList();

			//        for (ChatChannel::ChannelUsers::const_iterator i = users.begin(),
			//                i_end = users.end();
			//                i != i_end; ++i)
			//        {
			//            reply.writeString((*i)->characterName);
			//            reply.writeString(channel->getUserMode((*i)));
			//        }
			//        // Send an CPMSG_UPDATE_CHANNEL to warn other clients a user went
			//        // in the channel.
			//        warnUsersAboutPlayerEventInChat(channel,
			//                client.characterName,
			//                CHAT_EVENT_NEW_PLAYER);

			//        // log transaction
			//        Transaction trans;
			//        trans.mCharacterId = client.characterId;
			//        trans.mAction = TRANS_CHANNEL_JOIN;
			//        trans.mMessage = "User joined " + channelName;
			//        storage->addTransaction(trans);
			//    }
			//    else
			//    {
			//        reply.writeInt8(ERRMSG_FAILURE);
			//    }
			//}

			//client.send(reply);
		}

		void handleModeChangeMessage(ChatClient client, MessageIn msg)
		{
			//short channelId = msg.readInt16();
			//ChatChannel *channel = chatChannelManager->getChannel(channelId);

			//if (channelId == 0 || !channel)
			//{
			//    // invalid channel
			//    return;
			//}

			//if (channel->getUserMode(&client).find('o') == std::string::npos)
			//{
			//    // invalid permissions
			//    return;
			//}

			//// get the user whos mode has been changed
			//std::string user = msg.readString();

			//// get the mode to change to
			//unsigned char mode = msg.readInt8();
			//channel->setUserMode(getClient(user), mode);

			//// set the info to pass to all channel clients
			//std::stringstream info;
			//info << client.characterName << ":" << user << ":" << mode;

			//warnUsersAboutPlayerEventInChat(channel,
			//                info.str(),
			//                CHAT_EVENT_MODE_CHANGE);

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_CHANNEL_MODE;
			//trans.mMessage = "User mode ";
			//trans.mMessage.append(mode + " set on " + user);
			//storage->addTransaction(trans);
		}

		void handleKickUserMessage(ChatClient client, MessageIn msg)
		{
			//short channelId = msg.readInt16();
			//ChatChannel *channel = chatChannelManager->getChannel(channelId);

			//if (channelId == 0 || !channel)
			//{
			//    // invalid channel
			//    return;
			//}

			//if (channel->getUserMode(&client).find('o') == std::string::npos)
			//{
			//    // invalid permissions
			//    return;
			//}

			//// get the user whos being kicked
			//std::string user = msg.readString();

			//if (channel->removeUser(getClient(user)))
			//{
			//    std::stringstream ss;
			//    ss << client.characterName << ":" << user;
			//    warnUsersAboutPlayerEventInChat(channel,
			//            ss.str(),
			//            CHAT_EVENT_KICKED_PLAYER);
			//}

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_CHANNEL_KICK;
			//trans.mMessage = "User kicked " + user;
			//storage->addTransaction(trans);
		}

		void handleQuitChannelMessage(ChatClient client, MessageIn msg)
		{
			//MessageOut reply(CPMSG_QUIT_CHANNEL_RESPONSE);

			//short channelId = msg.readInt16();
			//ChatChannel *channel = chatChannelManager->getChannel(channelId);

			//if (channelId == 0 || !channel)
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (!channel->removeUser(&client))
			//{
			//    reply.writeInt8(ERRMSG_FAILURE);
			//}
			//else
			//{
			//    reply.writeInt8(ERRMSG_OK);
			//    reply.writeInt16(channelId);

			//    // Send an CPMSG_UPDATE_CHANNEL to warn other clients a user left
			//    // the channel.
			//    warnUsersAboutPlayerEventInChat(channel,
			//            client.characterName,
			//            CHAT_EVENT_LEAVING_PLAYER);

			//    // log transaction
			//    Transaction trans;
			//    trans.mCharacterId = client.characterId;
			//    trans.mAction = TRANS_CHANNEL_QUIT;
			//    trans.mMessage = "User left " + channel->getName();
			//    storage->addTransaction(trans);

			//    if (channel->getUserList().empty())
			//    {
			//        chatChannelManager->removeChannel(channel->getId());
			//    }
			//}

			//client.send(reply);
		}

		void handleListChannelsMessage(ChatClient client, MessageIn msg)
		{
			//MessageOut reply(CPMSG_LIST_CHANNELS_RESPONSE);

			//std::list<const ChatChannel*> channels =
			//    chatChannelManager->getPublicChannels();

			//for (std::list<const ChatChannel*>::iterator i = channels.begin(),
			//        i_end = channels.end();
			//        i != i_end; ++i)
			//{
			//    reply.writeString((*i)->getName());
			//    reply.writeInt16((*i)->getUserList().size());
			//}

			//client.send(reply);

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_CHANNEL_LIST;
			//storage->addTransaction(trans);
		}

		void handleListChannelUsersMessage(ChatClient client, MessageIn msg)
		{
			//MessageOut reply(CPMSG_LIST_CHANNELUSERS_RESPONSE);

			//std::string channelName = msg.readString();
			//ChatChannel *channel = chatChannelManager->getChannel(channelName);

			//if (channel)
			//{
			//    reply.writeString(channel->getName());

			//    const ChatChannel::ChannelUsers &users = channel->getUserList();

			//    for (ChatChannel::ChannelUsers::const_iterator
			//         i = users.begin(), i_end = users.end(); i != i_end; ++i)
			//    {
			//        reply.writeString((*i)->characterName);
			//        reply.writeString(channel->getUserMode((*i)));
			//    }

			//    client.send(reply);
			//}

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_CHANNEL_USERLIST;
			//storage->addTransaction(trans);
		}

		void handleTopicChange(ChatClient client, MessageIn msg)
		{
			//short channelId = msg.readInt16();
			//std::string topic = msg.readString();
			//ChatChannel *channel = chatChannelManager->getChannel(channelId);

			//if (!guildManager->doesExist(channel->getName()))
			//{
			//    chatChannelManager->setChannelTopic(channelId, topic);
			//}
			//else
			//{
			//    guildChannelTopicChange(channel, client.characterId, topic);
			//}

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = client.characterId;
			//trans.mAction = TRANS_CHANNEL_TOPIC;
			//trans.mMessage = "User changed topic to " + topic;
			//trans.mMessage.append(" in " + channel->getName());
			//storage->addTransaction(trans);
		}

		void handleDisconnectMessage(ChatClient client, MessageIn msg)
		{
			//MessageOut reply(CPMSG_DISCONNECT_RESPONSE);
			//reply.writeInt8(ERRMSG_OK);
			//chatChannelManager->removeUserFromAllChannels(&client);
			//guildManager->disconnectPlayer(&client);
			//client.send(reply);
		}

		void sayToPlayer(ChatClient computer, string playerName, string text)
		{
			//LOG_DEBUG(computer.characterName << " says to " << playerName << ": "
			//          << text);
			//// Send it to the being if the being exists
			//MessageOut result(CPMSG_PRIVMSG);
			//result.writeString(computer.characterName);
			//result.writeString(text);
			//for (NetComputers::iterator i = clients.begin(), i_end = clients.end();
			//     i != i_end; ++i) {
			//    if (static_cast< ChatClient * >(*i)->characterName == playerName)
			//    {
			//        (*i)->send(result);
			//        break;
			//    }
			//}
		}

		void warnUsersAboutPlayerEventInChat(ChatChannel channel, string info, char eventId)
		{
			//MessageOut msg(CPMSG_CHANNEL_EVENT);
			//msg.writeInt16(channel->getId());
			//msg.writeInt8(eventId);
			//msg.writeString(info);
			//sendInChannel(channel, msg);
		}

		void sendInChannel(ChatChannel channel, MessageOut msg)
		{
			//const ChatChannel::ChannelUsers &users = channel->getUserList();

			//for (ChatChannel::ChannelUsers::const_iterator
			//     i = users.begin(), i_end = users.end(); i != i_end; ++i)
			//{
			//    (*i)->send(msg);
			//}
		}

		ChatClient getClient(string name)
		{
			//std::map<std::string, ChatClient*>::const_iterator itr
			//        = mPlayerMap.find(name);

			//if (itr != mPlayerMap.end())
			//    return itr->second;
			//else
			//    return 0;

			return null; //SSK
		}


		void removeExpiredPartyInvites()
		{
			//time_t now = time(NULL);
			//while (!mInvitations.empty() && mInvitations.front().mExpireTime < now)
			//{
			//    std::map<std::string, int>::iterator itr;
			//    itr = mNumInvites.find(mInvitations.front().mInviter);
			//    if (--itr->second <= 0)
			//        mNumInvites.erase(itr);
			//    mInvitations.pop_front();
			//}
		}

		public void handlePartyInvite(MessageIn msg)
		{
			//std::string inviterName = msg.readString();
			//std::string inviteeName = msg.readString();
			//ChatClient *inviter = getClient(inviterName);
			//ChatClient *invitee = getClient(inviteeName);

			//if (!inviter || !invitee)
			//    return;

			//removeExpiredPartyInvites();
			//const int maxInvitesPerTimeframe = 10;
			//int &num = mNumInvites[inviterName];
			//if (num >= maxInvitesPerTimeframe)
			//{
			//    MessageOut out(CPMSG_PARTY_REJECTED);
			//    out.writeString(inviterName);
			//    out.writeInt8(ERRMSG_LIMIT_REACHED);
			//    inviter->send(out);
			//    return;
			//}
			//++num;

			//if (invitee->party)
			//{
			//    MessageOut out(CPMSG_PARTY_REJECTED);
			//    out.writeString(inviterName);
			//    out.writeInt8(ERRMSG_FAILURE);
			//    inviter->send(out);
			//    return;
			//}

			//mInvitations.push_back(PartyInvite(inviterName, inviteeName));

			//MessageOut out(CPMSG_PARTY_INVITED);
			//out.writeString(inviterName);
			//invitee->send(out);
		}

		void handlePartyInviteAnswer(ChatClient client, MessageIn msg)
		{
			//if (client.party)     return;

			//MessageOut outInvitee(CPMSG_PARTY_INVITE_ANSWER_RESPONSE);

			//std::string inviter = msg.readString();

			//// check if the invite is still valid
			//bool valid = false;
			//removeExpiredPartyInvites();
			//const size_t size = mInvitations.size();
			//for (size_t i = 0; i < size; ++i)
			//{
			//    if (mInvitations[i].mInviter == inviter &&
			//        mInvitations[i].mInvitee == client.characterName)
			//    {
			//        valid = true;
			//    }
			//}

			//// the invitee did not accept the invitation
			//if (!msg.readInt8())
			//{
			//    if (!valid)
			//        return;

			//    // send rejection to inviter
			//    ChatClient *inviterClient = getClient(inviter);
			//    if (inviterClient)
			//    {
			//        MessageOut out(CPMSG_PARTY_REJECTED);
			//        out.writeString(inviter);
			//        out.writeInt8(ERRMSG_OK);
			//        inviterClient->send(out);
			//    }
			//    return;
			//}

			//// if the invitation has expired, tell the inivtee about it
			//if (!valid)
			//{
			//    outInvitee.writeInt8(ERRMSG_TIME_OUT);
			//    client.send(outInvitee);
			//    return;
			//}

			//// check that the inviter is still in the game
			//ChatClient *c1 = getClient(inviter);
			//if (!c1)
			//{
			//    outInvitee.writeInt8(ERRMSG_FAILURE);
			//    client.send(outInvitee);
			//    return;
			//}

			//// if party doesnt exist, create it
			//if (!c1->party)
			//{
			//    c1->party = new Party();
			//    c1->party->addUser(inviter);
			//    // tell game server to update info
			//    updateInfo(c1, c1->party->getId());
			//}

			//outInvitee.writeInt8(ERRMSG_OK);
			//Party::PartyUsers users = c1->party->getUsers();
			//const unsigned usersSize = users.size();
			//for (unsigned i = 0; i < usersSize; i++)
			//    outInvitee.writeString(users[i]);

			//client.send(outInvitee);

			//// add invitee to the party
			//c1->party->addUser(client.characterName, inviter);
			//client.party = c1->party;

			//// tell game server to update info
			//updateInfo(&client, client.party->getId());
		}

		void handlePartyQuit(ChatClient client)
		{
			//removeUserFromParty(client);
			//MessageOut out(CPMSG_PARTY_QUIT_RESPONSE);
			//out.writeInt8(ERRMSG_OK);
			//client.send(out);

			//// tell game server to update info
			//updateInfo(&client, 0);
		}

		void removeUserFromParty(ChatClient client)
		{
			//if (client.party)
			//{
			//    client.party->removeUser(client.characterName);
			//    informPartyMemberQuit(client);

			//    // if theres less than 1 member left, remove the party
			//    if (client.party->userCount() < 1)
			//    {
			//        delete client.party;
			//        client.party = 0;
			//    }
			//}
		}

		void informPartyMemberQuit(ChatClient client)
		{
			//std::map<std::string, ChatClient*>::iterator itr;
			//std::map<std::string, ChatClient*>::const_iterator itr_end = mPlayerMap.end();

			//for (itr = mPlayerMap.begin(); itr != itr_end; ++itr)
			//{
			//    if (itr->second->party == client.party)
			//    {
			//        MessageOut out(CPMSG_PARTY_MEMBER_LEFT);
			//        out.writeInt32(client.characterId);
			//        itr->second->send(out);
			//    }
			//}
		}
	}
}
