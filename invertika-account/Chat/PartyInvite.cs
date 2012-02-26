using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Chat
{
	public class PartyInvite
	{
		public PartyInvite(string inviterName, string inviteeName)
		{
			mInviter=inviterName;
			mInvitee=inviteeName;
			const int validTimeframe=60;
			mExpireTime=DateTime.Now.Ticks+validTimeframe; //	time(NULL) + validTimeframe;
		}

		string mInviter;
		string mInvitee;
		long mExpireTime;
	}
}