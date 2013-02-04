//
//  MapContent.cs
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

namespace invertika_game.Game
{
    /**
 * Entities on a map.
 */
    public class MapContent
    {
        const int zoneDiam=256;

        //MapContent(Map *);
        //~MapContent();

        ///**
        // * Allocates a unique ID for an actor on this map.
        // */
        //bool allocate(Actor *);

        ///**
        // * Deallocates an ID.
        // */
        //void deallocate(Actor *);

        ///**
        // * Fills a region of zones within the range of a point.
        // */
        //void fillRegion(MapRegion &, const Point &, int) const;

        ///**
        // * Fills a region of zones inside a rectangle.
        // */
        //void fillRegion(MapRegion &, const Rectangle &) const;

        ///**
        // * Things (items, characters, monsters, etc) located on the map.
        // */
        public List< Thing  > things;

        ///**
        // * Buckets of MovingObjects located on the map, referenced by ID.
        // */
        ObjectBucket[] buckets=new ObjectBucket[256];

        int last_bucket; /**< Last bucket acted upon. */

        ///**
        // * Partition of the Objects, depending on their position on the map.
        // */
        MapZone zones;

        ushort mapWidth;  /**< Width with respect to zones. */
        ushort mapHeight; /**< Height with respect to zones. */


        //MapContent::MapContent(Map *map)
        //  : last_bucket(0), zones(NULL)
        //{
        //    buckets[0] = new ObjectBucket;
        //    buckets[0].allocate(); // Skip ID 0
        //    for (int i = 1; i < 256; ++i)
        //    {
        //        buckets[i] = NULL;
        //    }
        //    mapWidth = (map.getWidth() * map.getTileWidth() + zoneDiam - 1)
        //               / zoneDiam;
        //    mapHeight = (map.getHeight() * map.getTileHeight() + zoneDiam - 1)
        //                / zoneDiam;
        //    zones = new MapZone[mapWidth * mapHeight];
        //}

        //MapContent::~MapContent()
        //{
        //    for (int i = 0; i < 256; ++i)
        //    {
        //        delete buckets[i];
        //    }
        //    delete[] zones;
        //}

        public bool allocate(Actor obj)
        {
            // First, try allocating from the last used bucket.
            ObjectBucket b=buckets[last_bucket];
            int i=b.allocate();
            if(i>=0)
            {
                b.objects[i]=obj;
                obj.setPublicID(last_bucket*256+i);
                return true;
            }

            /* If the last used bucket is already full, scan all the buckets for an
               empty place. If none is available, create a new bucket. */
            for(i = 0;i < 256;++i)
            {
                b=buckets[i];
                if(b!=null)
                {
                    /* Buckets are created in order. If there is nothing at position i,
                       there will not be anything in the next positions. So create a
                       new bucket. */
                    b=new ObjectBucket();
                    buckets[i]=b;
                    Logger.Write(LogLevel.Debug, "New bucket created");
                }
                int j=b.allocate();
                if(j>=0)
                {
                    last_bucket=i;
                    b.objects[j]=obj;
                    obj.setPublicID(last_bucket*256+j);
                    return true;
                }
            }

            // All the IDs are currently used, fail.
            Logger.Write(LogLevel.Error, "unable to allocate id");
            return false;
        }

        //void MapContent::deallocate(Actor *obj)
        //{
        //    unsigned short id = obj.getPublicID();
        //    buckets[id / 256].deallocate(id % 256);
        //}

        //void MapContent::fillRegion(MapRegion &r, const Point &p, int radius) const
        //{
        //    int ax = p.x > radius ? (p.x - radius) / zoneDiam : 0,
        //        ay = p.y > radius ? (p.y - radius) / zoneDiam : 0,
        //        bx = std::min((p.x + radius) / zoneDiam, mapWidth - 1),
        //        by = std::min((p.y + radius) / zoneDiam, mapHeight - 1);
        //    for (int y = ay; y <= by; ++y)
        //    {
        //        for (int x = ax; x <= bx; ++x)
        //        {
        //            addZone(r, x + y * mapWidth);
        //        }
        //    }
        //}

        //void MapContent::fillRegion(MapRegion &r, const Rectangle &p) const
        //{
        //    int ax = p.x / zoneDiam,
        //        ay = p.y / zoneDiam,
        //        bx = std::min((p.x + p.w) / zoneDiam, mapWidth - 1),
        //        by = std::min((p.y + p.h) / zoneDiam, mapHeight - 1);
        //    for (int y = ay; y <= by; ++y)
        //    {
        //        for (int x = ax; x <= bx; ++x)
        //        {
        //            addZone(r, x + y * mapWidth);
        //        }
        //    }
        //}

        public MapZone getZone(Point pos)
        {
            int debug=555;
            return null; //ssk
            //return zones[(pos.x/zoneDiam)+(pos.y/zoneDiam)*mapWidth];
        }
    }
}
