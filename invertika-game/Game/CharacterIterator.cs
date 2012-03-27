//
//  CharacterIterator.cs
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
	 * Iterates through the Characters of a region.
	 */
	public class CharacterIterator
	{
		ZoneIterator iterator;
		ushort pos;
		Character current;

		//CharacterIterator(const ZoneIterator &);
		//void operator++();
		//Character *operator*() const { return current; }
		//operator bool() const { return iterator; }

		//        CharacterIterator::CharacterIterator(const ZoneIterator &it)
		//  : iterator(it), pos(0)
		//{
		//    while (iterator && (*iterator)->nbCharacters == 0) ++iterator;
		//    if (iterator)
		//    {
		//        current = static_cast< Character * >((*iterator)->objects[pos]);
		//    }
		//}

		//void CharacterIterator::operator++()
		//{
		//    if (++pos == (*iterator)->nbCharacters)
		//    {
		//        do ++iterator; while (iterator && (*iterator)->nbCharacters == 0);
		//        pos = 0;
		//    }
		//    if (iterator)
		//    {
		//        current = static_cast< Character * >((*iterator)->objects[pos]);
		//    }
		//}
	}
}
