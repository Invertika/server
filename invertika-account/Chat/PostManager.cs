//
//  PostManager.cs
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
using invertika_account.Account;
using ISL.Server.Account;

namespace invertika_account.Chat
{
	public class PostManager
	{
		Dictionary<Character, Post> mPostBox;

		public void addLetter(Letter letter)
		{
			Character character=letter.getReceiver();

			if(mPostBox.ContainsKey(character))
			{
				mPostBox[character].addLetter(letter);
			}
			else
			{
				Post post=new Post();
				post.addLetter(letter);
				mPostBox.Add(character, post);
			}
		}

		public Post getPost(Character player)
		{
			if(mPostBox.ContainsKey(player))
			{
				return mPostBox[player];
			}
				
			return null;
		}

		public void clearPost(Character player)
		{
			if(mPostBox.ContainsKey(player))
			{
				mPostBox.Remove(player);
			}
		}
	}
}
