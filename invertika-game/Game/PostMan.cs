//
//  PostMan.cs
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

namespace invertika_game.Game
{
	public class PostMan
	{
		public Character getCharacter(int id)
		{
			//std::map<int, Character*>::const_iterator itr = mCharacters.find(id);
			//if (itr != mCharacters.end())
			//    return itr.second;
			//return 0;

			return null; //ssk
		}

		public void addCharacter(Character player)
		{
			//std::map<int, Character*>::iterator itr = mCharacters.find(player.getDatabaseID());
			//if (itr == mCharacters.end())
			//{
			//    mCharacters.insert(std::pair<int, Character*>(player.getDatabaseID(), player));
			//}
		}

		void getPost(Character player)//, PostCallback &f)
		{
			//mCallbacks.insert(std::pair<Character*, PostCallback>(player, f));
			//accountHandler.getPost(player);
		}

		public void gotPost(Character player, string sender, string letter)
		{
			//std::map<Character*, PostCallback>::iterator itr = mCallbacks.find(player);
			//if (itr != mCallbacks.end())
			//{
			//    itr.second.handler(player, sender, letter, itr.second.data);
			//}
		}

		Dictionary<int, Character> mCharacters;
		//Dictionary<Character, PostCallback> mCallbacks;
	}
}
