using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Common;
using CSCL.Database.MySQL;
using ISL.Server.Utilities;
using System.Data;

namespace invertika_account.DAL
{
	public class MySQLDataProvider : DataProvider
	{
		//const string CFGPARAM_SQLITE_DB="sqlite_database";
		//const string CFGPARAM_SQLITE_DB_DEF="mana.db";

		//SQLite mDb;
		//CSCL.Database.SQLite.Sqlite3.sqlite3 mDbsqlite3; /**< the handle to the database connection */
		//sqlite3_stmt mStmt; /**< the prepared statement to process */
		MySQL mDb;

		public MySQLDataProvider()
		//throw()
		//    : mDb(0)
		{
		}

		~MySQLDataProvider()
		//throw()
		{
			//try
			//{
			//    // make sure that the database is closed.
			//    // disconnect() calls sqlite3_close() which takes care of freeing
			//    // the memory allocated for the handle.
			//    if (mIsConnected)
			//        disconnect();
			//}
			//catch (...)
			//{
			//    // ignore
			//}
		}

		/**
		 * Get the name of the database backend.
		 */
		DbBackends getDbBackend()
		{
			return DbBackends.DB_BKEND_SQLITE;
		}

		/**
		 * Create a connection to the database.
		 */
		public override void connect()
		{
			//<option name="mysql_hostname" value="invertika.org"/>
			//<option name="mysql_port" value="3306"/>
			//<option name="mysql_database" value="testdb"/>
			//<option name="mysql_username" value="nutzer"/>
			//<option name="mysql_password" value="geheim"/>

			// get configuration parameter for sqlite
			string hostname=Configuration.getValue("mysql_hostname", "");
			string port=Configuration.getValue("mysql_port", "");
			string dbName=Configuration.getValue("mysql_database", "");
			string username=Configuration.getValue("mysql_username", "");
			string password=Configuration.getValue("mysql_password", "");

			Logger.Add(LogLevel.Information, "Trying to connect with MySQL database '{0}'", dbName);

			mDb=new MySQL();
			mDb.Connect(hostname, dbName, username, password);

			// Save the Db Name.
			mDbName=dbName;

			mIsConnected=true;
			Logger.Add(LogLevel.Information, "Connection to database successful.", dbName);
		}

		/**
		 * Execute a SQL query.
		 */
		public override DataTable execSql(string sql, bool refresh)
		{
			if(!mIsConnected) throw new Exception("not connected to database");

			Logger.Add(LogLevel.Debug, "Performing SQL query: {0}", sql);

			DataTable dt=mDb.ExecuteQuery(sql);
			return dt;

			//// do something only if the query is different from the previous
			//// or if the cache must be refreshed
			//// otherwise just return the recordset from cache.
			//if (refresh || (sql != mSql))
			//{
			//    mRecordSet.clear();

			//    //char** result;
			//    //int nRows;
			//    //int nCols;
			//    //char* errMsg;

			//    //mRecordSet.clear();

			//    //int errCode = sqlite3_get_table(
			//    //                  mDb,          // an open database
			//    //                  sql.c_str(),  // SQL to be executed
			//    //                  &result,      // result of the query
			//    //                  &nRows,       // number of result rows
			//    //                  &nCols,       // number of result columns
			//    //                  &errMsg       // error msg
			//    //              );

			//    //if (errCode != SQLITE_OK)
			//    //{
			//    //    std::string msg(sqlite3_errmsg(mDb));

			//    //    LOG_ERROR("Error in SQL: " << sql << "\n" << msg);

			//    //    // free memory
			//    //    sqlite3_free_table(result);
			//    //    sqlite3_free(errMsg);

			//    //    throw DbSqlQueryExecFailure(msg);
			//    //}

			//    //// the first row of result[] contains the field names.
			//    //Row fieldNames;
			//    //for (int col = 0; col < nCols; ++col)
			//    //    fieldNames.push_back(result[col]);

			//    //mRecordSet.setColumnHeaders(fieldNames);

			//    //// populate the RecordSet
			//    //for (int row = 0; row < nRows; ++row)
			//    //{
			//    //    Row r;

			//    //    for (int col = 0; col < nCols; ++col)
			//    //        r.push_back(result[nCols + (row * nCols) + col]);

			//    //    mRecordSet.add(r);
			//    //}

			//    //// free memory
			//    //sqlite3_free_table(result);
			//    //sqlite3_free(errMsg);
			//}

			//return mRecordSet;
		}

		/**
		 * Close the connection to the database.
		 */
		public override void disconnect()
		{
			//if (!isConnected())
			//    return;

			//// sqlite3_close() closes the connection and deallocates the connection
			//// handle.
			//if (sqlite3_close(mDb) != SQLITE_OK)
			//    throw DbDisconnectionFailure(sqlite3_errmsg(mDb));

			//mDb = 0;
			//mIsConnected = false;
		}

		void beginTransaction()
		// throw (std::runtime_error)
		{
			//if (!mIsConnected)
			//{
			//    const std::string error = "Trying to begin a transaction while not "
			//        "connected to the database!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//if (inTransaction())
			//{
			//    const std::string error = "Trying to begin a transaction while anoter "
			//        "one is still open!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//// trying to open a transaction
			//try
			//{
			//    execSql("BEGIN TRANSACTION;");
			//    LOG_DEBUG("SQL: started transaction");
			//}
			//catch (const DbSqlQueryExecFailure &e)
			//{
			//    std::ostringstream error;
			//    error << "SQL ERROR while trying to start a transaction: " << e.what();
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error.str());
			//}
		}

		void commitTransaction()
		//throw (std::runtime_error)
		{
			//if (!mIsConnected)
			//{
			//    const std::string error = "Trying to commit a transaction while not "
			//        "connected to the database!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//if (!inTransaction())
			//{
			//    const std::string error = "Trying to commit a transaction while no "
			//        "one is open!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//// trying to commit a transaction
			//try
			//{
			//    execSql("COMMIT TRANSACTION;");
			//    LOG_DEBUG("SQL: commited transaction");
			//}
			//catch (const DbSqlQueryExecFailure &e)
			//{
			//    std::ostringstream error;
			//    error << "SQL ERROR while trying to commit a transaction: " << e.what();
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error.str());
			//}
		}

		void rollbackTransaction()
		//throw (std::runtime_error)
		{
			//if (!mIsConnected)
			//{
			//    const std::string error = "Trying to rollback a transaction while not "
			//        "connected to the database!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//if (!inTransaction())
			//{
			//    const std::string error = "Trying to rollback a transaction while no "
			//        "one is open!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//// trying to rollback a transaction
			//try
			//{
			//    execSql("ROLLBACK TRANSACTION;");
			//    LOG_DEBUG("SQL: transaction rolled back");
			//}
			//catch (const DbSqlQueryExecFailure &e)
			//{
			//    std::ostringstream error;
			//    error << "SQL ERROR while trying to rollback a transaction: " << e.what();
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error.str());
			//}
		}

		public override uint getModifiedRows()
		{
			if(!mIsConnected)
			{
				string error="Trying to getModifiedRows while not connected to the database!";
				Logger.Add(LogLevel.Error, error); //TODO Unterscheidung beim og zwischen Fatal und Error
				throw new Exception();
			}
			throw new Exception();
			//return (uint)Sqlite3.sqlite3_changes(mDbsqlite3);
			//return (uint) sqlite3_changes(mDb);
		}

		bool inTransaction()
		{
			//if (!mIsConnected)
			//{
			//    const std::string error = "not connected to the database!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//// The sqlite3_get_autocommit() interface returns non-zero or zero if the
			//// given database connection is or is not in autocommit mode, respectively.
			//// Autocommit mode is on by default. Autocommit mode is disabled by a BEGIN
			//// statement. Autocommit mode is re-enabled by a COMMIT or ROLLBACK.
			//const int ret = sqlite3_get_autocommit(mDb);
			//return ret == 0;

			return false; //ssk
		}

		uint getLastId()
		{
			//if (!mIsConnected)
			//{
			//    const std::string error = "not connected to the database!";
			//    LOG_ERROR(error);
			//    throw std::runtime_error(error);
			//}

			//// FIXME: not sure if this is correct to bring 64bit int into int?
			//const sqlite3_int64 lastId = sqlite3_last_insert_rowid(mDb);
			//if (lastId > UINT_MAX)
			//    throw std::runtime_error("SqLiteDataProvider::getLastId exceeded INT_MAX");

			//return (unsigned) lastId;

			return 0; //ssk;
		}

		public override bool prepareSql(string sql)
		{
			throw new NotImplementedException();
			//if(!mIsConnected) return false;

			//Logger.Add(LogLevel.Debug, "Preparing SQL statement: {0}", sql);

			//mRecordSet.clear();

			//Sqlite3.sqlite3_prepare_v2(mDbsqlite3, sql, sql.Length, ref mStmt, 0); //TODO wetermacen
			////if(sqlite3_prepare_v2(mDb, sql, sql.Length, &mStmt, null)!=SQLITE_OK)
			//{
			//    return false;
			//}

			//return true;
		}

		public override DataTable processSql()
		{
			//if (!mIsConnected)
			//{
			//    throw new Exception("not connected to database");
			//        //std::runtime_error("not connected to database");
			//}

			//int totalCols = Sqlite3.sqlite3_column_count(mStmt);
			//List<string> fieldNames=new List<string>();

			//while (Sqlite3.sqlite3_step(mStmt) == Sqlite3.SQLITE_ROW)
			//{
			//    List<string> r=new List<string>();
			//    for (int col = 0; col < totalCols; ++col)
			//    {
			//        fieldNames.Add(Sqlite3.sqlite3_column_name(mStmt, col));
			//        string txt = Sqlite3.sqlite3_column_text(mStmt, col);
			//        r.Add(txt!=null ? txt : ""); //TODO überprüfen

			//    }
			//    // ensure we set column headers before adding a row
			//    mRecordSet.setColumnHeaders(fieldNames);
			//    mRecordSet.add(r);
			//}



			//Sqlite3.sqlite3_finalize(ref mStmt);

			//return mRecordSet;

			throw new NotImplementedException();
		}

		public override void bindValue(int place, string value)
		{
			throw new Exception();
			//Sqlite3.sqlite3_bind_text(mStmt, place, value, value.Length, Sqlite3.SQLITE_STATIC);
		}

		public override void bindValue(int place, int value)
		{
			throw new Exception();
			//Sqlite3.sqlite3_bind_int(mStmt, place, value);
		}
	}
}