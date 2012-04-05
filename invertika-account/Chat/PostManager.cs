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
			//std::map<Character*, Post*>::iterator itr =
			//    mPostBox.find(letter.getReceiver());
			//if (itr != mPostBox.end())
			//{
			//    itr.second.addLetter(letter);
			//}
			//else
			//{
			//    Post *post = new Post();
			//    post.addLetter(letter);
			//    mPostBox.insert(
			//        std::pair<Character*, Post*>(letter.getReceiver(), post)
			//        );
			//}
		}

		public Post getPost(Character player)
		{
			//std::map<Character*, Post*>::const_iterator itr = mPostBox.find(player);
			//return (itr == mPostBox.end()) ? NULL : itr.second;

			return null; //ssk
		}

		public void clearPost(Character player)
		{
			//std::map<Character*, Post*>::iterator itr =
			//    mPostBox.find(player);
			//if (itr != mPostBox.end())
			//{
			//    delete itr.second;
			//    mPostBox.erase(itr);
			//}
		}
	}
}
