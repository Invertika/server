//
//  AccountClientHandler.cs
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
using invertika_account.Utilities;
using ISL.Server.Utilities;

namespace invertika_account.Account
{
	public static class AccountClientHandler
	{
		static AccountHandler accountHandler;

		public static bool initialize(string attributesFile, int port, string host)
		{
			accountHandler=new AccountHandler(attributesFile);
			Logger.Write(LogLevel.Information, "Account handler started:");

			return accountHandler.startListen((ushort)port, host);
		}

		public static void deinitialize()
		{
			//accountHandler->stopListen();
			//delete accountHandler;
			//accountHandler = 0;
		}

		public static void process()
		{
			accountHandler.process(50);
		}

		public static void prepareReconnect(string token, int id)
		{
			accountHandler.mTokenCollector.addPendingConnect(token, id);
		}
	}
}
