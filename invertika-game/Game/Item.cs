//  
//  Item.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2012 seeseekey
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
using ISL.Server.Common;

namespace invertika_game.Game
{
	/**
 * Class for an item stack laying on the floor in the game world
 */
	public class Item: Actor
	{
		ItemClass mType;
		int mAmount;
		int mLifetime;
		
		public Item(ItemClass type, int amount) : base (ThingType.OBJECT_ITEM)
		{
			mType=type;
			mAmount=amount;
			mLifetime = Configuration.getValue("game_floorItemDecayTime", 0) * 10;
		}

		void update()
		{
			if(mLifetime>0)
			{
				mLifetime--;
				if(mLifetime<=0)
				{
					GameState.enqueueRemove(this);		
				}
			}
		}

		ItemClass getItemClass()
		{ 
			return mType; 
		}

		int getAmount()
		{ 
			return mAmount; 
		}

		//virtual void update();
	}
}

