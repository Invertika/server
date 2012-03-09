using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
	 * Iterates through the Characters of a region.
	 */
	public class CharacterIterator
	{
		ZoneIterator iterator;
		ushort pos;
		Character current;

		//CharacterIterator(const ZoneIterator &);
		//void operator++();
		//Character *operator*() const { return current; }
		//operator bool() const { return iterator; }

		//        CharacterIterator::CharacterIterator(const ZoneIterator &it)
		//  : iterator(it), pos(0)
		//{
		//    while (iterator && (*iterator)->nbCharacters == 0) ++iterator;
		//    if (iterator)
		//    {
		//        current = static_cast< Character * >((*iterator)->objects[pos]);
		//    }
		//}

		//void CharacterIterator::operator++()
		//{
		//    if (++pos == (*iterator)->nbCharacters)
		//    {
		//        do ++iterator; while (iterator && (*iterator)->nbCharacters == 0);
		//        pos = 0;
		//    }
		//    if (iterator)
		//    {
		//        current = static_cast< Character * >((*iterator)->objects[pos]);
		//    }
		//}
	}
}
