//
//  Character.cs
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
using ISL.Server.Network;
using ISL.Server.Common;
using invertika_game.Enums;
using ISL.Server.Game;

namespace invertika_game.Game
{
    public class Character : Being
    {
        //static const float LEVEL_SKILL_PRECEDENCE_FACTOR; // I am taking suggestions for a better name
        const int CHARPOINTS_PER_LEVELUP = 5;
        const int CORRECTIONPOINTS_PER_LEVELUP = 2;
        const int CORRECTIONPOINTS_MAX = 10;

        // Experience curve related values
        const float EXPCURVE_EXPONENT = 3.0f;
        const float EXPCURVE_FACTOR = 10.0f;
        const float LEVEL_SKILL_PRECEDENCE_FACTOR = 0.75f;
        const float EXP_LEVEL_FLEXIBILITY = 1.0f;
        GameClient mClient;   /**< Client computer. */
        /** Handler of the transaction the character is involved in. */
        //void *mTransactionHandler;

        //Possessions mPossessions;    /**< Possesssions of the character. */

        /** Attributes modified since last update. */
        //std::set<size_t> mModifiedAttributes;
        //std::set<size_t> mModifiedExperience;

        Dictionary<int, int> mExperience; /**< experience collected for each skill.*/

        Dictionary<int, Special> mSpecials;
        Dictionary<int, int> mStatusEffects; /**< only used by select functions
                                                to make it easier to make the accountserver
                                                do not modify or use anywhere else*/
        int mRechargePerSpecial;
        bool mSpecialUpdateNeeded;
        int mDatabaseID;             /**< Character's database ID. */
        byte mHairStyle;    /**< Hair Style of the character. */
        byte mHairColor;    /**< Hair Color of the character. */
        int mLevel;                  /**< Level of the character. */
        int mLevelProgress;          /**< progress to next level in percent */
        int mCharacterPoints;        /**< Unused attribute points that can be distributed */
        int mCorrectionPoints;       /**< Unused attribute correction points */
        bool mUpdateLevelProgress;   /**< Flag raised when percent to next level changed */
        bool mRecalculateLevel;      /**< Flag raised when the character level might have increased */
        byte mAccountLevel; /**< Account level of the user. */
        int mParty;                  /**< Party id of the character */
        TransactionType mTransaction; /**< Trade/buy/sell action the character is involved in. */
        Dictionary<int, int> mKillCount;  /**< How many monsters the character has slain of each type */

        public Character(MessageIn msg) : base(ThingType.OBJECT_CHARACTER)
		//:
		//Being(OBJECT_CHARACTER),
		//mClient(NULL),
		//mTransactionHandler(NULL),
		//mRechargePerSpecial(0),
		//mSpecialUpdateNeeded(false),
		//mDatabaseID(-1),
		//mHairStyle(0),
		//mHairColor(0),
		//mLevel(1),
		//mLevelProgress(0),
		//mUpdateLevelProgress(false),
		//mRecalculateLevel(true),
		//mParty(0),
		//mTransaction(TRANS_NONE)
        {
            //const AttributeScope &attr =
            //                       attributeManager.getAttributeScope(CharacterScope);
            //LOG_DEBUG("Character creation: initialisation of "
            //          << attr.size() << " attributes.");
            //for (AttributeScope::const_iterator it1 = attr.begin(),
            //     it1_end = attr.end(); it1 != it1_end; ++it1)
            //    mAttributes.insert(std::make_pair(it1.first, Attribute(*it1.second)));

            //// Get character data.
            //mDatabaseID = msg.readInt32();
            //setName(msg.readString());
            //deserializeCharacterData(*this, msg);
            //mOld = getPosition();
            //Inventory(this).initialize();
            //modifiedAllAttribute();
            //setSize(16);

            //// Give the character some specials for testing.
            ////TODO: Get from quest vars and equipment
            //giveSpecial(1);
            //giveSpecial(2);
            //giveSpecial(3);
        }

        void update()
        {
            //// Update character level
            //if (mRecalculateLevel)
            //{
            //    mRecalculateLevel = false;
            //    recalculateLevel();
            //}

            //// Update special recharge
            //std::list<Special *> rechargeNeeded;
            //int numRechargeNeeded = 0;
            //for (std::map<int, Special*>::iterator i = mSpecials.begin();
            //     i != mSpecials.end(); i++)
            //{
            //    Special * s = i.second;
            //    if (s.currentMana < s.neededMana)
            //    {
            //        rechargeNeeded.push_back(s);
            //        numRechargeNeeded++;
            //    }
            //}
            //if (numRechargeNeeded > 0)
            //{
            //    mRechargePerSpecial = getModifiedAttribute(ATTR_INT)
            //                          / numRechargeNeeded;
            //    for (std::list<Special*>::iterator i = rechargeNeeded.begin();
            //         i != rechargeNeeded.end(); i++)
            //    {
            //        (*i).currentMana += mRechargePerSpecial;
            //    }
            //}

            //if (mSpecialUpdateNeeded)
            //{
            //    sendSpecialUpdate();
            //    mSpecialUpdateNeeded = false;
            //}

            //mStatusEffects.clear();
            //StatusEffects::iterator it = mStatus.begin();
            //while (it != mStatus.end())
            //{
            //    mStatusEffects[it.first] = it.second.time;
            //    it++;
            //}
            //Being::update();
        }

        void perform()
        {
            //// Ticks attacks even when not attacking to permit cooldowns and warmups.
            //std::list<AutoAttack> attacksReady;
            //mAutoAttacks.tick(&attacksReady);

            //if (mAction != ATTACK || mTarget == NULL)
            //{
            //    mAutoAttacks.stop();
            //    return;
            //}

            //// Deal with the ATTACK action.

            //// Install default bare knuckle attack if no attacks were added from config.
            //// TODO: Get this from configuration.
            //if (!mAutoAttacks.getAutoAttacksNumber())
            //{
            //    int damageBase = getModifiedAttribute(ATTR_STR);
            //    int damageDelta = damageBase / 2;
            //    Damage knuckleDamage(skillManager.getDefaultSkillId(),
            //                         damageBase, damageDelta, 2, ELEMENT_NEUTRAL,
            //                        DAMAGE_PHYSICAL,
            //                        (getSize() < DEFAULT_TILE_LENGTH) ?
            //                            DEFAULT_TILE_LENGTH : getSize());

            //    AutoAttack knuckleAttack(knuckleDamage, 7, 3);
            //    mAutoAttacks.add(knuckleAttack);
            //}

            //if (attacksReady.empty())
            //{
            //    if (!mAutoAttacks.areActive())
            //        mAutoAttacks.start();
            //}
            //else
            //{
            //    // Performs all ready attacks.
            //    for (std::list<AutoAttack>::iterator it = attacksReady.begin();
            //         it != attacksReady.end(); ++it)
            //    {
            //        performAttack(mTarget, it.getDamage());
            //    }
            //}
        }

        void died()
        {
            //Being::died();
            //Script::executeGlobalEventFunction("on_chr_death", this);
        }

        void respawn()
        {
            //if (mAction != DEAD)
            //{
            //    LOG_WARN("Character \"" << getName()
            //             << "\" tried to respawn without being dead");
            //    return;
            //}

            //// Make it alive again
            //setAction(STAND);
            //// Reset target
            //mTarget = NULL;

            //// Execute respawn script
            //if (!Script::executeGlobalEventFunction("on_chr_death_accept", this))
            //{
            //    // Script-controlled respawning didn't work - fall back to
            //    // hardcoded logic.
            //    mAttributes[ATTR_HP].setBase(mAttributes[ATTR_MAX_HP].getModifiedAttribute());
            //    updateDerivedAttributes(ATTR_HP);
            //    // Warp back to spawn point.
            //    int spawnMap = Configuration::getValue("char_respawnMap", 1);
            //    int spawnX = Configuration::getValue("char_respawnX", 1024);
            //    int spawnY = Configuration::getValue("char_respawnY", 1024);
            //    GameState::enqueueWarp(this, MapManager::getMap(spawnMap), spawnX, spawnY);
            //}
        }

        void useSpecial(int id)
        {
            ////check if the character may use this special in general
            //std::map<int, Special*>::iterator i = mSpecials.find(id);
            //if (i == mSpecials.end())
            //{
            //    LOG_INFO("Character uses special "<<id<<" without autorisation.");
            //    return;
            //}

            ////check if the special is currently recharged
            //Special *special = i.second;
            //if (special.currentMana < special.neededMana)
            //{
            //    LOG_INFO("Character uses special "<<id<<" which is not recharged. ("
            //             <<special.currentMana<<"/"<<special.neededMana<<")");
            //    return;
            //}

            ////tell script engine to cast the spell
            //special.currentMana = 0;
            //Script::performSpecialAction(id, this);
            //mSpecialUpdateNeeded = true;
            return;
        }

        void sendSpecialUpdate()
        {
            ////GPMSG_SPECIAL_STATUS = 0x0293,
            //// { B specialID, L current, L max, L recharge }
            //for (std::map<int, Special*>::iterator i = mSpecials.begin();
            //     i != mSpecials.end(); i++)
            //{

            //    MessageOut msg(GPMSG_SPECIAL_STATUS );
            //    msg.writeInt8(i.first);
            //    msg.writeInt32(i.second.currentMana);
            //    msg.writeInt32(i.second.neededMana);
            //    msg.writeInt32(mRechargePerSpecial);
            //    /* Yes, the last one is redundant because it is the same for each
            //       special, but I would like to keep the netcode flexible enough
            //       to allow different recharge speed per special when necessary */
            //    gameHandler.sendTo(this, msg);
            //}
        }

        int getMapId()
        {
            //return getMap().getID();
            return -1; //ssk
        }

        void setMapId(int id)
        {
            // setMap(MapManager::getMap(id));
        }

        void cancelTransaction()
        {
            //TransactionType t = mTransaction;
            //mTransaction = TRANS_NONE;
            //switch (t)
            //{
            //    case TRANS_TRADE:
            //        static_cast< Trade * >(mTransactionHandler).cancel();
            //        break;
            //    case TRANS_BUYSELL:
            //        static_cast< BuySell * >(mTransactionHandler).cancel();
            //        break;
            //    case TRANS_NONE:
            //        return;
            //}
        }

        Trade getTrading()
        {
            //return mTransaction == TRANS_TRADE
            //    ? static_cast< Trade * >(mTransactionHandler) : NULL;

            return null; //ssk
        }

        BuySell getBuySell()
        {
            //return mTransaction == TRANS_BUYSELL
            //    ? static_cast< BuySell * >(mTransactionHandler) : NULL;

            return null; //ssk
        }

        void setTrading(Trade t)
        {
            //if (t)
            //{
            //    cancelTransaction();
            //    mTransactionHandler = t;
            //    mTransaction = TRANS_TRADE;
            //}
            //else
            //{
            //    assert(mTransaction == TRANS_NONE || mTransaction == TRANS_TRADE);
            //    mTransaction = TRANS_NONE;
            //}
        }

        void setBuySell(BuySell t)
        {
            //if (t)
            //{
            //    cancelTransaction();
            //    mTransactionHandler = t;
            //    mTransaction = TRANS_BUYSELL;
            //}
            //else
            //{
            //    assert(mTransaction == TRANS_NONE || mTransaction == TRANS_BUYSELL);
            //    mTransaction = TRANS_NONE;
            //}
        }

        void sendStatus()
        {
            //MessageOut attribMsg(GPMSG_PLAYER_ATTRIBUTE_CHANGE);
            //for (std::set<size_t>::const_iterator i = mModifiedAttributes.begin(),
            //     i_end = mModifiedAttributes.end(); i != i_end; ++i)
            //{
            //    int attr = *i;
            //    attribMsg.writeInt16(attr);
            //    attribMsg.writeInt32(getAttribute(attr) * 256);
            //    attribMsg.writeInt32(getModifiedAttribute(attr) * 256);
            //}
            //if (attribMsg.getLength() > 2) gameHandler.sendTo(this, attribMsg);
            //mModifiedAttributes.clear();

            //MessageOut expMsg(GPMSG_PLAYER_EXP_CHANGE);
            //for (std::set<size_t>::const_iterator i = mModifiedExperience.begin(),
            //     i_end = mModifiedExperience.end(); i != i_end; ++i)
            //{
            //    int skill = *i;
            //    expMsg.writeInt16(skill);
            //    expMsg.writeInt32(getExpGot(skill));
            //    expMsg.writeInt32(getExpNeeded(skill));
            //}
            //if (expMsg.getLength() > 2) gameHandler.sendTo(this, expMsg);
            //mModifiedExperience.clear();

            //if (mUpdateLevelProgress)
            //{
            //    mUpdateLevelProgress = false;
            //    MessageOut progressMessage(GPMSG_LEVEL_PROGRESS);
            //    progressMessage.writeInt8(mLevelProgress);
            //    gameHandler.sendTo(this, progressMessage);
            //}
        }

        void modifiedAllAttribute()
        {
            //LOG_DEBUG("Marking all attributes as changed, requiring recalculation.");
            //for (AttributeMap::iterator it = mAttributes.begin(),
            //     it_end = mAttributes.end();
            //    it != it_end; ++it)
            //{
            //    recalculateBaseAttribute(it.first);
            //    updateDerivedAttributes(it.first);
            //}
        }

        bool recalculateBaseAttribute(uint attr)
        {
            ///*
            // * `attr' may or may not have changed. Recalculate the base value.
            // */
            //LOG_DEBUG("Received update attribute recalculation request at Character"
            //          "for " << attr << ".");
            //if (!mAttributes.count(attr))
            //    return false;
            //double newBase = getAttribute(attr);

            ///*
            // * Calculate new base.
            // */
            //switch (attr)
            //{
            //case ATTR_ACCURACY:
            //    newBase = getModifiedAttribute(ATTR_DEX); // Provisional
            //    break;
            //case ATTR_DEFENSE:
            //    newBase = 0.3 * getModifiedAttribute(ATTR_VIT);
            //    break;
            //case ATTR_DODGE:
            //    newBase = getModifiedAttribute(ATTR_AGI); // Provisional
            //    break;
            //case ATTR_MAGIC_DODGE:
            //    newBase = 1.0;
            //    // TODO
            //    break;
            //case ATTR_MAGIC_DEFENSE:
            //    newBase = 0.0;
            //    // TODO
            //    break;
            //case ATTR_BONUS_ASPD:
            //    newBase = 0.0;
            //    // TODO
            //    break;
            //default:
            //    return Being::recalculateBaseAttribute(attr);
            //}

            //if (newBase != getAttribute(attr))
            //{
            //    setAttribute(attr, newBase);
            //    updateDerivedAttributes(attr);
            //    return true;
            //}
            //LOG_DEBUG("No changes to sync for attribute '" << attr << "'.");
            return false;
        }

        void updateDerivedAttributes(uint attr)
        {
            ///*
            // * `attr' has changed, perform updates accordingly.
            // */
            //flagAttribute(attr);

            //switch(attr)
            //{
            //case ATTR_STR:
            //    updateDerivedAttributes(ATTR_INV_CAPACITY);
            //    break;
            //case ATTR_AGI:
            //    updateDerivedAttributes(ATTR_DODGE);
            //    updateDerivedAttributes(ATTR_MOVE_SPEED_TPS);
            //    break;
            //case ATTR_VIT:
            //    updateDerivedAttributes(ATTR_MAX_HP);
            //    updateDerivedAttributes(ATTR_HP_REGEN);
            //    updateDerivedAttributes(ATTR_DEFENSE);
            //    break;
            //case ATTR_INT:
            //    // TODO
            //    break;
            //case ATTR_DEX:
            //    updateDerivedAttributes(ATTR_ACCURACY);
            //    break;
            //case ATTR_WIL:
            //    // TODO
            //    break;
            //default:
            //    Being::updateDerivedAttributes(attr);
            //}
        }

        void flagAttribute(int attr)
        {
            //// Inform the client of this attribute modification.
            //accountHandler.updateAttributes(getDatabaseID(), attr,
            //                                 getAttribute(attr),
            //                                 getModifiedAttribute(attr));
            //mModifiedAttributes.insert(attr);
            //if (attr == ATTR_INT)
            //{
            //    mSpecialUpdateNeeded = true;
            //}
        }

        int expForLevel(int level)
        {
            //return int(pow(level, EXPCURVE_EXPONENT) * EXPCURVE_FACTOR);
            return 0; //ssk
        }

        int levelForExp(int exp)
        {
            //return int(pow(float(exp) / EXPCURVE_FACTOR, 1.0f / EXPCURVE_EXPONENT));
            return 0; //ssk
        }

        void receiveExperience(int skill, int experience, int optimalLevel)
        {
            //// reduce experience when skill is over optimal level
            //int levelOverOptimum = levelForExp(getExperience(skill)) - optimalLevel;
            //if (optimalLevel && levelOverOptimum > 0)
            //{
            //    experience *= EXP_LEVEL_FLEXIBILITY
            //                  / (levelOverOptimum + EXP_LEVEL_FLEXIBILITY);
            //}

            //// Add exp
            //int oldExp = mExperience[skill];
            //long int newExp = mExperience[skill] + experience;
            //if (newExp < 0)
            //    newExp = 0; // Avoid integer underflow/negative exp.

            //// Check the skill cap
            //long int maxSkillCap = Configuration::getValue("game_maxSkillCap", INT_MAX);
            //assert(maxSkillCap <= INT_MAX);  // Avoid integer overflow.
            //if (newExp > maxSkillCap)
            //{
            //    newExp = maxSkillCap;
            //    if (oldExp != maxSkillCap)
            //    {
            //        LOG_INFO("Player hit the skill cap");
            //        // TODO: Send a message to player letting them know they hit the cap
            //        // or not?
            //    }
            //}
            //mExperience[skill] = newExp;
            //mModifiedExperience.insert(skill);

            //// Inform account server
            //if (newExp != oldExp)
            //    accountHandler.updateExperience(getDatabaseID(), skill, newExp);

            //// Check for skill levelup
            //if (Character::levelForExp(newExp) >= Character::levelForExp(oldExp))
            //    updateDerivedAttributes(skill);

            //mRecalculateLevel = true;
        }

        void incrementKillCount(int monsterType)
        {
            //std::map<int, int>::iterator i = mKillCount.find(monsterType);
            //if (i == mKillCount.end())
            //{
            //    // Character has never murdered this species before
            //    mKillCount[monsterType] = 1;
            //}
            //else
            //{
            //    // Character is a repeated offender
            //    mKillCount[monsterType] ++;
            //}
        }

        int getKillCount(int monsterType)
        {
            //std::map<int, int>::const_iterator i = mKillCount.find(monsterType);
            //if (i != mKillCount.end())
            //    return i.second;
            return 0;
        }

        void recalculateLevel()
        {
            //std::list<float> levels;
            //std::map<int, int>::const_iterator a;
            //for (a = getSkillBegin(); a != getSkillEnd(); a++)
            //{
            //    // Only use the first 1000 skill levels in calculation
            //    if (a.first < 1000)
            //    {
            //        float expGot = getExpGot(a.first);
            //        float expNeed = getExpNeeded(a.first);
            //        levels.push_back(levelForExp(a.first) + expGot / expNeed);
            //    }
            //}
            //levels.sort();

            //std::list<float>::iterator i = levels.end();
            //float level = 0.0f;
            //float factor = 1.0f;
            //float factorSum = 0.0f;
            //while (i != levels.begin())
            //{
            //    i--;
            //    level += *i * factor;
            //    factorSum += factor;
            //    factor *= LEVEL_SKILL_PRECEDENCE_FACTOR;
            //}
            //level /= factorSum;
            //level += 1.0f; // + 1.0f because the lowest level is 1 and not 0

            //while (mLevel < level)
            //{
            //    levelup();
            //}

            //int levelProgress = int((level - floor(level)) * 100);
            //if (levelProgress != mLevelProgress)
            //{
            //    mLevelProgress = levelProgress;
            //    mUpdateLevelProgress = true;
            //}
        }

        int getExpNeeded(Int64 skill)
        {
            //int level = levelForExp(getExperience(skill));
            //return Character::expForLevel(level + 1) - expForLevel(level);

            return 0; //ssk
        }

        int getExpGot(Int64 skill)
        {
            //int level = levelForExp(getExperience(skill));
            //return mExperience.at(skill) - Character::expForLevel(level);

            return 0; //ssk
        }

        void levelup()
        {
            //mLevel++;

            //mCharacterPoints += CHARPOINTS_PER_LEVELUP;
            //mCorrectionPoints += CORRECTIONPOINTS_PER_LEVELUP;
            //if (mCorrectionPoints > CORRECTIONPOINTS_MAX)
            //    mCorrectionPoints = CORRECTIONPOINTS_MAX;

            //MessageOut levelupMsg(GPMSG_LEVELUP);
            //levelupMsg.writeInt16(mLevel);
            //levelupMsg.writeInt16(mCharacterPoints);
            //levelupMsg.writeInt16(mCorrectionPoints);
            //gameHandler.sendTo(this, levelupMsg);
            //LOG_INFO(getName()<<" reached level "<<mLevel);
        }

        AttribmodResponseCode useCharacterPoint(Int64 attribute)
        {
            //if (!attributeManager.isAttributeDirectlyModifiable(attribute))
            //    return ATTRIBMOD_INVALID_ATTRIBUTE;
            //if (!mCharacterPoints)
            //    return ATTRIBMOD_NO_POINTS_LEFT;

            //--mCharacterPoints;
            //setAttribute(attribute, getAttribute(attribute) + 1);
            //updateDerivedAttributes(attribute);
            return AttribmodResponseCode.ATTRIBMOD_OK;
        }

        AttribmodResponseCode useCorrectionPoint(Int64 attribute)
        {
            //if (!attributeManager.isAttributeDirectlyModifiable(attribute))
            //    return ATTRIBMOD_INVALID_ATTRIBUTE;
            //if (!mCorrectionPoints)
            //    return ATTRIBMOD_NO_POINTS_LEFT;
            //if (getAttribute(attribute) <= 1)
            //    return ATTRIBMOD_DENIED;

            //--mCorrectionPoints;
            //++mCharacterPoints;
            //setAttribute(attribute, getAttribute(attribute) - 1);
            //updateDerivedAttributes(attribute);
            //return ATTRIBMOD_OK;

            return AttribmodResponseCode.ATTRIBMOD_OK; //ssk
        }

        void disconnected()
        {
            //for (Listeners::iterator i = mListeners.begin(),
            //     i_end = mListeners.end(); i != i_end;)
            //{
            //    const EventListener &l = **i;
            //    ++i; // In case the listener removes itself from the list on the fly.
            //    if (l.dispatch.disconnected)
            //        l.dispatch.disconnected(&l, this);
            //}
        }

        void giveSpecial(int id)
        {
            //if (mSpecials.find(id) == mSpecials.end())
            //{
            //    Special *s = new Special();
            //    Script::addDataToSpecial(id, s);
            //    mSpecials[id] = s;
            //    mSpecialUpdateNeeded = true;
            //}
        }

        void takeSpecial(int id)
        {
            //std::map<int, Special*>::iterator i = mSpecials.find(id);
            //if (i != mSpecials.end())
            //{
            //    delete i.second;
            //    mSpecials.erase(i);
            //    mSpecialUpdateNeeded = true;
            //}
        }

        void clearSpecials()
        {
            //for (std::map<int, Special*>::iterator i = mSpecials.begin();
            //     i != mSpecials.end(); i++)
            //{
            //    delete i.second;
            //}
            //mSpecials.clear();
        }
		
        public int getDatabaseID()
        { 
            return mDatabaseID;
        }
    }
}
