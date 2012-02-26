using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Chat
{
	public class Party
	{
		    List<string> mUsers;

    uint mId;

		Party()
{
	//static int id = 0;
	//id++;
	//mId = id;
}

void addUser(string name, string inviter)
{
	//mUsers.push_back(name);

	//for (size_t i = 0; i < userCount(); ++i)
	//{
	//    MessageOut out(ManaServ::CPMSG_PARTY_NEW_MEMBER);
	//    out.writeString(name);
	//    out.writeString(inviter);
	//    chatHandler->getClient(mUsers[i])->send(out);
	//}
}

void removeUser(string name)
{
	//PartyUsers::iterator itr = std::find(mUsers.begin(), mUsers.end(), name);
	//if (itr != mUsers.end())
	//{
	//    mUsers.erase(itr);
	//}
}

	}
}