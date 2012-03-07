using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Account
{
	public class Account
	{
		//        Account(const Account &rhs);
		//Account &operator=(const Account &rhs);

		string mName;        /**< User name */
		string mPassword;    /**< User password (hashed with salt) */
		string mRandomSalt;  /**< A random sequence sent to client to
                                       protect against replay attacks.*/
		string mEmail;       /**< User email address (hashed) */
		Dictionary<uint, Character> mCharacters;   /**< Character data */
		int mID;                  /**< Unique id */
		byte mLevel;     /**< Account level */
		//time_t mRegistrationDate; /**< Date and time of the account registration */
		//time_t mLastLogin;        /**< Date and time of the last login */

		~Account()
		{
			//for (Characters::iterator i = mCharacters.begin(),
			//     i_end = mCharacters.end(); i != i_end; ++i)
			//{
			//    delete (*i).second;
			//}
		}

		/**
 * Get all the characters.
 *
 * @return all the characters.
 */
		public Dictionary<uint, Character> getCharacters()
		{
			return mCharacters;
		}

		bool isSlotEmpty(uint slot)
		{
			//return mCharacters.find(slot) == mCharacters.end();

			return true; //ssk
		}

		void setCharacters(Dictionary<uint, Character> characters)
		{
			//mCharacters = characters;
		}

		void addCharacter(Character character)
		{
			//unsigned int slot = (unsigned int) character->getCharacterSlot();
			//assert(isSlotEmpty(slot));

			//mCharacters[slot] = character;
		}

		void delCharacter(uint slot)
		{
			//for (Characters::iterator iter = mCharacters.begin(),
			//         iter_end = mCharacters.end(); iter != iter_end; ++iter)
			//{
			//    if ((*iter).second->getCharacterSlot() == slot)
			//    {
			//        delete (*iter).second;
			//        (*iter).second = 0;
			//        mCharacters.erase(iter);
			//    }
			//}
		}

		void setID(int id)
		{
			//assert(mID < 0);
			//mID = id;
		}

		//void setRegistrationDate(time_ ttime)
		//{
		//    //mRegistrationDate = time;
		//}

		//void setLastLogin(time_t time)
		//{
		//    //mLastLogin = time;
		//}
	}
}