//
//  Post.cs
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

namespace invertika_account.Chat
{
	public class Post
	{
		List<Letter> mLetters;

		~Post()
		{
			//std::vector<Letter*>::iterator itr_end = mLetters.end();
			//for (std::vector<Letter*>::iterator itr = mLetters.begin();
			//     itr != itr_end;
			//     ++itr)
			//{
			//    delete (*itr);
			//}

			//mLetters.clear();
		}

		bool addLetter(Letter letter)
		{
			uint max=(uint)Configuration.getValue("mail_maxLetters", 10);
			if(mLetters.Count>max)
			{
				return false;
			}

			mLetters.Add(letter);

			return true;
		}

		public Letter getLetter(int letter)
		{
			if(letter<0||(UInt64)letter>(UInt64)mLetters.Count)
			{
				return null;
			}
			return mLetters[letter];
		}

		public uint getNumberOfLetters()
		{
			return (uint)mLetters.Count;
		}
	}
}
