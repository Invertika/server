using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Utilities
{
	public class TokenCollector<Handler, Client, ServerData> : TokenCollectorBase
	{
		//TokenCollector(Handler *h): mHandler(h)
		//{
		//    _TC_CheckData<Client> ClientMustBeSimple;
		//    (void)&ClientMustBeSimple;
		//    _TC_CheckData<ServerData> ServerDataMustBeSimple;
		//    (void)&ServerDataMustBeSimple;
		//}

		/**
		 * Checks if the server expected this client token. If so, calls
		 * Handler::tokenMatched. Otherwise marks the client as pending.
		 */
		public void addPendingClient(string token, Client data)
		{
			//insertClient(token, (intptr_t)data); 
		}

		/**
		 * Checks if a client already registered this token. If so, calls
		 * Handler::tokenMatched. Otherwise marks the data as pending.
		 */
		public void addPendingConnect(string token, ServerData data)
		{
			//insertConnect(token, (intptr_t)data); 
		}

		/**
		 * Removes a pending client.
		 * @note Does not call destroyPendingClient.
		 */
		public void deletePendingClient(Client data)
		{
			//removeClient((intptr_t)data); 
		}

		//private:

		//    void removedClient(intptr_t data)
		//    { mHandler->deletePendingClient((Client)data); }

		//    void removedConnect(intptr_t data)
		//    { mHandler->deletePendingConnect((ServerData)data); }

		//    void foundMatch(intptr_t client, intptr_t data)
		//    { mHandler->tokenMatched((Client)client, (ServerData)data); }

		//    Handler *mHandler;
	}
}
