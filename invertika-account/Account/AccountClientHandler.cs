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
			Logger.Add(LogLevel.Information, "Account handler started:");

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