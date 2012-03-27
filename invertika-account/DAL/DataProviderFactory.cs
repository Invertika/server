//
//  DataProviderFactory.cs
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
using CSCL.Database;
using CSCL.Database.MySQL;
using CSCL.Database.SQLite;

namespace invertika_account.DAL
{
	public class DataProviderFactory
	{
		/**
		 * Create a data provider.
		 */
		public static Database createDataProvider()
		{
			//<option name="db_system" value="mysql"/>
			string dbsystem=Configuration.getValue("db_system", "mysql");

			switch(dbsystem.ToLower())
			{
				case "sqlite":
					{
						SQLite provider=new SQLite();
						return provider;
					}
				case "mysql":
					{
						string host=Configuration.getValue("mysql_hostname", "example.org");
						int port=Convert.ToInt32(Configuration.getValue("mysql_port", "3306"));
						string database=Configuration.getValue("mysql_database", "");
						string username=Configuration.getValue("mysql_username", "");
						string password=Configuration.getValue("mysql_password", "");

						MySQL provider=new MySQL(host, port, database, username, password);
						return provider;
					}
				default:
					{
						throw new Exception();
					}
			}
		}
	}
}
