//
//  FloorItem.cs
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

namespace invertika_account.Account
{
	public class FloorItem
	{
		int mItemId;
		int mItemAmount;
		int mPosX;
		int mPosY;

		public FloorItem()
		{ }

		public FloorItem(int itemId, int itemAmount, int posX, int posY)
		{
			mItemId=itemId;
			mItemAmount=itemAmount;
			mPosX=posX;
			mPosY=posY;
		}

		/**
		 * Returns the item id
		 */
		public int getItemId()
		{
			return mItemId;
		}

		/**
		 * Returns the amount of items
		 */
		public int getItemAmount()
		{
			return mItemAmount;
		}

		/**
		 * Returns the position x of the item(s)
		 */
		public int getPosX()
		{
			return mPosX;
		}

		/**
		 * Returns the position x of the item(s)
		 */
		public int getPosY()
		{
			return mPosY;
		}
	}
}
