//
//  ChatChannel.cs
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
	public class ChatChannel
	{
		ushort mId;            /**< The ID of the channel. */
		string mName;             /**< The name of the channel. */
		string mAnnouncement;     /**< Welcome message. */
		string mPassword;         /**< The channel password. */
		bool mJoinable;                /**< Whether anyone can join. */
		List<ChatClient> mRegisteredUsers; /**< Users in this channel. */
		string mOwner;             /**< Channel owner character name */

		public ChatChannel(int id, string name, string announcement, string password, bool joinable)
		{
			mId=(ushort)id;
			mName=name;
			mAnnouncement=announcement;
			mPassword=password;
			mJoinable=joinable;
		}

		bool addUser(ChatClient user)
		{
			// First user is the channel owner
			if(mRegisteredUsers.Count()<1)
			{
				mOwner=user.characterName;
				setUserMode(user, (byte)'o');
			}

			// Check if the user already exists in the channel
			foreach(ChatClient chatClient in mRegisteredUsers)
			{
				if(chatClient.characterName==user.characterName)
				{
					return false;
				}
			}

			mRegisteredUsers.Add(user);
			user.channels.Add(this);

			// set user as logged in
			setUserMode(user, (byte)'l');

			// if owner has rejoined, give them ops
			if(user.characterName==mOwner)
			{
				setUserMode(user, (byte)'o');
			}

			return true;
		}

		public bool removeUser(ChatClient user)
		{
			user.channels.Clear();
			user.userModes.Clear();

			mRegisteredUsers.Remove(user);

			return true;
		}

		public void removeAllUsers()
		{
			foreach(ChatClient chatClient in mRegisteredUsers)
			{
				chatClient.channels.Clear();
				chatClient.userModes.Clear();
			}

			mRegisteredUsers.Clear();
		}

		public bool canJoin()
		{
			return mJoinable;
		}

		void setUserMode(ChatClient user, byte mode)
		{
			if(user.userModes.ContainsKey(this))
			{
				user.userModes[this]+=mode;
			}
			else
			{
				user.userModes.Add(this, mode.ToString());
			}
		}

		string getUserMode(ChatClient user)
		{
			try
			{
				return user.userModes[this];
			}
			catch
			{
				return "";
			}
		}

		/**
 * Get the name of the channel.
 */
		public string getName()
		{
			return mName;
		}

		/**
 * Sets the announcement string of the channel.
 */
		public void setAnnouncement(string channelAnnouncement)
		{
			mAnnouncement=channelAnnouncement;
		}
	}
}
