using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Common;

namespace invertika_account.DAL
{
	public class DataProviderFactory
	{
		/**
		 * Create a data provider.
		 */
		public static DataProvider createDataProvider()
		{
			//<option name="db_system" value="mysql"/>
			string dbsystem=Configuration.getValue("db_system", "mysql");

			switch(dbsystem.ToLower())
			{
				case "sqlite":
					{
						SqLiteDataProvider provider=new SqLiteDataProvider();
						return provider;
					}
				case "mysql":
					{
						MySQLDataProvider provider=new MySQLDataProvider();
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
