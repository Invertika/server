//
//  AutoAttacks.cs
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

namespace invertika_game.Game
{
/**
 * Helper class for storing multiple auto-attacks.
 */
	public class AutoAttacks
	{
		//public:
		//    /**
		//     * Whether the being has at least one auto attack that is ready.
		//     */
		//    void add(const AutoAttack &);
		//    void clear(); // Wipe the list completely (used in place of remove for now; FIXME)
		//    void start();
		//    void stop(); // If the character does some action other than attacking, reset all warmups (NOT cooldowns!)
		//    void tick(std::list<AutoAttack> *ret = 0);

		//    /**
		//     * Tells the number of attacks available
		//     */
		//    unsigned getAutoAttacksNumber()
		//    { return mAutoAttacks.size(); }

		//    /**
		//     * Tells whether the autoattacks are active.
		//     */
		//    bool areActive()
		//    { return mActive; }

		//private:
		//    /**
		//     * Marks whether or not to keep auto-attacking. Cooldowns still need
		//     * to be processed when false.
		//     */
		//    bool mActive;
		//    std::list<AutoAttack> mAutoAttacks;
	}
}
