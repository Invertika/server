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
using ISL.Server.Common;
using ISL.Server.Enums;

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
			mChatChannels=new Dictionary<ushort, ChatChannel>();
			mChannelsNoLongerUsed=new List<int>();
		}

		int createNewChannel(string channelName, string channelAnnouncement, string channelPassword, bool joinable)
		{
			int channelId=nextUsable();

			// Register channel
			mChatChannels.Add((ushort)channelId, new ChatChannel(channelId, channelName, channelAnnouncement, channelPassword, joinable));

			return channelId;
		}

		public bool tryNewPublicChannel(string name)
		{
			//Slangfilter �berpr�fung
			if (!Program.stringFilter.filterContent(name))
			{
			    return false;
			}

			// Checking strings for length and double quotes
			uint maxNameLength = (uint)Configuration.getValue("chat_maxChannelNameLength", 15);
			
			if (name==null|| name.Length>maxNameLength || Program.stringFilter.findDoubleQuotes(name))   
			{
			    return false;
			}
			else if (Program.guildManager.doesExist(name) ||  channelExists(name))
			{
			    // Channel already exists
			    return false;
			}
			else
			{
			    // We attempt to create a new channel
			    int id = createNewChannel(name, "", "", true);
			    return id != 0;
			}
		}

		public bool removeChannel(int channelId)
		{
			if(mChatChannels.ContainsKey((ushort)channelId)==false) return false;

			mChatChannels[(ushort)channelId].removeAllUsers();
			mChatChannels.Remove((ushort)channelId);
			mChannelsNoLongerUsed.Add(channelId);

			return true;
		}

		public List<ChatChannel> getPublicChannels()
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

		public ChatChannel getChannel(int channelId)
		{
			if(mChatChannels.ContainsKey((ushort)channelId))
			{
				return mChatChannels[(ushort)channelId];
			}

			return null;
		}

		public ChatChannel getChannel(string name)
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

		public void setChannelTopic(int channelId, string topic)
		{
			if(mChatChannels.ContainsKey((ushort)channelId))
			{
				ChatChannel channel=mChatChannels[(ushort)channelId];
				channel.setAnnouncement(topic);
				Program.chatHandler.warnUsersAboutPlayerEventInChat(channel, topic, (int)ChatValues.CHAT_EVENT_TOPIC_CHANGE);
			}
		}

		public void removeUserFromAllChannels(ChatClient user)
		{
			foreach(ChatChannel channel in user.channels)
			{
				Program.chatHandler.warnUsersAboutPlayerEventInChat(channel, user.characterName, (int)ChatValues.CHAT_EVENT_LEAVING_PLAYER);
				channel.removeUser(user);
			}
		}

		public bool channelExists(int channelId)
		{
			return mChatChannels.ContainsKey((ushort)channelId);
		}

		public bool channelExists(string channelName)
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
