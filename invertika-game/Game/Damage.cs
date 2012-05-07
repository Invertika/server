//
//  Damage.cs
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
using invertika_game.Common;

namespace invertika_game.Game
{
	/**
	 * Structure that describes the severity and nature of an attack a being can
	 * be hit by.
	 */
	public class Damage
	{
		uint skill;             /**< Skill used by source (needed for exp calculation) */
		ushort @base;            /**< Base amount of damage. */
		ushort delta;           /**< Additional damage when lucky. */
		ushort cth;             /**< Chance to hit. Opposes the evade attribute. */
		byte element;          /**< Elemental damage. */
		DamageType type;                /**< Damage type: Physical or magical? */
		bool trueStrike;                /**< Override dodge calculation */
		ushort range;           /**< Maximum distance that this attack can be used from, in pixels */

		Damage(uint skill, ushort @base, ushort delta, ushort cth, byte element, DamageType type=DamageType.DAMAGE_OTHER, ushort range=DEFAULT_TILE_LENGTH)
		{
			this.skill=skill;
			this.@base=@base;
			this.delta=delta;
			this.cth=cth;
			this.element=element;
			this.type=type;
			this.trueStrike=false;
			this.range=range;
		}
	}
}
