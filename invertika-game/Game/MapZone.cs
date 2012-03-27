//
//  MapZone.cs
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
* Part of a map.
*/
	public class MapZone
	{
		ushort nbCharacters, nbMovingObjects;
		/**
		 * Objects present in this zone.
		 * Characters are stored first, then the remaining MovingObjects, then the
		 * remaining Objects.
		 */
		List<Actor> objects;

		/**
		 * Destinations of the objects that left this zone.
		 * This is necessary in order to have an accurate iterator around moving
		 * objects.
		 */
		List<uint> destinations;

		MapZone()
		{
		}

		void insert(Actor obj)
		{
			//int type = obj->getType();
			//switch (type)
			//{
			//    case OBJECT_CHARACTER:
			//    {
			//        if (nbCharacters != nbMovingObjects)
			//        {
			//            if (nbMovingObjects != objects.size())
			//            {
			//                objects.push_back(objects[nbMovingObjects]);
			//                objects[nbMovingObjects] = objects[nbCharacters];
			//            }
			//            else
			//            {
			//                objects.push_back(objects[nbCharacters]);
			//            }
			//            objects[nbCharacters] = obj;
			//            ++nbCharacters;
			//            ++nbMovingObjects;
			//            break;
			//        }
			//        ++nbCharacters;
			//    } // no break!
			//    case OBJECT_MONSTER:
			//    case OBJECT_NPC:
			//    {
			//        if (nbMovingObjects != objects.size())
			//        {
			//            objects.push_back(objects[nbMovingObjects]);
			//            objects[nbMovingObjects] = obj;
			//            ++nbMovingObjects;
			//            break;
			//        }
			//        ++nbMovingObjects;
			//    } // no break!
			//    default:
			//    {
			//        objects.push_back(obj);
			//    }
			//}
		}

		void remove(Actor obj)
		{
			//std::vector< Actor * >::iterator i_beg = objects.begin(), i, i_end;
			//int type = obj->getType();
			//switch (type)
			//{
			//    case OBJECT_CHARACTER:
			//    {
			//        i = i_beg;
			//        i_end = objects.begin() + nbCharacters;
			//    } break;
			//    case OBJECT_MONSTER:
			//    case OBJECT_NPC:
			//    {
			//        i = objects.begin() + nbCharacters;
			//        i_end = objects.begin() + nbMovingObjects;
			//    } break;
			//    default:
			//    {
			//        i = objects.begin() + nbMovingObjects;
			//        i_end = objects.end();
			//    }
			//}
			//i = std::find(i, i_end, obj);
			//assert(i != i_end);
			//unsigned pos = i - i_beg;
			//if (pos < nbCharacters)
			//{
			//    objects[pos] = objects[nbCharacters - 1];
			//    pos = nbCharacters - 1;
			//    --nbCharacters;
			//}
			//if (pos < nbMovingObjects)
			//{
			//    objects[pos] = objects[nbMovingObjects - 1];
			//    pos = nbMovingObjects - 1;
			//    --nbMovingObjects;
			//}
			//objects[pos] = objects[objects.size() - 1];
			//objects.pop_back();
		}
	}
}
