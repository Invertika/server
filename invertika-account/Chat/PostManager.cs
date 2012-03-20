using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_account.Account;

namespace invertika_account.Chat
{
	public class PostManager
	{
		Dictionary<Character, Post> mPostBox;

		public void addLetter(Letter letter)
		{
			//std::map<Character*, Post*>::iterator itr =
			//    mPostBox.find(letter->getReceiver());
			//if (itr != mPostBox.end())
			//{
			//    itr->second->addLetter(letter);
			//}
			//else
			//{
			//    Post *post = new Post();
			//    post->addLetter(letter);
			//    mPostBox.insert(
			//        std::pair<Character*, Post*>(letter->getReceiver(), post)
			//        );
			//}
		}

		public Post getPost(Character player)
		{
			//std::map<Character*, Post*>::const_iterator itr = mPostBox.find(player);
			//return (itr == mPostBox.end()) ? NULL : itr->second;

			return null; //ssk
		}

		public void clearPost(Character player)
		{
			//std::map<Character*, Post*>::iterator itr =
			//    mPostBox.find(player);
			//if (itr != mPostBox.end())
			//{
			//    delete itr->second;
			//    mPostBox.erase(itr);
			//}
		}
	}
}
