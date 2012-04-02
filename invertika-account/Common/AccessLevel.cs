//
//  AccessLevel.cs
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

namespace invertika_account.Common
{
	/**
	 * Enumeration type for account levels.
	 * A normal player would have permissions of 1
	 * A tester would have permissions of 3 (AL_PLAYER | AL_TESTER)
	 * A dev would have permissions of 7 (AL_PLAYER | AL_TESTER | AL_DEV)
	 * A gm would have permissions of 11 (AL_PLAYER | AL_TESTER | AL_GM)
	 * A admin would have permissions of 255 (*)
	 */
	public enum AccessLevel
	{
		AL_BANNED=0,     /**< This user is currently banned. */
		AL_PLAYER=1,     /**< User has regular rights. */
		AL_TESTER=2,     /**< User can perform testing tasks. */
		AL_DEV=4,     /**< User is a developer and can perform dev tasks */
		AL_GM=8,     /**< User is a moderator and can perform mod tasks */
		AL_ADMIN=128     /**< User can perform administrator tasks. */
	}
}
