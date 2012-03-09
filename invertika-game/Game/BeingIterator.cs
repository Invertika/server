using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
 * Iterates through the Beings of a region.
 */
	public class BeingIterator
	{
		//        ZoneIterator iterator;
		//unsigned short pos;
		//Being *current;

		//BeingIterator(const ZoneIterator &);
		//void operator++();
		//Being *operator*() const { return current; }
		//operator bool() const { return iterator; }

		//        BeingIterator::BeingIterator(const ZoneIterator &it)
		//  : iterator(it), pos(0)
		//{
		//    while (iterator && (*iterator)->nbMovingObjects == 0) ++iterator;
		//    if (iterator)
		//    {
		//        current = static_cast< Being * >((*iterator)->objects[pos]);
		//    }
		//}

		//void BeingIterator::operator++()
		//{
		//    if (++pos == (*iterator)->nbMovingObjects)
		//    {
		//        do ++iterator; while (iterator && (*iterator)->nbMovingObjects == 0);
		//        pos = 0;
		//    }
		//    if (iterator)
		//    {
		//        current = static_cast< Being * >((*iterator)->objects[pos]);
		//    }
		//}
	}
}