using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Common;
using invertika_account.Account;
using ISL.Server.Common;

namespace invertika_account.Chat
{
	public class Letter
	{
		uint mId;
		uint mType;
		ulong mExpiry;
		string mContents;
		List<InventoryItem> mAttachments;
		Character mSender;
		Character mReceiver;

		public Letter(uint type, Character sender, Character receiver)
		{
			mId=0;
			mType=type;
			mSender=sender;
			mReceiver=receiver;
		}

		~Letter()
		{
			//if (mSender)
			//    delete mSender;

			//if (mReceiver)
			//    delete mReceiver;
		}

		void setExpiry(ulong expiry)
		{
			mExpiry=expiry;
		}

		ulong getExpiry()
		{
			return mExpiry;
		}

		public void addText(string text)
		{
			mContents=text;
		}

		public string getContents()
		{
			return mContents;
		}

		public bool addAttachment(InventoryItem item)
		{
			uint max=(uint)Configuration.getValue("mail_maxAttachments", 3);
			if(mAttachments.Count>max)
			{
				return false;
			}

			mAttachments.Add(item);

			return true;
		}

		Character getReceiver()
		{
			return mReceiver;
		}

		public Character getSender()
		{
			return mSender;
		}

		public List<InventoryItem> getAttachments()
		{
			return mAttachments;
		}
	}
}
