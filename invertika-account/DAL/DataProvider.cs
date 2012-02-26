using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace invertika_account.DAL
{
	public abstract class DataProvider
	{
		protected string mDbName;  /**< the database name */
		protected bool mIsConnected;    /**< the connection status */
		protected string mSql;     /**< cache the last SQL query */
		protected DataTable mRecordSet; /**< cache the result of the last SQL query */

		public DataProvider()
		{
			mIsConnected=false;
			mRecordSet=new DataTable();
		}

		//virtual ~DataProvider()
		//{
		//}

		/**
		 * Get the connection status.
		 *
		 * @return true if connected.
		 */
		public bool isConnected()
		{
			return mIsConnected;
		}

		/**
		 * Get the database name.
		 */
		string getDbName()
		{
			if(!isConnected()) return "";

			return mDbName;
		}

		public abstract bool prepareSql(string sql);

		        /**
         * Bind Value (String)
         * @param place - which parameter to bind to
         * @param value - the string to bind
         */
        public abstract void bindValue(int place, string value);

        /**
         * Bind Value (Integer)
         * @param place - which parameter to bind to
         * @param value - the integer to bind
         */
        public abstract void bindValue(int place, int value);

		public abstract DataTable execSql(string sql, bool refresh=false);

		public abstract uint getModifiedRows();

		public abstract void connect();

		public abstract void disconnect();

		        /**
         * Process SQL statement
         * SQL statement needs to be prepared and parameters binded before
         * calling this function
         */
        public abstract DataTable processSql();
	}
}
