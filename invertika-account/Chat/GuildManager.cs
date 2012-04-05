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

namespace invertika_account.Chat
{
	public class GuildManager
	{
		List<Guild> mGuilds;
		List<int> mOwners;

		public GuildManager()
		{
			// Load stored guilds from db
			//mGuilds = storage->getGuildList();
		}

		~GuildManager()
		{
			//for (std::list<Guild*>::iterator itr = mGuilds.begin();
			//        itr != mGuilds.end(); ++itr)
			//{
			//    delete *itr;
			//}
			//mGuilds.clear();
		}

		public Guild createGuild(string name, int playerId)
		{
			//Guild *guild = new Guild(name);
			//// Add guild to db
			//storage->addGuild(guild);

			//// Add guild, and add owner
			//mGuilds.push_back(guild);
			//mOwners.push_back(playerId);

			//// put the owner in the guild
			//addGuildMember(guild, playerId);

			//// Set and save the member rights
			//storage->setMemberRights(guild->getId(), playerId, GAL_OWNER);

			//guild->setOwner(playerId);

			//return guild;

			return null; //ssk
		}

		void removeGuild(Guild guild)
		{
			//storage->removeGuild(guild);
			//mOwners.remove(guild->getOwner());
			//mGuilds.remove(guild);
			//delete guild;
		}

		public void addGuildMember(Guild guild, int playerId)
		{
			//storage->addGuildMember(guild->getId(), playerId);
			//guild->addMember(playerId);
		}

		void removeGuildMember(Guild guild, int playerId)
		{
			//// remove the user from the guild
			//storage->removeGuildMember(guild->getId(), playerId);
			//guild->removeMember(playerId);

			//// if theres no more members left delete the guild
			//if (guild->memberCount() == 0)
			//    removeGuild(guild);

			//// remove the user from owners list
			//std::list<int>::iterator itr = mOwners.begin();
			//std::list<int>::iterator itr_end = mOwners.end();
			//while (itr != itr_end)
			//{
			//    if ((*itr) == playerId)
			//    {
			//        mOwners.remove(playerId);
			//        break;
			//    }
			//    ++itr;
			//}
		}

		public Guild findById(short id)
		{
			//for (std::list<Guild*>::const_iterator itr = mGuilds.begin(),
			//        itr_end = mGuilds.end();
			//        itr != itr_end; ++itr)
			//{
			//    Guild *guild = (*itr);
			//    if (guild->getId() == id)
			//        return guild;
			//}

			return null;
		}

		public Guild findByName(string name)
		{
			//for (std::list<Guild*>::const_iterator itr = mGuilds.begin(),
			//        itr_end = mGuilds.end();
			//        itr != itr_end; ++itr)
			//{
			//    Guild *guild = (*itr);
			//    if (guild->getName() == name)
			//        return guild;
			//}
			return null;
		}

		public bool doesExist(string name)
		{
			return findByName(name)!=null;
		}

		public List<Guild> getGuildsForPlayer(int playerId)
		{
			List<Guild> guildList=new List<Guild>();

			//for (std::list<Guild*>::const_iterator itr = mGuilds.begin();
			//        itr != mGuilds.end(); ++itr)
			//{
			//    if ((*itr)->checkInGuild(playerId))
			//    {
			//        guildList.push_back((*itr));
			//    }
			//}

			return guildList;
		}

		public void disconnectPlayer(ChatClient player)
		{
			//std::vector<Guild*> guildList = getGuildsForPlayer(player->characterId);

			//for (std::vector<Guild*>::const_iterator itr = guildList.begin();
			//     itr != guildList.end(); ++itr)
			//{
			//    chatHandler->sendGuildListUpdate((*itr)->getName(),
			//                                     player->characterName,
			//                                     GUILD_EVENT_OFFLINE_PLAYER);
			//}
		}

		int changeMemberLevel(ChatClient player, Guild guild, int playerId, int level)
		{
			//if (guild->checkInGuild(player->characterId) && guild->checkInGuild(playerId))
			//{
			//    int playerLevel = guild->getUserPermissions(player->characterId);

			//    if (playerLevel == GAL_OWNER)
			//    {
			//        // player can modify anyones permissions
			//        setUserRights(guild, playerId, level);
			//        return 0;
			//    }
			//}

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
			//storage->setMemberRights(guild->getId(), playerId, rights);

			//// Set with guild
			//guild->setUserPermissions(playerId, rights);
		}
	}
}
