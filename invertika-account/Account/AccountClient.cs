//
//  AccountClient.cs
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
using System.Net.Sockets;
using ISL.Server.Network;
using ISL.Server.Account;

namespace invertika_account.Account
{
	public class AccountClient : NetComputer
	{
		/** Account associated with connection */
		ISL.Server.Account.Account mAccount;

		public AccountClientStatus status;

		public AccountClient(TcpClient peer)
			: base(peer)
		{
			status=AccountClientStatus.CLIENT_LOGIN;
			mAccount=null;
		}

		~AccountClient()
		{
			unsetAccount();
		}

		public void setAccount(ISL.Server.Account.Account acc)
		{
			unsetAccount();
			mAccount=acc;
		}

		void unsetAccount()
		{
			mAccount=null;
		}

		/**
 * Get account associated with the connection.
 */
		public ISL.Server.Account.Account getAccount()
		{ return mAccount; }
	}
}
