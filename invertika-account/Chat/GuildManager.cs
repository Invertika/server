//
//  GuildManager.cs
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
	public class GuildManager
	{
		List<Guild> mGuilds;
		List<int> mOwners;

		public GuildManager()
		{
			// Load stored guilds from db
			mGuilds=Program.storage.getGuildList();
		}

		public Guild createGuild(string name, int playerId)
		{
			Guild guild=new Guild(name);
			// Add guild to db
			Program.storage.addGuild(guild);

			// Add guild, and add owner
			mGuilds.Add(guild);
			mOwners.Add(playerId);

			// put the owner in the guild
			addGuildMember(guild, playerId);

			// Set and save the member rights
			Program.storage.setMemberRights(guild.getId(), playerId, (int)GuildAccessLevel.GAL_OWNER);

			guild.setOwner(playerId);

			return guild;
		}

		void removeGuild(Guild guild)
		{
			Program.storage.removeGuild(guild);
			mOwners.Remove(guild.getOwner());
			mGuilds.Remove(guild);
		}

		public void addGuildMember(Guild guild, int playerId)
		{
			Program.storage.addGuildMember(guild.getId(), playerId);
			guild.addMember(playerId, (int)GuildAccessLevel.GAL_NONE);
		}

		public void removeGuildMember(Guild guild, int playerId)
		{
			// remove the user from the guild
			Program.storage.removeGuildMember(guild.getId(), playerId);
			guild.removeMember(playerId);

			// if theres no more members left delete the guild
			if (guild.memberCount() == 0)
			    removeGuild(guild);

			// remove the user from owners list
			mOwners.Remove(playerId);
		}

		public Guild findById(short id)
		{
			foreach(Guild guild in mGuilds)
			{
				if(guild.getId()==id) return guild;
			}
				
			return null;
		}

		public Guild findByName(string name)
		{
			foreach(Guild guild in mGuilds)
			{
				if(guild.getName()==name) return guild;
			}

			return null;
		}

		public bool doesExist(string name)
		{
			return findByName(name)!=null;
		}

		public List<Guild> getGuildsForPlayer(int playerId)
		{
			List<Guild> guildList=new List<Guild>();

			foreach(Guild guild in mGuilds)
			{
				if(guild.checkInGuild(playerId))
				{
					guildList.Add(guild);
				}
			}

			return guildList;
		}

		public void disconnectPlayer(ChatClient player)
		{
			List<Guild> guildList = getGuildsForPlayer((int)player.characterId);

			foreach(Guild guild in guildList)
			{
				Program.chatHandler.sendGuildListUpdate(guild.getName(), player.characterName, (byte)GuildValues.GUILD_EVENT_OFFLINE_PLAYER);
			}
		}

		public int changeMemberLevel(ChatClient player, Guild guild, int playerId, int level)
		{
			if(guild.checkInGuild((int)player.characterId)&&guild.checkInGuild(playerId))
			{
				int playerLevel=guild.getUserPermissions((int)player.characterId);

				if(playerLevel==(int)GuildAccessLevel.GAL_OWNER)
				{
					// player can modify anyones permissions
					setUserRights(guild, playerId, level);
					return 0;
				}
			}

			return -1;
		}

		public bool alreadyOwner(int playerId)
		{
			//std::list<int>::const_iterator itr = mOwners.begin();
			//std::list<int>::const_iterator itr_end = mOwners.end();

			//while (itr != itr_end)
			//{
			//    if ((*itr) == playerId)
			//        return true;
			//    ++itr;
			//}

			return false;
		}

		void setUserRights(Guild guild, int playerId, int rights)
		{
			//// Set and save the member rights
			//storage.setMemberRights(guild.getId(), playerId, rights);

			//// Set with guild
			//guild.setUserPermissions(playerId, rights);
		}
	}
}
