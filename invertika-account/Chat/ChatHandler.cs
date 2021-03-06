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
using System.Net.Sockets;
using invertika_account.Utilities;
using ISL.Server.Network;
using ISL.Server.Utilities;
using ISL.Server.Common;
using ISL.Server.Account;
using invertika_account.Common;
using invertika_account.Account;
using ISL.Server.Enums;
using ISL.Server;

namespace invertika_account.Chat
{
    public class ChatHandler : ConnectionHandler, ITokenCollectorHandler
    {
        #region ITokenCollectorHandler implementation
        public void deletePendingClient(NetComputer client)
        {
            MessageOut msg=new MessageOut(Protocol.CPMSG_CONNECT_RESPONSE);
            msg.writeInt8((int)ErrorMessage.ERRMSG_TIME_OUT);
            
            // The computer will be deleted when the disconnect event is processed
            client.disconnect(msg);
        }

        public void deletePendingConnect(object data)
        {
            //Pending
//            void ChatHandler::deletePendingConnect(Pending *p)
//            {
//                delete p;
//            }
        }

        public void tokenMatched(NetComputer computer, object data)
        {
            ChatClient client=(ChatClient)computer;
            Pending p=(Pending)data;

            MessageOut msg=new MessageOut(Protocol.CPMSG_CONNECT_RESPONSE);
            
            client.characterName=p.character;
            client.accountLevel=p.level;

            Character c=Program.storage.getCharacter(p.character);

            if(c==null)
            {
                // character wasnt found
                msg.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }
            else
            {
                client.characterId=(uint)c.getDatabaseID();
                //delete p;

                msg.writeInt8((int)ErrorMessage.ERRMSG_OK);
                
                // Add chat client to player map
                mPlayerMap.Add(client.characterName, client);
            }
            
            client.send(msg);
        }
        #endregion

        TokenCollector mTokenCollector;
        //friend void registerChatClient(const std::string &, const std::string &, int);

        Dictionary<string, ChatClient> mPlayerMap;
        List<PartyInvite> mInvitations;
        //std::deque<PartyInvite> mInvitations;
        Dictionary<string, int> mNumInvites;

        public static void registerChatClient(string token, string name, int level)
        {
            Pending p=new Pending();
            p.character=name;
            p.level=(byte)level;
            Program.chatHandler.mTokenCollector.addPendingConnect(token, p);
        }

        public ChatHandler()
        {
            mTokenCollector=new TokenCollector(this);
            // mTokenCollector(this)
        }

        new bool startListen(UInt16 port, string host)
        {
            Logger.Write(LogLevel.Information, "Chat handler started:");
            return base.startListen(port, host);
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
                Program.chatChannelManager.removeUserFromAllChannels(computer);

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
            ChatClient computer=(ChatClient)(comp);
            MessageOut result=new MessageOut();

            if(computer.characterName==null)
            {
                if(message.getId()!=Protocol.PCMSG_CONNECT)
                    return;

                string magic_token=message.readString();
                mTokenCollector.addPendingClient(magic_token, computer);
                sendGuildRejoin(computer);
                return;
            }

            switch(message.getId())
            {
                case Protocol.PCMSG_CHAT:
                    handleChatMessage(computer, message);
                    break;

                case Protocol.PCMSG_ANNOUNCE:
                    handleAnnounceMessage(computer, message);
                    break;

                case Protocol.PCMSG_PRIVMSG:
                    handlePrivMsgMessage(computer, message);
                    break;

                case Protocol.PCMSG_WHO:
                    handleWhoMessage(computer);
                    break;

                case Protocol.PCMSG_ENTER_CHANNEL:
                    handleEnterChannelMessage(computer, message);
                    break;

                case Protocol.PCMSG_USER_MODE:
                    handleModeChangeMessage(computer, message);
                    break;

                case Protocol.PCMSG_KICK_USER:
                    handleKickUserMessage(computer, message);
                    break; //TODO hinzugef�gt, evt sollte es durchfallen

                case Protocol.PCMSG_QUIT_CHANNEL:
                    handleQuitChannelMessage(computer, message);
                    break;

                case Protocol.PCMSG_LIST_CHANNELS:
                    handleListChannelsMessage(computer, message);
                    break;

                case Protocol.PCMSG_LIST_CHANNELUSERS:
                    handleListChannelUsersMessage(computer, message);
                    break;

                case Protocol.PCMSG_TOPIC_CHANGE:
                    handleTopicChange(computer, message);
                    break;

                case Protocol.PCMSG_DISCONNECT:
                    handleDisconnectMessage(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_CREATE:
                    handleGuildCreate(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_INVITE:
                    handleGuildInvite(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_ACCEPT:
                    handleGuildAcceptInvite(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_GET_MEMBERS:
                    handleGuildGetMembers(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_PROMOTE_MEMBER:
                    handleGuildMemberLevelChange(computer, message);
                    break;

                case Protocol.PCMSG_GUILD_KICK_MEMBER:
                    handleGuildKickMember(computer, message);
                    break; //TODO hinzugef�gt, evt sollte es durchfallen

                case Protocol.PCMSG_GUILD_QUIT:
                    handleGuildQuit(computer, message);
                    break;

                case Protocol.PCMSG_PARTY_INVITE_ANSWER:
                    handlePartyInviteAnswer(computer, message);
                    break;

                case Protocol.PCMSG_PARTY_QUIT:
                    handlePartyQuit(computer);
                    break;

                default:
                    Logger.Write(LogLevel.Warning, "processMessage, Invalid message type {0}", message.getId());
                    result.writeInt16((int)Protocol.XXMSG_INVALID);
                    break;
            }

            if(result.getLength()>0)
                computer.send(result);
        }

        void handleCommand(ChatClient computer, string command)
        {
            Logger.Write(LogLevel.Information, "Chat: Received unhandled command:  {0}", command);
            MessageOut result=new MessageOut(Protocol.CPMSG_ERROR);
            result.writeInt8((int)ChatValues.CHAT_UNHANDLED_COMMAND); //TODO sollte im Protocol landen
            computer.send(result);
        }

        void warnPlayerAboutBadWords(ChatClient computer)
        {
            // We could later count if the player is really often unpolite.
            MessageOut result=new MessageOut(Protocol.CPMSG_ERROR);
            result.writeInt8((int)ChatValues.CHAT_USING_BAD_WORDS); // The Channel //TODO sollte im Protocol landen
            computer.send(result);

            Logger.Write(LogLevel.Information, "{0} says bad words.", computer.characterName);
        }

        void handleChatMessage(ChatClient client, MessageIn msg)
        {
            string text=msg.readString();

            // Pass it through the slang filter (false when it contains bad words)
            if(!Program.stringFilter.filterContent(text))
            {
                warnPlayerAboutBadWords(client);
                return;
            }

            short channelId=msg.readInt16();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelId);

            if(channel!=null)
            {
                Logger.Write(LogLevel.Debug, "{0} says in channel {1}: {2}", client.characterName, channelId, text);

                MessageOut result=new MessageOut(Protocol.CPMSG_PUBMSG);
                result.writeInt16(channelId);
                result.writeString(client.characterName);
                result.writeString(text);
                sendInChannel(channel, result);
            }

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_MSG_PUBLIC;
            trans.mMessage="User said "+text;
            Program.storage.addTransaction(trans);
        }

        void handleAnnounceMessage(ChatClient client, MessageIn msg)
        {
            string text=msg.readString();

            if(!Program.stringFilter.filterContent(text))
            {
                warnPlayerAboutBadWords(client);
                return;
            }

            if(client.accountLevel==(byte)AccessLevel.AL_ADMIN||client.accountLevel==(byte)AccessLevel.AL_GM)
            {
                // TODO: b_lindeijer: Shouldn't announcements also have a sender?
                Logger.Write(LogLevel.Information, "ANNOUNCE: {0}", text);
                MessageOut result=new MessageOut(Protocol.CPMSG_ANNOUNCEMENT);
                result.writeString(text);

                // We send the message to all players in the default channel as it is
                // an announcement.
                sendToEveryone(result);

                // log transaction
                Transaction trans=new Transaction();
                trans.mCharacterId=client.characterId;
                trans.mAction=(uint)TransactionMembers.TRANS_MSG_ANNOUNCE;
                trans.mMessage="User announced "+text;
                Program.storage.addTransaction(trans);
            }
            else
            {
                MessageOut result=new MessageOut(Protocol.CPMSG_ERROR);
                result.writeInt8((int)ErrorMessage.ERRMSG_INSUFFICIENT_RIGHTS);
                client.send(result);
                Logger.Write(LogLevel.Information, "{0} couldn't make an announcement due to insufficient rights.", client.characterName);
            }
        }

        void handlePrivMsgMessage(ChatClient client, MessageIn msg)
        {
            string user=msg.readString();
            string text=msg.readString();

            if(!Program.stringFilter.filterContent(text))
            {
                warnPlayerAboutBadWords(client);
                return;
            }

            // We seek the player to whom the message is told and send it to her/him.
            sayToPlayer(client, user, text);
        }

        void handleWhoMessage(ChatClient client)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_WHO_RESPONSE);

            foreach(string id in mPlayerMap.Keys)
            {
                reply.writeString(id);
            }

            client.send(reply);
        }

        void handleEnterChannelMessage(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_ENTER_CHANNEL_RESPONSE);

            string channelName=msg.readString();
            string givenPassword=msg.readString();
            ChatChannel channel=null;

            if(Program.chatChannelManager.channelExists(channelName)||
                Program.chatChannelManager.tryNewPublicChannel(channelName))
            {
                channel=Program.chatChannelManager.getChannel(channelName);
            }

            if(channel!=null)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else if(channel.getPassword()!=null&&channel.getPassword()!=givenPassword)
            {
                // Incorrect password (should probably have its own return value)
                reply.writeInt8((int)ErrorMessage.ERRMSG_INSUFFICIENT_RIGHTS);
            }
            else if(!channel.canJoin())
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else
            {
                if(channel.addUser(client))
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    // The user entered the channel, now give him the channel
                    // id, the announcement string and the user list.
                    reply.writeInt16(channel.getId());
                    reply.writeString(channelName);
                    reply.writeString(channel.getAnnouncement());
                    List<ChatClient> users=channel.getUserList();

                    foreach(ChatClient user in users)
                    {
                        reply.writeString(user.characterName);
                        reply.writeString(channel.getUserMode(user));
                    }

                    // Send an CPMSG_UPDATE_CHANNEL to warn other clients a user went
                    // in the channel.
                    warnUsersAboutPlayerEventInChat(channel, client.characterName, (int)ChatValues.CHAT_EVENT_NEW_PLAYER);

                    // log transaction
                    Transaction trans=new Transaction();
                    trans.mCharacterId=client.characterId;
                    trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_JOIN;
                    trans.mMessage="User joined "+channelName;
                    Program.storage.addTransaction(trans);
                }
                else
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                }
            }

            client.send(reply);
        }

        void handleModeChangeMessage(ChatClient client, MessageIn msg)
        {
            short channelId=msg.readInt16();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelId);

            if(channelId==0||channel!=null)
            {
                // invalid channel
                return;
            }

            if(channel.getUserMode(client).IndexOf('o')==-1)
            {
                // invalid permissions
                return;
            }

            // get the user whos mode has been changed
            string user=msg.readString();

            // get the mode to change to
            byte mode=msg.readInt8();
            channel.setUserMode(getClient(user), mode);

            // set the info to pass to all channel clients
            string info=client.characterName+":"+user+":"+mode;

            warnUsersAboutPlayerEventInChat(channel, info, (int)ChatValues.CHAT_EVENT_MODE_CHANGE);

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_MODE;
            trans.mMessage="User mode ";
            trans.mMessage+=mode+" set on "+user;
            Program.storage.addTransaction(trans);
        }

        void handleKickUserMessage(ChatClient client, MessageIn msg)
        {
            short channelId=msg.readInt16();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelId);

            if(channelId==0||channel!=null)
            {
                // invalid channel
                return;
            }

            if(channel.getUserMode(client).IndexOf('o')==-1)
            {
                // invalid permissions
                return;
            }

            // get the user whos being kicked
            string user=msg.readString();

            if(channel.removeUser(getClient(user)))
            {
                string ss=client.characterName+":"+user;
                warnUsersAboutPlayerEventInChat(channel, ss, (int)ChatValues.CHAT_EVENT_KICKED_PLAYER);
            }

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_KICK;
            trans.mMessage="User kicked "+user;
            Program.storage.addTransaction(trans);
        }

        void handleQuitChannelMessage(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_QUIT_CHANNEL_RESPONSE);

            short channelId=msg.readInt16();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelId);

            if(channelId==0||channel!=null)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else if(!channel.removeUser(client))
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                reply.writeInt16(channelId);

                // Send an CPMSG_UPDATE_CHANNEL to warn other clients a user left
                // the channel.
                warnUsersAboutPlayerEventInChat(channel, client.characterName, (int)ChatValues.CHAT_EVENT_LEAVING_PLAYER);

                // log transaction
                Transaction trans=new Transaction();
                trans.mCharacterId=client.characterId;
                trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_QUIT;
                trans.mMessage="User left "+channel.getName();
                Program.storage.addTransaction(trans);

                if(channel.getUserList()!=null)
                {
                    Program.chatChannelManager.removeChannel(channel.getId());
                }
            }

            client.send(reply);
        }

        void handleListChannelsMessage(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_LIST_CHANNELS_RESPONSE);

            List<ChatChannel> channels=Program.chatChannelManager.getPublicChannels();

            foreach(ChatChannel channel in channels)
            {
                reply.writeString(channel.getName());
                reply.writeInt16(channel.getUserList().Count);
            }

            client.send(reply);

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_LIST;
            Program.storage.addTransaction(trans);
        }

        void handleListChannelUsersMessage(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_LIST_CHANNELUSERS_RESPONSE);

            string channelName=msg.readString();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelName);

            if(channel!=null)
            {
                reply.writeString(channel.getName());

                List<ChatClient> users=channel.getUserList();

                foreach(ChatClient user in users)
                {
                    reply.writeString(user.characterName);
                    reply.writeString(channel.getUserMode(user));
                }

                client.send(reply);
            }

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_USERLIST;
            Program.storage.addTransaction(trans);
        }

        void handleTopicChange(ChatClient client, MessageIn msg)
        {
            short channelId=msg.readInt16();
            string topic=msg.readString();
            ChatChannel channel=Program.chatChannelManager.getChannel(channelId);

            if(!Program.guildManager.doesExist(channel.getName()))
            {
                Program.chatChannelManager.setChannelTopic(channelId, topic);
            }
            else
            {
                guildChannelTopicChange(channel, (int)client.characterId, topic);
            }

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=client.characterId;
            trans.mAction=(uint)TransactionMembers.TRANS_CHANNEL_TOPIC;
            trans.mMessage="User changed topic to "+topic;
            trans.mMessage+=(" in "+channel.getName());
            Program.storage.addTransaction(trans);
        }

        void handleDisconnectMessage(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_DISCONNECT_RESPONSE);
            reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
            Program.chatChannelManager.removeUserFromAllChannels(client);
            Program.guildManager.disconnectPlayer(client);
            client.send(reply);
        }

        void sayToPlayer(ChatClient computer, string playerName, string text)
        {
            Logger.Write(LogLevel.Debug, computer.characterName+" says to "+playerName+": "+text);

            // Send it to the being if the being exists
            MessageOut result=new MessageOut(Protocol.CPMSG_PRIVMSG);
            result.writeString(computer.characterName);
            result.writeString(text);

            foreach(NetComputer client in clients)
            {
                if(client is ChatClient)
                {
                    client.send(result);
                }
            }
        }

        public void warnUsersAboutPlayerEventInChat(ChatChannel channel, string info, byte eventId)
        {
            MessageOut msg=new MessageOut(Protocol.CPMSG_CHANNEL_EVENT);
            msg.writeInt16(channel.getId());
            msg.writeInt8(eventId);
            msg.writeString(info);
            sendInChannel(channel, msg);
        }

        void sendInChannel(ChatChannel channel, MessageOut msg)
        {
            List<ChatClient> users=channel.getUserList();

            foreach(ChatClient user in users)
            {
                user.send(msg);
            }
        }

        public ChatClient getClient(string name)
        {
            if(mPlayerMap.ContainsKey(name))
            {
                return mPlayerMap[name];
            }
            else
            {
                return null;
            }
        }

        void removeExpiredPartyInvites()
        {
            DateTime now=DateTime.Now;

            //TODO Reimplementation

            while(mInvitations.Count!=0&&mInvitations[0].mExpireTime<now.Ticks) //TODO Vergleich Überprüfen
            {
                //std::map<std::string, int>::iterator itr;
                //itr = mNumInvites.find(mInvitations.front().mInviter);
                //if (--itr.second <= 0)
                //    mNumInvites.erase(itr);
                //mInvitations.pop_front();
            }
        }

        public void handlePartyInvite(MessageIn msg)
        {
            string inviterName=msg.readString();
            string inviteeName=msg.readString();
            ChatClient inviter=getClient(inviterName);
            ChatClient invitee=getClient(inviteeName);

            if(inviter==null||invitee==null)
                return;

            removeExpiredPartyInvites();
            int maxInvitesPerTimeframe=10;

            int num=mNumInvites[inviterName];

            if(num>=maxInvitesPerTimeframe)
            {
                MessageOut @out=new MessageOut(Protocol.CPMSG_PARTY_REJECTED);
                 @out.writeString(inviterName);
                	@out.writeInt8((int)ErrorMessage.ERRMSG_LIMIT_REACHED);
                inviter.send(@out);
                return;
            }
            ++num;

            if(invitee.party!=null)
            {
                MessageOut @out=new MessageOut(Protocol.CPMSG_PARTY_REJECTED);
                 @out.writeString(inviterName);
                	@out.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                inviter.send(@out);
                return;
            }

            mInvitations.Add(new PartyInvite(inviterName, inviteeName));

            MessageOut msgout=new MessageOut(Protocol.CPMSG_PARTY_INVITED);
            msgout.writeString(inviterName);
            invitee.send(msgout);
        }

        void updateInfo(ChatClient client, int partyId)
        {
            Character character=Program.storage.getCharacter(client.characterName);
            GameServerHandler.sendPartyChange(character, partyId);
        }

        void handlePartyInviteAnswer(ChatClient client, MessageIn msg)
        {
            if(client.party==null)
                return;

            MessageOut outInvitee=new MessageOut(Protocol.CPMSG_PARTY_INVITE_ANSWER_RESPONSE);

            string inviter=msg.readString();

            // check if the invite is still valid
            bool valid=false;
            removeExpiredPartyInvites();
            int size=mInvitations.Count;

            for(int i=0;i<size;i++)
            {
                if(mInvitations[i].mInviter==inviter&&mInvitations[i].mInvitee==client.characterName)
                {
                    valid=true;
                }
            }

            // the invitee did not accept the invitation
            //if (msg.readInt8();)
            //{
            msg.readInt8();

            if(!valid)
                return;

            // send rejection to inviter
            ChatClient inviterClient=getClient(inviter);

            if(inviterClient!=null)
            {
                MessageOut outmsg=new MessageOut(Protocol.CPMSG_PARTY_REJECTED);
                outmsg.writeString(inviter);
                outmsg.writeInt8((int)ErrorMessage.ERRMSG_OK);
                inviterClient.send(outmsg);
            }
            return;
            //}

            // if the invitation has expired, tell the inivtee about it
            if(!valid)
            {
                outInvitee.writeInt8((int)ErrorMessage.ERRMSG_TIME_OUT);
                client.send(outInvitee);
                return;
            }

            // check that the inviter is still in the game
            ChatClient c1=getClient(inviter);
            if(c1==null)
            {
                outInvitee.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                client.send(outInvitee);
                return;
            }

            // if party doesnt exist, create it
            if(c1.party!=null)
            {
                c1.party=new Party();
                //c1.party.addUser(inviter);
                c1.party.addUser("", inviter); //TODO Überprüfen
                // tell game server to update info
                updateInfo(c1, (int)c1.party.getId());
            }

            outInvitee.writeInt8((int)ErrorMessage.ERRMSG_OK);

            List<string> users=c1.party.getUsers();

            int usersSize=users.Count;

            for(int i=0;i<usersSize;i++)
            {
                outInvitee.writeString(users[i]);
            }

            client.send(outInvitee);

            // add invitee to the party
            c1.party.addUser(client.characterName, inviter);
            client.party=c1.party;

            // tell game server to update info
            updateInfo(client, (int)client.party.getId());
        }

        void handlePartyQuit(ChatClient client)
        {
            removeUserFromParty(client);
            MessageOut outmsg=new MessageOut(Protocol.CPMSG_PARTY_QUIT_RESPONSE);
            outmsg.writeInt8((int)ErrorMessage.ERRMSG_OK);
            client.send(outmsg);

            // tell game server to update info
            updateInfo(client, 0);
        }

        void removeUserFromParty(ChatClient client)
        {
            if(client.party!=null)
            {
                client.party.removeUser(client.characterName);
                informPartyMemberQuit(client);

                // if theres less than 1 member left, remove the party
                if(client.party.userCount()<1)
                {
                    client.party=null;
                }
            }
        }

        void informPartyMemberQuit(ChatClient client)
        {
            foreach(KeyValuePair<string, ChatClient> pair in mPlayerMap)
            {
                if(pair.Value.party==client.party)
                {
                    MessageOut outmsg=new MessageOut(Protocol.CPMSG_PARTY_MEMBER_LEFT);
                    outmsg.writeInt32((int)client.characterId);
                    pair.Value.send(outmsg);
                }
            }
        }

        void sendGuildRejoin(ChatClient client)
        {
            // Get list of guilds and check what rights they have.
            List<Guild> guilds=Program.guildManager.getGuildsForPlayer((int)client.characterId);

            foreach(Guild guild in guilds)
            {
                int permissions=guild.getUserPermissions((int)client.characterId);
                string guildName=guild.getName();

                // Tell the client what guilds the character belongs to and their permissions
                MessageOut msg=new MessageOut(Protocol.CPMSG_GUILD_REJOIN);
                msg.writeString(guildName);
                msg.writeInt16(guild.getId());
                msg.writeInt16(permissions);

                // get channel id of guild channel
                ChatChannel channel=joinGuildChannel(guildName, client);

                // send the channel id for the autojoined channel
                msg.writeInt16(channel.getId());
                msg.writeString(channel.getAnnouncement());

                client.send(msg);

                sendGuildListUpdate(guildName, client.characterName, (int)ISL.Server.Enums.GuildValues.GUILD_EVENT_ONLINE_PLAYER);
            }
        }

        ChatChannel joinGuildChannel(string guildName, ChatClient client)
        {
            // Automatically make the character join the guild chat channel
            ChatChannel channel=Program.chatChannelManager.getChannel(guildName);
			
            if(channel==null)
            {
                // Channel doesnt exist so create it
                int channelId=Program.chatChannelManager.createNewChannel(guildName, "Guild Channel", "", false);
                channel=Program.chatChannelManager.getChannel(channelId);
            }

            // Add user to the channel
            if(channel.addUser(client))
            {
                // Send an CPMSG_UPDATE_CHANNEL to warn other clients a user went
                // in the channel.
                warnUsersAboutPlayerEventInChat(channel, client.characterName, (int)ChatValues.CHAT_EVENT_NEW_PLAYER);
            }

            return channel;
        }

        public void sendGuildListUpdate(string guildName, string characterName, byte eventId)
        {
            Guild guild=Program.guildManager.findByName(guildName);

            if(guild!=null)
            {
                MessageOut msg=new MessageOut(Protocol.CPMSG_GUILD_UPDATE_LIST);

                msg.writeInt16(guild.getId());
                msg.writeString(characterName);
                msg.writeInt8(eventId);

                foreach(GuildMember member in guild.getMembers())
                {
                    Character c=Program.storage.getCharacter(member.mId, null);

                    if(mPlayerMap.ContainsKey(c.getName()))
                    {
                        ChatClient chr=mPlayerMap[c.getName()];
                        chr.send(msg);
                    }
                }
            }
        }

        void handleGuildCreate(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_CREATE_RESPONSE);

            // Check if guild already exists and if so, return error
            string guildName=msg.readString();

            if(!Program.guildManager.doesExist(guildName))
            {
                // check the player hasnt already created a guild
                if(Program.guildManager.alreadyOwner((int)client.characterId))
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_LIMIT_REACHED);
                }
                else
                {
                    // Guild doesnt already exist so create it
                    Guild guild=Program.guildManager.createGuild(guildName, (int)client.characterId);
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    reply.writeString(guildName);
                    reply.writeInt16(guild.getId());
                    reply.writeInt16(guild.getUserPermissions((int)client.characterId));

                    // Send autocreated channel id
                    ChatChannel channel=joinGuildChannel(guildName, client);
                    reply.writeInt16(channel.getId());
                }
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_ALREADY_TAKEN);
            }

            client.send(reply);
        }

        void handleGuildInvite(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_INVITE_RESPONSE);
            MessageOut invite=new MessageOut(Protocol.CPMSG_GUILD_INVITED);

            // send an invitation from sender to character to join guild
            int guildId=msg.readInt16();
            string character=msg.readString();

            // get the chat client and the guild
            ChatClient invitedClient=mPlayerMap[character];
            Guild guild=Program.guildManager.findById((short)guildId);

            if(invitedClient!=null&&guild!=null)
            {
                // check permissions of inviter, and that they arent inviting themself,
                // and arent someone already in the guild
                if(guild.canInvite((int)client.characterId)&&(client.characterName!=character)&&!guild.checkInGuild((int)invitedClient.characterId))
                {
                    // send the name of the inviter and the name of the guild
                    // that the character has been invited to join
                    string senderName=client.characterName;
                    string guildName=guild.getName();
                    invite.writeString(senderName);
                    invite.writeString(guildName);
                    invite.writeInt16(guildId);
                    invitedClient.send(invite);
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);

                    // add member to list of invited members to the guild
                    guild.addInvited((int)invitedClient.characterId);
                }
                else
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                }
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }

            client.send(reply);
        }

        void handleGuildAcceptInvite(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_ACCEPT_RESPONSE);
            string guildName=msg.readString();
            bool error=true; // set true by default, and set false only if success

            // check guild exists and that member was invited
            // then add them as guild member
            // and remove from invite list
            Guild guild=Program.guildManager.findByName(guildName);

            if(guild!=null)
            {
                if(guild.checkInvited((int)client.characterId))
                {
                    // add user to guild
                    Program.guildManager.addGuildMember(guild, (int)client.characterId);
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    reply.writeString(guild.getName());
                    reply.writeInt16(guild.getId());
                    reply.writeInt16(guild.getUserPermissions((int)client.characterId));

                    // have character join guild channel
                    ChatChannel channel=joinGuildChannel(guild.getName(), client);
                    reply.writeInt16(channel.getId());
                    sendGuildListUpdate(guildName, client.characterName, (int)GuildValues.GUILD_EVENT_NEW_PLAYER);

                    // success! set error to false
                    error=false;
                }
            }

            if(error)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }

            client.send(reply);
        }

        void handleGuildGetMembers(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_GET_MEMBERS_RESPONSE);
            short guildId=msg.readInt16();
            Guild guild=Program.guildManager.findById(guildId);

            // check for valid guild
            // write a list of member names that belong to the guild
            if(guild!=null)
            {
                // make sure the requestor is in the guild
                if(guild.checkInGuild((int)client.characterId))
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    reply.writeInt16(guildId);

                    foreach(GuildMember member in guild.getMembers())
                    {
                        Character c=Program.storage.getCharacter(member.mId, null);
                        string memberName=c.getName();
                        reply.writeString(memberName);
                        reply.writeInt8((int)((mPlayerMap.ContainsKey(memberName)?1:0)));
                    }
                }
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }

            client.send(reply);
        }

        void handleGuildMemberLevelChange(ChatClient client, MessageIn msg)
        {
            // get the guild, the user to change the permissions, and the new permission
            // check theyre valid, and then change them
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_PROMOTE_MEMBER_RESPONSE);
            short guildId=msg.readInt16();
            string user=msg.readString();
            short level=msg.readInt8();

            Guild guild=Program.guildManager.findById(guildId);
            Character c=Program.storage.getCharacter(user);

            if(guild!=null&&c!=null)
            {
                int rights=guild.getUserPermissions(c.getDatabaseID())|level;
                if(Program.guildManager.changeMemberLevel(client, guild, c.getDatabaseID(), rights)==0)
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    client.send(reply);
                }
            }

            reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE); //TODO Muss das so oder fehlt oben ein return?
            client.send(reply);
        }

        void handleGuildKickMember(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_KICK_MEMBER_RESPONSE);
            short guildId=msg.readInt16();
            string user=msg.readString();

            Guild guild=Program.guildManager.findById(guildId);
            Character c=Program.storage.getCharacter(user);

            if(guild!=null&&c!=null)
            {
                if((guild.getUserPermissions(c.getDatabaseID())&(int)(GuildAccessLevel.GAL_KICK))!=0) //TODO Überprüfen ob Vergleich so richtig rum
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                }
                else
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_INSUFFICIENT_RIGHTS);
                }
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }

            client.send(reply);
        }

        void handleGuildQuit(ChatClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.CPMSG_GUILD_QUIT_RESPONSE);
            short guildId=msg.readInt16();
            Guild guild=Program.guildManager.findById(guildId);

            // check for valid guild
            // check the member is in the guild
            // remove the member from the guild
            if(guild!=null)
            {
                if(guild.checkInGuild((int)client.characterId))
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                    reply.writeInt16(guildId);

                    // Check if there are no members left, remove the guild channel
                    if(guild.memberCount()==0)
                    {
                        Program.chatChannelManager.removeChannel(Program.chatChannelManager.getChannelId(guild.getName()));
                    }

                    // guild manager checks if the member is the last in the guild
                    // and removes the guild if so
                    Program.guildManager.removeGuildMember(guild, (int)client.characterId);
                    sendGuildListUpdate(guild.getName(), client.characterName, (int)GuildValues.GUILD_EVENT_LEAVING_PLAYER);
                }
                else
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                }
            }
            else
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }

            //client.send(reply);
        }

        void guildChannelTopicChange(ChatChannel channel, int playerId, string topic)
        {
            Guild guild=Program.guildManager.findByName(channel.getName());

            if(guild!=null)
            {
                if((guild.getUserPermissions(playerId)&(int)GuildAccessLevel.GAL_TOPIC_CHANGE)!=0) //TODO Überprüfen ob Vergleich so richtig rum
                {
                    Program.chatChannelManager.setChannelTopic(channel.getId(), topic);
                }
            }
        }
    }
}
