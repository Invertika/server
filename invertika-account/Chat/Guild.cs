//
//  Guild.cs
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
	public class Guild
	{
		short mId;
		string mName;
		List<GuildMember> mMembers;
		List<int> mInvited;


		Guild(string name)
		{
			mName=name;
		}

		~Guild()
		{
		}

		void addMember(int playerId, int permissions)
		{
			//// create new guild member
			//GuildMember *member = new GuildMember;
			//member->mId = playerId;
			//member->mPermissions = permissions;

			//// add new guild member to guild
			//mMembers.push_back(member);

			//if (checkInvited(playerId))
			//{
			//    mInvited.remove(playerId);
			//}
		}

		void removeMember(int playerId)
		{
			//if (getOwner() == playerId)
			//{
			//    // if the leader is leaving, assign next member as leader
			//    std::list<GuildMember*>::iterator itr = mMembers.begin();
			//    ++itr;
			//    if (itr != mMembers.end())
			//        setOwner((*itr)->mId);
			//}
			//GuildMember *member = getMember(playerId);
			//if (member)
			//    mMembers.remove(member);
		}

		int getOwner()
		{
			//std::list<GuildMember*>::const_iterator itr = mMembers.begin();
			//std::list<GuildMember*>::const_iterator itr_end = mMembers.end();

			//while (itr != itr_end)
			//{
			//    if ((*itr)->mPermissions == GAL_OWNER)
			//        return (*itr)->mId;
			//    ++itr;
			//}

			return 0;
		}

		void setOwner(int playerId)
		{
			//GuildMember *member = getMember(playerId);
			//if (member)
			//{
			//    member->mPermissions = GAL_OWNER;
			//}
		}

		bool checkInvited(int playerId)
		{
			//return std::find(mInvited.begin(), mInvited.end(), playerId) != mInvited.end();

			return true; //ssk
		}

		void addInvited(int playerId)
		{
			//mInvited.push_back(playerId);
		}

		bool checkInGuild(int playerId)
		{
			return getMember(playerId)!=null;
		}

		GuildMember getMember(int playerId)
		{
			//std::list<GuildMember*>::const_iterator itr = mMembers.begin(),
			//                                        itr_end = mMembers.end();
			//while (itr != itr_end)
			//{
			//    if ((*itr)->mId == playerId)
			//        return (*itr);
			//    ++itr;
			//}

			return null;
		}

		bool canInvite(int playerId)
		{
			//// Guild members with permissions above NONE can invite
			//// Check that guild members permissions are not NONE
			//GuildMember *member = getMember(playerId);
			//if (member->mPermissions & GAL_INVITE)
			//    return true;
			return false;
		}

		public int getUserPermissions(int playerId)
		{
			//GuildMember *member = getMember(playerId);
			//return member->mPermissions;

			return 0; //ssk
		}

		void setUserPermissions(int playerId, int level)
		{
			//GuildMember *member = getMember(playerId);
			//member->mPermissions = level;
		}

		/**
 * Returns the ID of the guild.
 */
		public int getId()
		{
			return mId;
		}

		/**
 * Returns the name of the guild.
 */
		public string getName()
		{
			return mName;
		}

		/**
 * Returns a list of the members in this guild.
 */
		public List<GuildMember> getMembers()
		{
			return mMembers;
		}
	}
}
