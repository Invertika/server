/*
 *  The Mana Server
 *  Copyright (C) 2004  The Mana World Development Team
 *
 *  This file is part of The Mana Server.
 *
 *  The Mana Server is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  any later version.
 *
 *  The Mana Server is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with The Mana Server.  If not, see <http://www.gnu.org/licenses/>.
 */

#ifndef CHARACTERDATA_HPP
#define CHARACTERDATA_HPP

#include <string>
#include <vector>
#include <map>

#include "defines.h"
#include "point.h"
#include "common/inventorydata.hpp"

class Account;
class MessageIn;

class Character
{
    public:

        Character(const std::string &name, int id = -1);

        /**
         * Gets the database id of the character.
         */
        int getDatabaseID() const { return mDatabaseID; }
        void setDatabaseID(int id) { mDatabaseID = id; }

        /** Gets the account the character belongs to. */
        Account *getAccount() const
        { return mAccount; }

        /** Sets the account the character belongs to, and related fields. */
        void setAccount(Account *ptr);

        /**
         * Gets the ID of the account the character belongs to.
         */
        int getAccountID() const { return mAccountID; }
        void setAccountID(int id) { mAccountID = id; }

        /**
         * Gets the name of the character.
         */
        const std::string &getName() const { return mName; }
        void setName(const std::string &name) { mName = name; }

        /**
         * Gets the gender of the character (male / female).
         */
        int getGender() const { return mGender; }
        void setGender(int gender) { mGender = gender; }

        /**
         * Gets the hairstyle of the character.
         */
        int getHairStyle() const { return mHairStyle; }
        void setHairStyle(int style) { mHairStyle = style; }

        /**
         * Gets the haircolor of the character.
         */
        int getHairColor() const { return mHairColor; }
        void setHairColor(int color) { mHairColor = color; }

        /** Gets the account level of the user. */
        int getAccountLevel() const
        { return mAccountLevel; }

        /**
          * Sets the account level of the user.
          * @param force ensure the level is not modified by a game server.
          */
        void setAccountLevel(int l, bool force = false)
        { if (force) mAccountLevel = l; }

        /**
         * Gets the level of the character.
         */
        int getLevel() const { return mLevel; }
        void setLevel(int level) { mLevel = level; }

        /** Gets the value of a base attribute of the character. */
        int getAttribute(int n) const
        { return mAttributes[n - CHAR_ATTR_BEGIN]; }

        /** Sets the value of a base attribute of the character. */
        void setAttribute(int n, int value)
        { mAttributes[n - CHAR_ATTR_BEGIN] = value; }

        int getSkillSize() const
        { return mExperience.size(); }

        const std::map<int, int>::const_iterator getSkillBegin() const
        { return mExperience.begin(); }

        const std::map<int, int>::const_iterator getSkillEnd() const
        { return mExperience.end(); }

        int getExperience(int skill) const
        { return mExperience.find(skill)->second; }

        void setExperience(int skill, int value)
        { mExperience[skill] = value; }

        void receiveExperience(int skill, int value)
        { mExperience[skill] += value; }

        /**
         * Get / Set a status effects
         */
        void applyStatusEffect(int id, int time)
        { mStatusEffects[id] = time; }

        int getStatusEffectSize() const
        { return mStatusEffects.size(); }

        const std::map<int, int>::const_iterator getStatusEffectBegin() const
        { return mStatusEffects.begin(); }

        const std::map<int, int>::const_iterator getStatusEffectEnd() const
        { return mStatusEffects.end(); }

        /**
         * Gets the Id of the map that the character is on.
         */
        int getMapId() const { return mMapId; }
        void setMapId(int mapId) { mMapId = mapId; }

        /**
         * Gets the position of the character on the map.
         */
        const Point &getPosition() const { return mPos; }
        void setPosition(const Point &p) { mPos = p; }

        /** Add a guild to the character */
        void addGuild(const std::string &name) { mGuilds.push_back(name); }

        /** Returns a list of guilds the player belongs to */
        std::vector<std::string>
        getGuilds() const { return mGuilds; }

        /**
         * Gets a reference on the possessions.
         */
        const Possessions &getPossessions() const
        { return mPossessions; }

        /**
         * Gets a reference on the possessions.
         */
        Possessions &getPossessions()
        { return mPossessions; }

        void setCharacterPoints(int points)
        { mCharacterPoints = points; }

        int getCharacterPoints() const
        { return mCharacterPoints; }

        void setCorrectionPoints(int points)
        { mCorrectionPoints = points; }

        int getCorrectionPoints() const
        { return mCorrectionPoints; }


    private:
        Character(const Character &);
        Character &operator=(const Character &);

        Possessions mPossessions; //!< All the possesions of the character.
        std::string mName;        //!< Name of the character.
        int mDatabaseID;          //!< Character database ID.
        int mAccountID;           //!< Account ID of the owner.
        Account *mAccount;        //!< Account owning the character.
        Point mPos;               //!< Position the being is at.
        unsigned short mAttributes[CHAR_ATTR_NB]; //!< Attributes.
        std::map<int, int> mExperience; //!< Skill Experience.
        std::map<int, int> mStatusEffects; //!< Status Effects
        unsigned short mMapId;    //!< Map the being is on.
        unsigned char mGender;    //!< Gender of the being.
        unsigned char mHairStyle; //!< Hair style of the being.
        unsigned char mHairColor; //!< Hair color of the being.
        short mLevel;             //!< Level of the being.
        short mCharacterPoints;   //!< Unused character points.
        short mCorrectionPoints;  //!< Unused correction points.
        unsigned char mAccountLevel; //!< Level of the associated account.

        std::vector<std::string> mGuilds;        //!< All the guilds the player
                                                 //!< belongs to.
};

/**
 * Type definition for a list of Characters.
 */
typedef std::vector< Character * > Characters;

#endif
