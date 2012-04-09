//
//  Letter.cs
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
using invertika_account.Common;
using invertika_account.Account;
using ISL.Server.Common;
using ISL.Server.Account;

namespace invertika_account.Chat
{
	public class Letter
	{
		uint mId;
		uint mType;
		ulong mExpiry;
		string mContents;
		List<InventoryItem> mAttachments;
		Character mSender;
		Character mReceiver;

		public Letter(uint type, Character sender, Character receiver)
		{
			mId=0;
			mType=type;
			mSender=sender;
			mReceiver=receiver;
		}

		void setExpiry(ulong expiry)
		{
			mExpiry=expiry;
		}

		ulong getExpiry()
		{
			return mExpiry;
		}

		public void addText(string text)
		{
			mContents=text;
		}

		public string getContents()
		{
			return mContents;
		}

		public bool addAttachment(InventoryItem item)
		{
			uint max=(uint)Configuration.getValue("mail_maxAttachments", 3);
			if(mAttachments.Count>max)
			{
				return false;
			}

			mAttachments.Add(item);

			return true;
		}

		Character getReceiver()
		{
			return mReceiver;
		}

		public Character getSender()
		{
			return mSender;
		}

		public List<InventoryItem> getAttachments()
		{
			return mAttachments;
		}
	}
}
