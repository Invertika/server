using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
	 * Pool of public IDs for MovingObjects on a map. By maintaining public ID
	 * availability using bits, it can locate an available public ID fast while
	 * using minimal memory access.
	 */
	public class ObjectBucket
	{
		//static int const int_bitsize = sizeof(unsigned) * 8;
		//unsigned bitmap[256 / int_bitsize]; /**< Bitmap of free locations. */
		//short free;                         /**< Number of empty places. */
		//short next_object;                  /**< Next object to look at. */
		//Actor *objects[256];

		//ObjectBucket();
		//int allocate();
		//void deallocate(int);

		//ObjectBucket::ObjectBucket()
		//  : free(256), next_object(0)
		//{
		//    for (unsigned i = 0; i < 256 / int_bitsize; ++i)
		//    {
		//        // An occupied ID is represented by zero in the bitmap.
		//        bitmap[i] = ~0u;
		//    }
		//}

		//int ObjectBucket::allocate()
		//{
		//    // Any free ID in the bucket?
		//    if (!free)
		//    {
		//        LOG_INFO("No free id in bucket");
		//        return -1;
		//    }

		//    int freeBucket = -1;
		//    // See if the the next_object bucket is free
		//    if (bitmap[next_object] != 0)
		//    {
		//        freeBucket = next_object;
		//    }
		//    else
		//    {
		//        /* next_object was not free. Check the whole bucket until one ID is found,
		//           starting from the IDs around next_object. */
		//        for (unsigned i = 0; i < 256 / int_bitsize; ++i)
		//        {
		//            // Check to see if this subbucket is free
		//            if (bitmap[i] != 0)
		//            {
		//                freeBucket = i;
		//                break;
		//            }
		//        }
		//    }

		//    assert(freeBucket >= 0);

		//    // One of them is free. Find it by looking bit-by-bit.
		//    int b = bitmap[freeBucket];
		//    int j = 0;
		//    while (!(b & 1))
		//    {
		//        b >>= 1;
		//        ++j;
		//    }
		//    // Flip that bit to on, and return the value
		//    bitmap[freeBucket] &= ~(1 << j);
		//    j += freeBucket * int_bitsize;
		//    next_object = freeBucket;
		//    --free;
		//    return j;
		//}

		//void ObjectBucket::deallocate(int i)
		//{
		//    assert(!(bitmap[i / int_bitsize] & (1 << (i % int_bitsize))));
		//    bitmap[i / int_bitsize] |= 1 << (i % int_bitsize);
		//    ++free;
		//}
	}
}
