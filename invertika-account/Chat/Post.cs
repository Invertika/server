using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Common;

namespace invertika_account.Chat
{
	public class Post
	{
		List<Letter> mLetters;

		~Post()
		{
			//std::vector<Letter*>::iterator itr_end = mLetters.end();
			//for (std::vector<Letter*>::iterator itr = mLetters.begin();
			//     itr != itr_end;
			//     ++itr)
			//{
			//    delete (*itr);
			//}

			//mLetters.clear();
		}

		bool addLetter(Letter letter)
		{
			uint max=(uint)Configuration.getValue("mail_maxLetters", 10);
			if(mLetters.Count>max)
			{
				return false;
			}

			mLetters.Add(letter);

			return true;
		}

		public Letter getLetter(int letter)
		{
			if(letter<0||(UInt64)letter>(UInt64)mLetters.Count)
			{
				return null;
			}
			return mLetters[letter];
		}

		public uint getNumberOfLetters()
		{
			return (uint)mLetters.Count;
		}
	}
}
