using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
	 * Iterates through the zones of a region of the map.
	 */
	class ZoneIterator
	{
		List<uint> region; /**< Zones to visit. Empty means the entire map. */
		uint pos;
		MapZone current;
		MapContent map;

		//ZoneIterator(const List<uint> &, const MapContent);
		//void operator++();
		//MapZone *operator*() const { return current; }
		//operator bool() const { return current; }


		//ZoneIterator::ZoneIterator(const MapRegion &r, const MapContent *m)
		//  : region(r), pos(0), map(m)
		//{
		//    current = &map->zones[r.empty() ? 0 : r[0]];
		//}

		//void ZoneIterator::operator++()
		//{
		//    current = NULL;
		//    if (!region.empty())
		//    {
		//        if (++pos != region.size())
		//        {
		//            current = &map->zones[region[pos]];
		//        }
		//    }
		//    else
		//    {
		//        if (++pos != (unsigned)map->mapWidth * map->mapHeight)
		//        {
		//            current = &map->zones[pos];
		//        }
		//    }
		//}
	}
}
