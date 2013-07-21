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
using ISL.Server.Common;

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
        public List<uint> destinations=new List<uint>();

        public MapZone()
        {
			objects=new List<Actor>();
        }

        public void insert(Actor obj)
        {
            ThingType type=obj.getType();

            if(type==ThingType.OBJECT_CHARACTER)
            {
                if(nbCharacters!=nbMovingObjects)
                {
                    if(nbMovingObjects!=objects.Count)
                    {
                        objects.Add(objects[nbMovingObjects]);
                        objects[nbMovingObjects]=objects[nbCharacters];
                    }
                    else
                    {
                        objects.Add(objects[nbCharacters]);
                    }
                    
                    objects[nbCharacters]=obj;
                    ++nbCharacters;
                    ++nbMovingObjects;
                    //break;
                }
                ++nbCharacters;
            }

            if(type==ThingType.OBJECT_CHARACTER||type==ThingType.OBJECT_MONSTER||type==ThingType.OBJECT_NPC)
            {
                if(nbMovingObjects!=objects.Count)
                {
                    objects.Add(objects[nbMovingObjects]);
                    objects[nbMovingObjects]=obj;
                    ++nbMovingObjects;
                    //break;
                }
                ++nbMovingObjects;
            }

            objects.Add(obj);

//            //Original ohne code
//            switch(type)
//            {
//                case ThingType.OBJECT_CHARACTER:
//                    {
//                       
//                    } // no break!
//                case ThingType.OBJECT_MONSTER:
//                case ThingType.OBJECT_NPC:
//                    {
//              
//                    } // no break!
//                default:
//                    {
//                    }
//            }
        }

        public void remove(Actor obj)
        {
			////List< Actor >::iterator i_beg = objects.begin(), i, i_end;

			//ThingType type = obj.getType();

			//switch (type)
			//{
			//    case ThingType.OBJECT_CHARACTER:
			//    {
			//        i = i_beg;
			//        i_end = objects.begin() + nbCharacters;
			//    } break;
			//    case ThingType.OBJECT_MONSTER:
			//    case ThingType.OBJECT_NPC:
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
			////assert(i != i_end);
			//uint pos = i - i_beg;

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
