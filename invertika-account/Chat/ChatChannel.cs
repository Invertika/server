using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Chat
{
	public class ChatChannel
	{
		ushort mId;            /**< The ID of the channel. */
		string mName;             /**< The name of the channel. */
		string mAnnouncement;     /**< Welcome message. */
		string mPassword;         /**< The channel password. */
		bool mJoinable;                /**< Whether anyone can join. */
		List<ChatClient> mRegisteredUsers; /**< Users in this channel. */
		string mOwner;             /**< Channel owner character name */

		ChatChannel(int id, string name, string announcement, string password, bool joinable)
		{
			mId=(ushort)id;
			mName=name;
			mAnnouncement=announcement;
			mPassword=password;
			mJoinable=joinable;
		}

		bool addUser(ChatClient user)
		{
			//// First user is the channel owner
			//if (mRegisteredUsers.Count() < 1)
			//{
			//    mOwner = user->characterName;
			//    setUserMode(user, 'o');
			//}

			//// Check if the user already exists in the channel
			//ChannelUsers::const_iterator i = mRegisteredUsers.begin(),
			//                             i_end = mRegisteredUsers.end();
			//if (std::find(i, i_end, user) != i_end)
			//    return false;

			//mRegisteredUsers.push_back(user);
			//user->channels.push_back(this);

			//// set user as logged in
			//setUserMode(user, 'l');

			//// if owner has rejoined, give them ops
			//if (user->characterName == mOwner)
			//{
			//    setUserMode(user, 'o');
			//}

			return true;
		}

		bool removeUser(ChatClient user)
		{
			//ChannelUsers::iterator i_end = mRegisteredUsers.end(),
			//                       i = std::find(mRegisteredUsers.begin(), i_end, user);
			//if (i == i_end) return false;
			//mRegisteredUsers.erase(i);
			//std::vector< ChatChannel * > &channels = user->channels;
			//channels.erase(std::find(channels.begin(), channels.end(), this));
			//std::map<ChatChannel*,std::string> &modes = user->userModes;
			//modes.erase(modes.begin(), modes.end());
			return true;
		}

		void removeAllUsers()
		{
			//for (ChannelUsers::const_iterator i = mRegisteredUsers.begin(),
			//     i_end = mRegisteredUsers.end(); i != i_end; ++i)
			//{
			//    std::vector< ChatChannel * > &channels = (*i)->channels;
			//    channels.erase(std::find(channels.begin(), channels.end(), this));
			//    std::map<ChatChannel*,std::string> &modes = (*i)->userModes;
			//    modes.erase(modes.begin(), modes.end());
			//}
			//mRegisteredUsers.clear();
		}

		bool canJoin()
		{
			return mJoinable;
		}

		void setUserMode(ChatClient user, byte mode)
		{
			//Dictionary<ChatChannel, string>::iterator itr = user->userModes.find(this);
			//if (itr != user->userModes.end())
			//{
			//    itr->second += mode;
			//}
			//else
			//{
			//    std::stringstream ss; ss << mode;
			//    user->userModes.insert(std::pair<ChatChannel*, std::string>(this, ss.str()));
			//}
		}

		string getUserMode(ChatClient user)
		{
			//std::map<ChatChannel*, std::string>::const_iterator itr =
			//        user->userModes.find(const_cast<ChatChannel*>(this));

			//if (itr != user->userModes.end())
			//    return itr->second;

			//return 0;

			return ""; //ssk
		}
	}
}
