using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Utilities
{
	public class TokenCollectorBase
	{
	//            struct Item
	//    {
	//        std::string token; /**< Cookie used by the client. */
	//        intptr_t data;     /**< User data. */
	//        time_t timeStamp;  /**< Creation time. */
	//    };

	//    /**
	//     * List containing client already connected. Newer clients are at the
	//     * back of the list.
	//     */
	//    std::list<Item> mPendingClients;

	//    /**
	//     * List containing server data waiting for clients. Newer data are at
	//     * the back of the list.
	//     */
	//    std::list<Item> mPendingConnects;

	//    /**
	//     * Time at which the TokenCollector performed its last check.
	//     */
	//    time_t mLastCheck;

	//protected:

	//    virtual void removedClient(intptr_t) = 0;
	//    virtual void removedConnect(intptr_t) = 0;
	//    virtual void foundMatch(intptr_t client, intptr_t connect) = 0;
	//    TokenCollectorBase();
	//    virtual ~TokenCollectorBase();
	//    void insertClient(const std::string &, intptr_t);
	//    void removeClient(intptr_t);
	//    void insertConnect(const std::string &, intptr_t);
	//    void removeOutdated(time_t);

		void insertClient(string token, object data) //intptr_t data)
		{
			//for (std::list<Item>::reverse_iterator it = mPendingConnects.rbegin(),
			//     it_end = mPendingConnects.rend(); it != it_end; ++it)
			//{
			//    if (it->token == token)
			//    {
			//        foundMatch(data, it->data);
			//        mPendingConnects.erase(--it.base());
			//        return;
			//    }
			//}

			//time_t current = time(NULL);

			//Item item;
			//item.token = token;
			//item.data = data;
			//item.timeStamp = current;
			//mPendingClients.push_back(item);

			//removeOutdated(current);
		}

		void insertConnect(string token, object data) //intptr_t data)
		{
			//for (std::list<Item>::reverse_iterator it = mPendingClients.rbegin(),
			//     it_end = mPendingClients.rend(); it != it_end; ++it)
			//{
			//    if (it->token == token)
			//    {
			//        foundMatch(it->data, data);
			//        mPendingClients.erase(--it.base());
			//        return;
			//    }
			//}

			//time_t current = time(NULL);

			//Item item;
			//item.token = token;
			//item.data = data;
			//item.timeStamp = current;
			//mPendingConnects.push_back(item);

			//removeOutdated(current);
		}

		void removeClient(object data) //intptr_t data)
		{
			//for (std::list<Item>::iterator it = mPendingClients.begin(),
			//     it_end = mPendingClients.end(); it != it_end; ++it)
			//{
			//    if (it->data == data)
			//    {
			//        mPendingClients.erase(it);
			//        return;
			//    }
			//}
		}

		void removeOutdated(long current) //time_t
		{
			//// Timeout happens after 30 seconds. Much longer may actually pass, though.
			//time_t threshold = current - 30;
			//if (threshold < mLastCheck) return;

			//std::list<Item>::iterator it;

			//it = mPendingConnects.begin();
			//while (it != mPendingConnects.end() && it->timeStamp < threshold)
			//{
			//    removedConnect(it->data);
			//    it = mPendingConnects.erase(it);
			//}

			//it = mPendingClients.begin();
			//while (it != mPendingClients.end() && it->timeStamp < threshold)
			//{
			//    removedClient(it->data);
			//    it = mPendingClients.erase(it);
			//}

			//mLastCheck = current;
		}

		public TokenCollectorBase()
		{
			//mLastCheck(time(NULL))
		}

		//TokenCollectorBase::~TokenCollectorBase()
		//{
		//    // Not declared inline, as the list destructors are not trivial.
		//}
	}
}
