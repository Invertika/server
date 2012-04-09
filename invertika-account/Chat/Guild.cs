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
using ISL.Server.Enums;

namespace invertika_account.Chat
{
	public class Guild
	{
		short mId;
		string mName;
		List<GuildMember> mMembers;
		List<int> mInvited;

		public Guild(string name)
		{
			mName=name;
		}

		public void addMember(int playerId, int permissions)
		{
			// create new guild member
			GuildMember member=new GuildMember();
			member.mId=playerId;
			member.mPermissions=permissions;

			// add new guild member to guild
			mMembers.Add(member);

			if(checkInvited(playerId))
			{
				mInvited.Remove(playerId);
			}
		}

		public void removeMember(int playerId)
		{
			if(getOwner()==playerId)
			{
				// if the leader is leaving, assign next member as leader
				if(mMembers.Count>0)
				{
					setOwner(mMembers[0].mId);
				}
			}

			GuildMember member=getMember(playerId);
			if(member!=null) mMembers.Remove(member); //Überprüfen ob Ausage sorum richtig ist
		}

		public int getOwner()
		{
			foreach(GuildMember member in mMembers)
			{
				if(member.mPermissions==(int)GuildAccessLevel.GAL_OWNER) return member.mId;
			}

			return 0;
		}

		public void setOwner(int playerId)
		{
			GuildMember member=getMember(playerId);

			if(member!=null)
			{
				member.mPermissions=(int)GuildAccessLevel.GAL_OWNER;
			}
		}

		public bool checkInvited(int playerId)
		{
			return mInvited.Contains(playerId);
		}

		public void addInvited(int playerId)
		{
			mInvited.Add(playerId);
		}

		public bool checkInGuild(int playerId)
		{
			return getMember(playerId)!=null;
		}

		GuildMember getMember(int playerId)
		{
			foreach(GuildMember member in mMembers)
			{
				if(member.mId==playerId) return member;
			}

			return null;
		}

		public bool canInvite(int playerId)
		{
			// Guild members with permissions above NONE can invite
			// Check that guild members permissions are not NONE
			GuildMember member=getMember(playerId);
			if((member.mPermissions&(int)(GuildAccessLevel.GAL_INVITE))!=0) return true; //TODO schauen ob Vergleich so richtig rum
			else return false;
		}

		public int getUserPermissions(int playerId)
		{
			GuildMember member=getMember(playerId);
			return member.mPermissions;
		}

		void setUserPermissions(int playerId, int level)
		{
			GuildMember member=getMember(playerId);
			member.mPermissions=level;
		}

		/// <summary>
		///  Returns the ID of the guild.
		/// </summary>
		/// <returns></returns>
		public int getId()
		{
			return mId;
		}

		/// <summary>
		/// Returns the name of the guild.
		/// </summary>
		/// <returns></returns>
		public string getName()
		{
			return mName;
		}

		/// <summary>
		/// Returns a list of the members in this guild.
		/// </summary>
		/// <returns></returns>
		public List<GuildMember> getMembers()
		{
			return mMembers;
		}

		/// <summary>
		/// Returns the number of members in the guild.
		/// </summary>
		/// <returns></returns>
		public int memberCount()
		{
			return mMembers.Count;
		}
	}
}
