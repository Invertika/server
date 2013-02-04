//  
//  Map.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2012 seeseekey
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
using invertika_game.Enums;

namespace invertika_game.Game
{
    public class Map
    {
        /**
         * Blockmasks for different entities
         */
        const byte BLOCKMASK_WALL=0x80;     // = bin 1000 0000
        const byte BLOCKMASK_CHARACTER=0x01;// = bin 0000 0001
        const byte BLOCKMASK_MONSTER=0x02;  // = bin 0000 0010

        // map properties
        int mWidth, mHeight;
        int mTileWidth, mTileHeight;
        Dictionary<string, string> mProperties;
        List<MetaTile> mMetaTiles;
        List<MapObject> mMapObjects;
			
        public Map(int width, int height, int tileWidth, int tileHeight)
        {
            mWidth=width;
            mHeight=height;
            mTileWidth=tileWidth;
            mTileHeight=tileHeight;
            mMetaTiles=new List<MetaTile>(); //    mMetaTiles(width * height)   

            mProperties=new Dictionary<string, string>();
        }

        /**
        * Sets a map property
        */
        public void setProperty(string key, string val)
        { 
            if(mProperties.ContainsKey(key))
            {
                mProperties[key]=val; 
            }
            else
            {
                mProperties.Add(key, val);
            }
        }

        /**
         * Returns the objects of the map.
         */
        public List<MapObject> getObjects()
        {
            return mMapObjects;
        }

        ~Map()
        {
//    for (std::vector<MapObject*>::iterator it = mMapObjects.begin();
//         it != mMapObjects.end(); ++it)
//    {
//        delete *it;
//    }
        }

        /**
         * Returns the width of this map.
         */
        public int getWidth()
        {
            return mWidth;
        }

        /**
         * Returns the height of this map.
         */
        public int getHeight()
        {
            return mHeight;
        }

        void setSize(int width, int height)
        {
            mWidth=width;
            mHeight=height;

            //mMetaTiles.resize(width*height);
        }

        public string getProperty(string key)
        {
            if(mProperties.ContainsKey(key))
            {
                return mProperties[key];
            }

            return "";
        }

        public void blockTile(int x, int y, BlockType type)
        {
//    if (type == BLOCKTYPE_NONE || !contains(x, y))
//        return;
//
//    MetaTile &metaTile = mMetaTiles[x + y * mWidth];
//
//    if (metaTile.occupation[type] < UINT_MAX &&
//        (++metaTile.occupation[type]) > 0)
//    {
//        switch (type)
//        {
//            case BLOCKTYPE_WALL:
//                metaTile.blockmask |= BLOCKMASK_WALL;
//                break;
//            case BLOCKTYPE_CHARACTER:
//                metaTile.blockmask |= BLOCKMASK_CHARACTER;
//                break;
//            case BLOCKTYPE_MONSTER:
//                metaTile.blockmask |= BLOCKMASK_MONSTER;
//                break;
//            default:
//                // Nothing to do.
//                break;
//        }
//    }
        }

        public void freeTile(int x, int y, BlockType type)
        {
//    if (type == BLOCKTYPE_NONE || !contains(x, y))
//        return;
//
//    MetaTile &metaTile = mMetaTiles[x + y * mWidth];
//    assert(metaTile.occupation[type] > 0);
//
//    if (!(--metaTile.occupation[type]))
//    {
//        switch (type)
//        {
//            case BLOCKTYPE_WALL:
//                metaTile.blockmask &= (BLOCKMASK_WALL xor 0xff);
//                break;
//            case BLOCKTYPE_CHARACTER:
//                metaTile.blockmask &= (BLOCKMASK_CHARACTER xor 0xff);
//                break;
//            case BLOCKTYPE_MONSTER:
//                metaTile.blockmask &= (BLOCKMASK_MONSTER xor 0xff);
//                break;
//            default:
//                // nothing
//                break;
//        }
//    }
        }

        public bool getWalk(int x, int y, byte walkmask)
        {
//			// You can't walk outside of the map
//			if(!contains(x, y))
//				return false;
//
//			// Check if the tile is walkable
//			return !(mMetaTiles[x+y*mWidth].blockmask&walkmask);
		
            return true; //ssk
        }

//		Path findPath(int startX, int startY, int destX, int destY, byte walkmask, int maxCost)
//		{
////    return ::findPath(startX, startY,
////                      destX, destY,
////                      walkmask, maxCost,
////                      this);
//			
//			return null;
//		}
		
        /**
         * Returns the tile width of this map.
         */
        public  int getTileWidth()
        {
            return mTileWidth;
        }

        /**
         * Returns the tile height used by this map.
         */
        public  int getTileHeight()
        {
            return mTileHeight;
        }
    }
}

