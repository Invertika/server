//
//  Account.cs
//
//  This file is part of Invertika (http://invertika.org)
// 
//  Based on The Mana Server (http://manasource.org)
//  Copyright (C) 2004-2012  The Mana World Development Team 
//
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by Invertika Development Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
