//
//  ObjectBucket.cs
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
using ISL.Server.Utilities;

namespace invertika_game.Game
{
    /**
	 * Pool of public IDs for MovingObjects on a map. By maintaining public ID
	 * availability using bits, it can locate an available public ID fast while
	 * using minimal memory access.
	 */
    public class ObjectBucket
    {
        static readonly int int_bitsize=32; //int * 8
        uint[] bitmap=new uint[256/int_bitsize]; /**< Bitmap of free locations. */
        short free;                         /**< Number of empty places. */
        short next_object;                  /**< Next object to look at. */
        public Actor[] objects=new Actor[256];

        public ObjectBucket()
        {
            free=256;
            next_object=0;

            for(uint i = 0;i < 256 / int_bitsize;++i)
            {
                // An occupied ID is represented by zero in the bitmap.
                bitmap[i]=~0u;
            }
        }

        public int allocate()
        {
            // Any free ID in the bucket?
            if(free==0)
            {
                Logger.Write(LogLevel.Information, "No free id in bucket");
                return -1;
            }

            int freeBucket=-1;
            // See if the the next_object bucket is free
            if(bitmap[next_object]!=0)
            {
                freeBucket=next_object;
            }
            else
            {
                /* next_object was not free. Check the whole bucket until one ID is found,
		           starting from the IDs around next_object. */
                for(uint i = 0;i < 256 / int_bitsize;++i)
                {
                    // Check to see if this subbucket is free
                    if(bitmap[i]!=0)
                    {
                        freeBucket=(int)i;
                        break;
                    }
                }
            }

            //assert(freeBucket>=0);

            // One of them is free. Find it by looking bit-by-bit.
            int b=(int)bitmap[freeBucket];
            int j=0;

            while(0!=(b & 1))
            //while(!(b & 1)) //TODO Check ob Implementation richtig ist
            {
                b>>=1;
                ++j;
            }
            // Flip that bit to on, and return the value
            bitmap[freeBucket]&=~(uint)(1<<j);
            j+=freeBucket*int_bitsize;
            next_object=(short)freeBucket;
            --free;
            return j;
        }

        void deallocate(int i)
        {
            //assert(!(bitmap[i / int_bitsize] & (1 << (i % int_bitsize))));
            bitmap[i/int_bitsize]|=(uint)(1<<(i%int_bitsize));
            ++free;
        }
    }
}
