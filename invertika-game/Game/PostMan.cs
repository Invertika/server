using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class PostMan
	{
		Character getCharacter(int id)
		{
			//std::map<int, Character*>::const_iterator itr = mCharacters.find(id);
			//if (itr != mCharacters.end())
			//    return itr->second;
			//return 0;

			return null; //ssk
		}

		void addCharacter(Character player)
		{
			//std::map<int, Character*>::iterator itr = mCharacters.find(player->getDatabaseID());
			//if (itr == mCharacters.end())
			//{
			//    mCharacters.insert(std::pair<int, Character*>(player->getDatabaseID(), player));
			//}
		}

		void getPost(Character player)//, PostCallback &f)
		{
			//mCallbacks.insert(std::pair<Character*, PostCallback>(player, f));
			//accountHandler->getPost(player);
		}

		void gotPost(Character player, string sender, string letter)
		{
			//std::map<Character*, PostCallback>::iterator itr = mCallbacks.find(player);
			//if (itr != mCallbacks.end())
			//{
			//    itr->second.handler(player, sender, letter, itr->second.data);
			//}
		}

		Dictionary<int, Character> mCharacters;
		//Dictionary<Character, PostCallback> mCallbacks;
	}
}
