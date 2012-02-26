using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.DAL
{
	public class DataProviderFactory
	{
		/**
		 * Create a data provider.
		 */
		public static DataProvider createDataProvider()
		{
			//#if defined (MYSQL_SUPPORT)
			//    MySqlDataProvider* provider = new MySqlDataProvider;
			//    return provider;
			//#elif defined (POSTGRESQL_SUPPORT)
			//    PqDataProvider *provider = new PqDataProvider;
			//    return provider;
			//#else // SQLITE_SUPPORT
			SqLiteDataProvider provider=new SqLiteDataProvider();
			return provider;
			//#endif
		}
	}
}
