using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using ISL.Server.Network;

namespace invertika_account.Account
{
	public class AccountClient : NetComputer
	{
		/** Account associated with connection */
		Account mAccount;

		public AccountClientStatus status;

		public AccountClient(TcpClient peer):base(peer)
		{
					 //NetComputer(peer);
					 status=AccountClientStatus.CLIENT_LOGIN;
			//mAccount(NULL)
		}

		~AccountClient()
		{
			//unsetAccount();
		}

		void setAccount(Account acc)
		{
			//unsetAccount();
			//mAccount = acc;
		}

		void unsetAccount()
		{
			//delete mAccount;
			//mAccount = NULL;
		}
	}
}