using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
 * Entities on a map.
 */
	public class MapContent
	{
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
		// * Gets zone at given position.
		// */
		//MapZone &getZone(const Point &pos) const;

		///**
		// * Things (items, characters, monsters, etc) located on the map.
		// */
		//std::vector< Thing * > things;

		///**
		// * Buckets of MovingObjects located on the map, referenced by ID.
		// */
		//ObjectBucket *buckets[256];

		//int last_bucket; /**< Last bucket acted upon. */

		///**
		// * Partition of the Objects, depending on their position on the map.
		// */
		//MapZone *zones;

		//unsigned short mapWidth;  /**< Width with respect to zones. */
		//unsigned short mapHeight; /**< Height with respect to zones. */


		//MapContent::MapContent(Map *map)
		//  : last_bucket(0), zones(NULL)
		//{
		//    buckets[0] = new ObjectBucket;
		//    buckets[0]->allocate(); // Skip ID 0
		//    for (int i = 1; i < 256; ++i)
		//    {
		//        buckets[i] = NULL;
		//    }
		//    mapWidth = (map->getWidth() * map->getTileWidth() + zoneDiam - 1)
		//               / zoneDiam;
		//    mapHeight = (map->getHeight() * map->getTileHeight() + zoneDiam - 1)
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

		//bool MapContent::allocate(Actor *obj)
		//{
		//    // First, try allocating from the last used bucket.
		//    ObjectBucket *b = buckets[last_bucket];
		//    int i = b->allocate();
		//    if (i >= 0)
		//    {
		//        b->objects[i] = obj;
		//        obj->setPublicID(last_bucket * 256 + i);
		//        return true;
		//    }

		//    /* If the last used bucket is already full, scan all the buckets for an
		//       empty place. If none is available, create a new bucket. */
		//    for (i = 0; i < 256; ++i)
		//    {
		//        b = buckets[i];
		//        if (!b)
		//        {
		//            /* Buckets are created in order. If there is nothing at position i,
		//               there will not be anything in the next positions. So create a
		//               new bucket. */
		//            b = new ObjectBucket;
		//            buckets[i] = b;
		//            LOG_DEBUG("New bucket created");
		//        }
		//        int j = b->allocate();
		//        if (j >= 0)
		//        {
		//            last_bucket = i;
		//            b->objects[j] = obj;
		//            obj->setPublicID(last_bucket * 256 + j);
		//            return true;
		//        }
		//    }

		//    // All the IDs are currently used, fail.
		//    LOG_ERROR("unable to allocate id");
		//    return false;
		//}

		//void MapContent::deallocate(Actor *obj)
		//{
		//    unsigned short id = obj->getPublicID();
		//    buckets[id / 256]->deallocate(id % 256);
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

		//MapZone& MapContent::getZone(const Point &pos) const
		//{
		//    return zones[(pos.x / zoneDiam) + (pos.y / zoneDiam) * mapWidth];
		//}
	}
}