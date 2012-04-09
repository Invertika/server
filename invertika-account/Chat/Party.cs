//
//  Party.cs
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
using ISL.Server.Common;
using ISL.Server.Network;

namespace invertika_account.Chat
{
	public class Party
	{
		static int id=0;
		List<string> mUsers;
		uint mId;

		public Party()
		{
			id++;
			mId=(uint)id;
		}

		public void addUser(string name, string inviter)
		{
			mUsers.Add(name);

			for(int i=0; i<userCount(); ++i)
			{
				MessageOut outmsg=new MessageOut(Protocol.CPMSG_PARTY_NEW_MEMBER);
				outmsg.writeString(name);
				outmsg.writeString(inviter);
				Program.chatHandler.getClient(mUsers[i]).send(outmsg);
			}
		}

		public void removeUser(string name)
		{
			mUsers.Remove(name);
		}

		public List<string> getUsers()
		{
			return mUsers;
		}

		/// <summary>
		/// Return the party id
		/// </summary>
		/// <returns></returns>
		public uint getId()
		{
			return mId;
		}

		/// <summary>
		/// Return number of users in party
		/// </summary>
		/// <returns></returns>
		public int userCount()
		{
			return mUsers.Count;
		}
	}
}
