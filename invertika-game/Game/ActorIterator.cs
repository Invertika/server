//
//  ActorIterator.cs
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
	/**
	 * Iterates through the Actors of a region.
	 */
	public class ActorIterator
	{
		//ZoneIterator iterator;
		//unsigned short pos;
		//Actor *current;

		//ActorIterator(const ZoneIterator &);
		//void operator++();
		//Actor *operator*() const { return current; }
		//operator bool() const { return iterator; }

		//        ActorIterator::ActorIterator(const ZoneIterator &it)
		//  : iterator(it), pos(0)
		//{
		//    while (iterator && (*iterator).objects.empty()) ++iterator;
		//    if (iterator)
		//    {
		//        current = (*iterator).objects[pos];
		//    }
		//}

		//void ActorIterator::operator++()
		//{
		//    if (++pos == (*iterator).objects.size())
		//    {
		//        do ++iterator; while (iterator && (*iterator).objects.empty());
		//        pos = 0;
		//    }
		//    if (iterator)
		//    {
		//        current = (*iterator).objects[pos];
		//    }
		//}
	}
}
