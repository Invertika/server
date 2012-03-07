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
