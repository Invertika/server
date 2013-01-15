//
//  Thing.cs
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
    public class Thing
    {
        protected List<EventListener> mListeners;   /**< List of event listeners. */

        MapComposite mMap;     /**< Map the thing is on */
        ThingType mType;        /**< Type of this thing. */
		
        public  Thing(ThingType type)//, MapComposite *map = 0)
        {
            mMap=null;
            mType=type;
        }

        public Thing(ThingType type, MapComposite map)
        {
            mMap=map;
            mType=type;
        }

        /**
         * Updates the internal status.
         */
        public virtual void update()
        {
            throw new NotImplementedException("These function must be overloaded from derived class.");
        }

        /**
         * Returns whether this thing is visible on the map or not. (Actor)
         */
        public bool isVisible()
        {
            return mType!=ThingType.OBJECT_OTHER;
        }

        ~Thing()
        {
//    /* As another object will stop listening and call removeListener when it is
//       deleted, the following assertion ensures that all the calls to
//       removeListener have been performed will this object was still alive. It
//       is not strictly necessary, as there are cases where no removal is
//       performed (e.g. ~SpawnArea). But this is rather exceptional, so keep the
//       assertion to catch all the other forgotten calls to removeListener. */
//    assert(mListeners.empty());
        }

        void addListener(EventListener one)
        {
            mListeners.Add(one);
        }

        void removeListener(EventListener one)
        {
            mListeners.Remove(one);
        }

        public void inserted()
        {
            int debug=555;
//    for (Listeners::iterator i = mListeners.begin(),
//         i_end = mListeners.end(); i != i_end;)
//    {
//        const EventListener &l = **i;
//        ++i; // In case the listener removes itself from the list on the fly.
//        if (l.dispatch->inserted) l.dispatch->inserted(&l, this);
//    }
        }

        void removed()
        {
//    for (Listeners::iterator i = mListeners.begin(),
//         i_end = mListeners.end(); i != i_end;)
//    {
//        const EventListener &l = **i;
//        ++i; // In case the listener removes itself from the list on the fly.
//        if (l.dispatch->removed) l.dispatch->removed(&l, this);
//    }
        }
		
        /**
         * Gets the map this thing is located on.
         */
        public MapComposite getMap()
        {
            return mMap;
        }
		
        /**
         * Sets the map this thing is located on.
         */
        public virtual void setMap(MapComposite map)
        {
            mMap=map;
        }

        /**
         * Gets type of this thing.
         *
         * @return the type of this thing.
         */
        public ThingType getType()
        {
            return mType;
        }
    }
}
