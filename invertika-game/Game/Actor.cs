//
//  Actor.cs
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
using ISL.Server.Utilities;
using invertika_game.Enums;
using System.Diagnostics;
using ISL.Server.Common;

namespace invertika_game.Game
{
    public class Actor : Thing
    {
        byte mUpdateFlags;          /**< Changes in actor status. */

        /** Actor ID sent to clients (unique with respect to the map). */
        ushort mPublicID;
        Point mPos=new Point();                 /**< Coordinates. */
        byte mSize;        /**< Radius of bounding circle. */

        //protected
        /**
         * Gets the way the actor blocks pathfinding for other actors.
         */
        protected virtual BlockType getBlockType()
        {
            return BlockType.BLOCKTYPE_NONE;
        }

        /** Delay until move to next tile in miliseconds. */
        ushort mMoveTime;

        public Actor(ThingType type): base(type)
        {
            mMoveTime=0;
            mUpdateFlags=0;
            mPublicID=65535;
            mSize=0;
        }

        ~Actor()
        {
            // Free the map position
            MapComposite mapComposite=getMap();
            if(mapComposite!=null)
            {
                Map map=mapComposite.getMap();
                int tileWidth=map.getTileWidth();
                int tileHeight=map.getTileHeight();
                Point oldP=getPosition();
                map.freeTile(oldP.x/tileWidth, oldP.y/tileHeight, getBlockType());
            }
        }

        /**
         * Set public ID. The actor shall not have any public ID yet.
         */
        public void setPublicID(int id)
        {
            mPublicID=(ushort)id;
        }

        /**
         * Gets the way the actor blocks pathfinding for other actors.
         */
        public virtual byte getWalkMask()
        {
            return 0x00;
        } //can walk through everything

        public void setPosition(Point p)
        {
            // Update blockmap
            MapComposite mapComposite=getMap();

            if(mapComposite!=null)
            {
                Map map=mapComposite.getMap();
                int tileWidth=map.getTileWidth();
                int tileHeight=map.getTileHeight();
                if((mPos.x/tileWidth!=p.x/tileWidth
                    ||mPos.y/tileHeight!=p.y/tileHeight))
                {
                    map.freeTile(mPos.x/tileWidth, mPos.y/tileHeight,
						  getBlockType());
                    map.blockTile(p.x/tileWidth, p.y/tileHeight, getBlockType());
                }
            }

            mPos=p;
        }

        public new void setMap(MapComposite mapComposite)
        {
            Point p=getPosition();

            MapComposite oldMapComposite=getMap();

            if(oldMapComposite!=null)
            {
                Map oldMap=oldMapComposite.getMap();
                int oldTileWidth=oldMap.getTileWidth();
                int oldTileHeight=oldMap.getTileHeight();
                oldMap.freeTile(p.x/oldTileWidth, p.y/oldTileHeight, getBlockType());
            }

            base.setMap(mapComposite); //TODO Überprüfen ob es wie im ORiginal funktioniert
            Map map=mapComposite.getMap();
            int tileWidth=map.getTileWidth();
            int tileHeight=map.getTileHeight();
            map.blockTile(p.x/tileWidth, p.y/tileHeight, getBlockType());
            /* the last line might look illogical because the current position is
     * invalid on the new map, but it is necessary to block the old position
     * because the next call of setPosition() will automatically free the old
     * position. When we don't block the position now the occupation counting
     * will be off.
     */
        }

        /**
		 * Gets the coordinates.
		 *
		 * @return the coordinates.
		 */
        public Point getPosition()
        {
            return mPos;
        }

        /**
         * Sets some changes in the actor.
         */
        public void raiseUpdateFlags(byte n)
        {
            mUpdateFlags|=n;
        }
    }
}
