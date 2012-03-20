using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Account
{
	public class Character
	{
		//Character(Character &);
		//Character &operator=(const Character &);

		//double getAttrBase(AttributeMap::const_iterator &it) const
		//{ return it->second.base; }
		//double getAttrMod(AttributeMap::const_iterator &it) const
		//{ return it->second.modified; }

		//Possessions mPossessions; //!< All the possesions of the character.
		string mName;        //!< Name of the character.
		int mDatabaseID;          //!< Character database ID.
		uint mCharacterSlot;  //!< Character slot.
		int mAccountID;           //!< Account ID of the owner.
		Account mAccount;        //!< Account owning the character.
		//Point mPos;               //!< Position the being is at.
		//AttributeMap mAttributes; //!< Attributes.
		Dictionary<int, int> mExperience; //!< Skill Experience.
		Dictionary<int, int> mStatusEffects; //!< Status Effects
		Dictionary<int, int> mKillCount; //!< Kill Count
		//Dictionary<int, Special>  mSpecials;
		ushort mMapId;    //!< Map the being is on.
		byte mGender;    //!< Gender of the being.
		byte mHairStyle; //!< Hair style of the being.
		byte mHairColor; //!< Hair color of the being.
		short mLevel;             //!< Level of the being.
		short mCharacterPoints;   //!< Unused character points.
		short mCorrectionPoints;  //!< Unused correction points.
		byte mAccountLevel; //!< Level of the associated account.

		//std::vector<std::string> mGuilds;        //!< All the guilds the player
		//!< belongs to.
		//friend class AccountHandler;
		//friend class Storage;
		// Set as a friend, but still a lot of redundant accessors. FIXME.
		//template< class T >
		//friend void serializeCharacterData(const T &data, MessageOut &msg);

		public Character(string name, int id)
		{
			//Konstruktorgedöns
			//        mName(name),
			//mDatabaseID(id),
			//mCharacterSlot(0),
			//mAccountID(-1),
			//mAccount(NULL),
			//mMapId(0),
			//mGender(0),
			//mHairStyle(0),
			//mHairColor(0),
			//mLevel(0),
			//mCharacterPoints(0),
			//mCorrectionPoints(0),
			//mAccountLevel(0)
		}

		void setAccount(Account acc)
		{
			mAccount=acc;
			//mAccountID = acc.getID();
			//mAccountLevel = acc->getLevel();
		}

		/**
		* Gets the Id of the map that the character is on.
		*/
		public int getMapId()
		{
			return mMapId;
		}

		/**
 * Gets the ID of the account the character belongs to.
 */
		public int getAccountID()
		{
			return mAccountID;
		}

		void setAccountID(int id)
		{
			mAccountID=id;
		}

		/**
 * Gets the name of the character.
 */
		public string getName()
		{
			return mName;
		}

		void setName(string name)
		{
			mName=name;
		}

		/**
	  * Gets the database id of the character.
	  */
		public int getDatabaseID()
		{
			return mDatabaseID;
		}

		void setDatabaseID(int id)
		{
			mDatabaseID=id;
		}
	}
}
