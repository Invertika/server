//
//  ChatChannelManager.cs
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

namespace invertika_account.Chat
{
	public class ChatChannelManager
	{
		/**
		 * The map keeping all the chat channels. The channel id must be
		 * unique.
		 */
		Dictionary<ushort, ChatChannel> mChatChannels;
		int mNextChannelId;
		List<int> mChannelsNoLongerUsed;

		public ChatChannelManager()
		{
			mNextChannelId=1;
		}

		int createNewChannel(string channelName, string channelAnnouncement, string channelPassword, bool joinable)
		{
			int channelId=nextUsable();

			// Register channel
			mChatChannels.Add((ushort)channelId, new ChatChannel(channelId, channelName, channelAnnouncement, channelPassword, joinable));

			return channelId;
		}

		bool tryNewPublicChannel(string name)
		{
			//if (!stringFilter->filterContent(name))
			//{
			//    return false;
			//}

			//// Checking strings for length and double quotes
			//unsigned maxNameLength = Configuration::getValue("chat_maxChannelNameLength", 15);
			//if (name.empty() ||
			//    name.length() > maxNameLength ||
			//    stringFilter->findDoubleQuotes(name))
			//{
			//    return false;
			//}
			//else if (guildManager->doesExist(name) ||
			//         channelExists(name))
			//{
			//    // Channel already exists
			//    return false;
			//}
			//else
			//{
			//    // We attempt to create a new channel
			//    short id = createNewChannel(name, std::string(), std::string(), true);
			//    return id != 0;
			//}

			return true;
		}

		bool removeChannel(int channelId)
		{
			if(mChatChannels.ContainsKey((ushort)channelId)==false) return false;

			mChatChannels[(ushort)channelId].removeAllUsers();
			mChatChannels.Remove((ushort)channelId);
			mChannelsNoLongerUsed.Add(channelId);

			return true;
		}

		List<ChatChannel> getPublicChannels()
		{
			List<ChatChannel> channels=new List<ChatChannel>();

			foreach(ChatChannel channel in mChatChannels.Values)
			{
				if(channel.canJoin())
				{
					channels.Add(channel);
				}
			}

			return channels;
		}

		int getChannelId(string channelName)
		{
			foreach(KeyValuePair<ushort, ChatChannel> pair in mChatChannels)
			{
				if(pair.Value.getName()==channelName)
				{
					return (int)pair.Key;
				}
			}

			return 0;
		}

		ChatChannel getChannel(int channelId)
		{
			if(mChatChannels.ContainsKey((ushort)channelId))
			{
				return mChatChannels[(ushort)channelId];
			}

			return null;
		}

		ChatChannel getChannel(string name)
		{
			foreach(ChatChannel channel in mChatChannels.Values)
			{
				if(channel.getName()==name)
				{
					return channel;
				}
			}

			return null;
		}

		void setChannelTopic(int channelId, string topic)
		{
			//ChatChannels::iterator i = mChatChannels.find(channelId);
			//if (i == mChatChannels.end())
			//    return;

			//i->second.setAnnouncement(topic);
			//chatHandler->warnUsersAboutPlayerEventInChat(&(i->second),
			//                                             topic,
			//                                             CHAT_EVENT_TOPIC_CHANGE);
		}

		void removeUserFromAllChannels(ChatClient user)
		{
			//// Local copy as they will be destroyed under our feet.
			//std::vector<ChatChannel *> channels = user->channels;
			//// Reverse iterator to reduce load on vector operations.
			//for (std::vector<ChatChannel *>::const_reverse_iterator
			//     i = channels.rbegin(), i_end = channels.rend(); i != i_end; ++i)
			//{
			//    chatHandler->warnUsersAboutPlayerEventInChat((*i),
			//                                                 user->characterName,
			//                                                 CHAT_EVENT_LEAVING_PLAYER);
			//    (*i)->removeUser(user);
			//}
		}

		bool channelExists(int channelId)
		{
			return mChatChannels.ContainsKey((ushort)channelId);
		}

		bool channelExists(string channelName)
		{
			foreach(ChatChannel channel in mChatChannels.Values)
			{
				if(channel.getName()==channelName)
				{
					return true;
				}
			}

			return false;
		}

		int nextUsable()
		{
			int channelId=0;

			if(mChannelsNoLongerUsed.Count>0)
			{
				channelId=mChannelsNoLongerUsed[0];
				mChannelsNoLongerUsed.Remove(0);
			}
			else
			{
				channelId=mNextChannelId;
				++mNextChannelId;
			}

			return channelId;
		}
	}
}
