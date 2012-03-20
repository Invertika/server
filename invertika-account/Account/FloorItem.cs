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
