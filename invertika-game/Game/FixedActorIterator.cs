using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
	 * Iterates through the non-moving Actors of a region.
	 */
	public class FixedActorIterator
	{
		//ZoneIterator iterator;
		//unsigned short pos;
		//Actor *current;

		//FixedActorIterator(const ZoneIterator &);
		//void operator++();
		//Actor *operator*() const { return current; }
		//operator bool() const { return iterator; }

		//        FixedActorIterator::FixedActorIterator(const ZoneIterator &it)
		//  : iterator(it), pos(0)
		//{
		//    while (iterator && (*iterator)->nbMovingObjects == (*iterator)->objects.size()) ++iterator;
		//    if (iterator)
		//    {
		//        pos = (*iterator)->nbMovingObjects;
		//        current = (*iterator)->objects[pos];
		//    }
		//}

		//void FixedActorIterator::operator++()
		//{
		//    if (++pos == (*iterator)->objects.size())
		//    {
		//        do ++iterator; while (iterator && (*iterator)->nbMovingObjects == (*iterator)->objects.size());
		//        if (iterator)
		//        {
		//            pos = (*iterator)->nbMovingObjects;
		//        }
		//    }
		//    if (iterator)
		//    {
		//        current = (*iterator)->objects[pos];
		//    }
		//}
	}
}