//
//  Being.cs
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
using ISL.Server.Common;
using ISL.Server.Utilities;
using invertika_game.Common;

namespace invertika_game.Game
{
    public class Being: Actor
    {
        const int TICKS_PER_HP_REGENERATION=100;

        protected BeingAction mAction;
        protected Dictionary<uint, Attribute > mAttributes;
        protected AutoAttacks mAutoAttacks;
        protected Dictionary< int, Status > mStatus;
        protected Being mTarget;
        protected Point mOld;                 /**< Old coordinates. */
        protected Point mDst;                 /**< Target coordinates. */
        protected BeingGender mGender;        /**< Gender of the being. */
        BeingDirection mDirection;
        string mName;

        /** Delay until move to next tile in miliseconds. */
        protected ushort mMoveTime;

		List<Point> mPath=new List<Point>();

        public Being(ThingType type) : base(type)
        {
            mAction=BeingAction.STAND;
            mTarget=null;
            mGender=BeingGender.GENDER_UNSPECIFIED;
            mDirection=BeingDirection.DOWN;
			
            Dictionary<int, List<AttributeInfoType>> attr=Program.attributeManager.getAttributeScope(ScopeType.BeingScope);
            Logger.Write(LogLevel.Debug, "Being creation: initialisation of {0} attributes.", attr.Count);

            foreach(KeyValuePair<int, List<AttributeInfoType>> pair in attr)
            {
                int debug=555; //TODO hier mal reinschauen
                //if(mAttributes.Count

			  
                //if (mAttributes.count(it1.first))
                //    LOG_WARN("Redefinition of attribute '" << it1.first << "'!");
                //LOG_DEBUG("Attempting to create attribute '" << it1.first << "'.");
                //mAttributes.insert(std::make_pair(it1.first,
                //                                  Attribute(*it1.second)));
            }

            // TODO: Way to define default base values?
            // Should this be handled by the virtual modifiedAttribute?
            // URGENT either way

            //#if 0
            //    // Initialize element resistance to 100 (normal damage).
            //    for (i = BASE_ELEM_BEGIN; i < BASE_ELEM_END; ++i)
            //    {
            //        mAttributes[i] = Attribute(TY_ST);
            //        mAttributes[i].setBase(100);
            //    }
            //#endif
        }

        /**
         * Performs actions scheduled by the being.
         */
        public virtual void perform()
        {
            throw new NotImplementedException("These function must be overloaded from derived class.");
        }

		        /**
         * Gets the old coordinates of the being.
         */
        public Point getOldPosition()
        { return mOld; }


        /**
         * Checks whether or not an attribute exists in this being.
         * @returns True if the attribute is present in the being, false otherwise.
         */
        bool checkAttributeExists(uint id)
        {
            return mAttributes.ContainsKey(id);
        }

        /** Gets the gender of the being (male or female). */
        public BeingGender getGender()
        {
            return mGender;
        }

        /** Sets the gender of the being (male or female). */
        public void setGender(BeingGender gender)
        {
            mGender=gender;
        }

        int damage(Actor source, Damage damage)
        {
            //    if (mAction == DEAD)
            //        return 0;

            //    int HPloss = damage.base;
            //    if (damage.delta)
            //        HPloss += rand() * (damage.delta + 1) / RAND_MAX;

            //    // TODO magical attacks and associated elemental modifiers
            //    switch (damage.type)
            //    {
            //        case DAMAGE_PHYSICAL:
            //            if (!damage.trueStrike &&
            //                rand()%((int) getModifiedAttribute(ATTR_DODGE) + 1) >
            //                    rand()%(damage.cth + 1))
            //            {
            //                HPloss = 0;
            //                // TODO Process triggers for a dodged physical attack here.
            //                // If there is an attacker included, also process triggers for the attacker (failed physical strike)
            //            }
            //            else
            //            {
            //                HPloss = HPloss * (1.0 - (0.0159375f *
            //                                          getModifiedAttribute(ATTR_DEFENSE)) /
            //                                   (1.0 + 0.017 *
            //                                    getModifiedAttribute(ATTR_DEFENSE))) +
            //                         (rand()%((HPloss >> 4) + 1));
            //                // TODO Process triggers for receiving damage here.
            //                // If there is an attacker included, also process triggers for the attacker (successful physical strike)
            //            }
            //            break;
            //        case DAMAGE_MAGICAL:
            //#if 0
            //            getModifiedAttribute(BASE_ELEM_BEGIN + damage.element);
            //#else
            //            LOG_WARN("Attempt to use magical type damage! This has not been"
            //                      "implemented yet and should not be used!");
            //            HPloss = 0;
            //#endif
            //        case DAMAGE_DIRECT:
            //            break;
            //        default:
            //            LOG_WARN("Unknown damage type '" << damage.type << "'!");
            //            break;
            //    }

            //    if (HPloss > 0)
            //    {
            //        mHitsTaken.push_back(HPloss);
            //        Attribute &HP = mAttributes.at(ATTR_HP);
            //        LOG_DEBUG("Being " << getPublicID() << " suffered " << HPloss
            //                  << " damage. HP: "
            //                  << HP.getModifiedAttribute() << "/"
            //                  << mAttributes.at(ATTR_MAX_HP).getModifiedAttribute());
            //        setAttribute(ATTR_HP, HP.getBase() - HPloss);
            //        // No HP regen after being hit if this is set.
            //        setTimerSoft(T_B_HP_REGEN,
            //                     Configuration::getValue("game_hpRegenBreakAfterHit", 0));
            //    }
            //    else
            //    {
            //        HPloss = 0;
            //    }

            //    return HPloss;

            return 0;
        }

        void heal()
        {
            //Attribute hp=mAttributes[ATTR_HP];
            //Attribute maxHp=mAttributes.at(ATTR_MAX_HP);
            //if(maxHp.getModifiedAttribute()==hp.getModifiedAttribute())
            //    return; // Full hp, do nothing.

            //// Reset all modifications present in hp.
            //hp.clearMods();
            //setAttribute(ATTR_HP, maxHp.getModifiedAttribute());
        }

        void heal(int gain)
        {
            //Attribute &hp = mAttributes.at(ATTR_HP);
            //Attribute &maxHp = mAttributes.at(ATTR_MAX_HP);
            //if (maxHp.getModifiedAttribute() == hp.getModifiedAttribute())
            //    return; // Full hp, do nothing.

            //// Cannot go over maximum hitpoints.
            //setAttribute(ATTR_HP, hp.getBase() + gain);
            //if (hp.getModifiedAttribute() > maxHp.getModifiedAttribute())
            //    setAttribute(ATTR_HP, maxHp.getModifiedAttribute());
        }

        void died()
        {
            //if (mAction == DEAD)
            //    return;

            //LOG_DEBUG("Being " << getPublicID() << " died.");
            //setAction(DEAD);
            //// dead beings stay where they are
            //clearDestination();

            //// reset target
            //mTarget = NULL;

            //for (Listeners::iterator i = mListeners.begin(),
            //     i_end = mListeners.end(); i != i_end;)
            //{
            //    const EventListener &l = **i;
            //    ++i; // In case the listener removes itself from the list on the fly.
            //    if (l.dispatch.died) l.dispatch.died(&l, this);
            //}
        }

        public void setDestination(Point dst)
        {
            mDst = dst;
            raiseUpdateFlags((byte)UpdateFlag.UPDATEFLAG_NEW_DESTINATION);
            mPath.Clear();
        }

        List<Point> findPath()
        {
            //Map *map = getMap().getMap();
            //int tileWidth = map.getTileWidth();
            //int tileHeight = map.getTileHeight();
            //int startX = getPosition().x / tileWidth;
            //int startY = getPosition().y / tileHeight;
            //int destX = mDst.x / tileWidth, destY = mDst.y / tileHeight;

            //return map.findPath(startX, startY, destX, destY, getWalkMask());

            return null; //ssk
        }

        void updateDirection(Point currentPos, Point destPos)
        {
            //// We update the being direction on each tile to permit other beings
            //// entering in range to always see the being with a direction value.

            //// We first handle simple cases

            //// If the character has reached its destination,
            //// don't update the direction since it's only a matter of keeping
            //// the previous one.
            //if (currentPos == destPos)
            //    return;

            //if (currentPos.x == destPos.x)
            //{
            //    if (currentPos.y > destPos.y)
            //        setDirection(UP);
            //    else
            //        setDirection(DOWN);
            //    return;
            //}

            //if (currentPos.y == destPos.y)
            //{
            //    if (currentPos.x > destPos.x)
            //        setDirection(LEFT);
            //    else
            //        setDirection(RIGHT);
            //    return;
            //}

            //// Now let's handle diagonal cases
            //// First, find the lower angle:
            //if (currentPos.x < destPos.x)
            //{
            //    // Up-right direction
            //    if (currentPos.y > destPos.y)
            //    {
            //        // Compute tan of the angle
            //        if ((currentPos.y - destPos.y) / (destPos.x - currentPos.x) < 1)
            //            // The angle is less than 45°, we look to the right
            //            setDirection(RIGHT);
            //        else
            //            setDirection(UP);
            //        return;
            //    }
            //    else // Down-right
            //    {
            //        // Compute tan of the angle
            //        if ((destPos.y - currentPos.y) / (destPos.x - currentPos.x) < 1)
            //            // The angle is less than 45°, we look to the right
            //            setDirection(RIGHT);
            //        else
            //            setDirection(DOWN);
            //        return;
            //    }
            //}
            //else
            //{
            //    // Up-left direction
            //    if (currentPos.y > destPos.y)
            //    {
            //        // Compute tan of the angle
            //        if ((currentPos.y - destPos.y) / (currentPos.x - destPos.x) < 1)
            //            // The angle is less than 45°, we look to the left
            //            setDirection(LEFT);
            //        else
            //            setDirection(UP);
            //        return;
            //    }
            //    else // Down-left
            //    {
            //        // Compute tan of the angle
            //        if ((destPos.y - currentPos.y) / (currentPos.x - destPos.x) < 1)
            //            // The angle is less than 45°, we look to the left
            //            setDirection(LEFT);
            //        else
            //            setDirection(DOWN);
            //        return;
            //    }
            //}
        }

        public void move()
        {
            // Immobile beings cannot move.
            if(!checkAttributeExists((int)Attributes.ATTR_MOVE_SPEED_RAW)
                ||getModifiedAttribute((int)Attributes.ATTR_MOVE_SPEED_RAW)==0)
                return;

            // Remember the current position before moving. This is used by
            // MapComposite::update() to determine whether a being has moved from one
            // zone to another.
            mOld=getPosition();

            if(mMoveTime>ManaServ.WORLD_TICK_MS)
            {
                // Current move has not yet ended
                mMoveTime-=ManaServ.WORLD_TICK_MS;
                return;
            }

            Map map=getMap().getMap();
            int tileWidth=map.getTileWidth();
            int tileHeight=map.getTileHeight();
            int tileSX=getPosition().x/tileWidth;
            int tileSY=getPosition().y/tileHeight;
            int tileDX=mDst.x/tileWidth;
            int tileDY=mDst.y/tileHeight;

            if(tileSX==tileDX&&tileSY==tileDY)
            {
                if(mAction==BeingAction.WALK)
                    setAction(BeingAction.STAND);
                // Moving while staying on the same tile is free
                // We only update the direction in that case.
                updateDirection(getPosition(), mDst);
                setPosition(mDst);
                mMoveTime=0;
                return;
            }

            /* If no path exists, the for-loop won't be entered. Else a path for the
             * current destination has already been calculated.
             * The tiles in this path have to be checked for walkability,
             * in case there have been changes. The 'getWalk' method of the Map
             * class has been used, because that seems to be the most logical
             * place extra functionality will be added.
             */
            foreach(Point point in mPath)
            {
                if(!map.getWalk(point.x, point.y, getWalkMask()))
                {
                    mPath.Clear();
                    break;
                }
            }

            if(mPath.Count==0)
            {
                // No path exists: the walkability of cached path has changed, the
                // destination has changed, or a path was never set.
                mPath=findPath();
            }

            if(mPath.Count==0)
            {
                if(mAction==BeingAction.WALK)
                    setAction(BeingAction.STAND);
                // no path was found
                mDst=mOld;
                mMoveTime=0;
                return;
            }

            setAction(BeingAction.WALK);

            Point prev=new Point(tileSX, tileSY);
            Point pos=new Point();
            do
            {
                Point next=mPath[0];
                mPath.RemoveAt(0);
                // SQRT2 is used for diagonal movement.
                mMoveTime+=(ushort)((prev.x==next.x||prev.y==next.y)?
                    getModifiedAttribute((int)Attributes.ATTR_MOVE_SPEED_RAW):
                        getModifiedAttribute((int)Attributes.ATTR_MOVE_SPEED_RAW)*Math.Sqrt(2));

                if(mPath.Count==0)
                {
                    // skip last tile center
                    pos=mDst;
                    break;
                }

                // Position the actor in the middle of the tile for pathfinding purposes
                pos.x=next.x*tileWidth+(tileWidth/2);
                pos.y=next.y*tileHeight+(tileHeight/2);
            }
            while (mMoveTime < ManaServ.WORLD_TICK_MS);
            setPosition(pos);

            mMoveTime=(ushort)(mMoveTime>ManaServ.WORLD_TICK_MS?mMoveTime-ManaServ.WORLD_TICK_MS:0);

            // Update the being direction also
            updateDirection(mOld, pos);
        }

        /**
         * Sets the destination coordinates of the being to the current
         * position.
         */
        public void clearDestination()
        {
            setDestination(getPosition());
        }

        int directionToAngle(int direction)
        {
            //switch (direction)
            //{
            //    case UP:    return  90;
            //    case DOWN:  return 270;
            //    case RIGHT: return 180;
            //    case LEFT:
            //    default:    return   0;
            //}

            return 0; //ssk
        }

        int performAttack(Being target, Damage damage)
        {
            //// check target legality
            //if (!target
            //        || target == this
            //        || target.getAction() == DEAD
            //        || !target.canFight())
            //    return -1;

            //if (getMap().getPvP() == PVP_NONE
            //        && target.getType() == OBJECT_CHARACTER
            //        && getType() == OBJECT_CHARACTER)
            //    return -1;

            //// check if target is in range using the pythagorean theorem
            //int distx = this.getPosition().x - target.getPosition().x;
            //int disty = this.getPosition().y - target.getPosition().y;
            //int distSquare = (distx * distx + disty * disty);
            //int maxDist = damage.range + target.getSize();
            //if (maxDist * maxDist < distSquare)
            //    return -1;

            //// Note: The auto-attack system will handle the delay between two attacks.

            //return (mTarget.damage(this, damage));

            return 0; //ssk
        }

        void setAction(BeingAction action)
        {
            //// Stops the auto-attacks when changing action
            //if (mAction == ATTACK && action != ATTACK)
            //    mAutoAttacks.stop();

            //mAction = action;
            //if (action != ATTACK && // The players are informed about these actions
            //    action != WALK)     // by other messages
            //{
            //    raiseUpdateFlags(UPDATEFLAG_ACTIONCHANGE);
            //}
        }

        void applyModifier(uint attr, double value, uint layer, uint duration, uint id)
        {
            //mAttributes.at(attr).add(duration, value, layer, id);
            //updateDerivedAttributes(attr);
        }

        bool removeModifier(uint attr, double value, uint layer, uint id, bool fullcheck)
        {
            //bool ret = mAttributes.at(attr).remove(value, layer, id, fullcheck);
            //updateDerivedAttributes(attr);
            //return ret;

            return true;
        }

        public void setAttribute(uint id, double value)
        {
            //AttributeMap::iterator ret = mAttributes.find(id);
            //if (ret == mAttributes.end())
            //{
            //    /*
            //     * The attribute does not yet exist, so we must attempt to create it.
            //     */
            //    LOG_ERROR("Being: Attempt to access non-existing attribute '"
            //              << id << "'!");
            //    LOG_WARN("Being: Creation of new attributes dynamically is not "
            //             "implemented yet!");
            //}
            //else
            //{
            //    ret.second.setBase(value);
            //    updateDerivedAttributes(id);
            //}
        }

        double getAttribute(uint id)
        {
            //AttributeMap::const_iterator ret = mAttributes.find(id);
            //if (ret == mAttributes.end())
            //{
            //    LOG_DEBUG("Being::getAttribute: Attribute "
            //              << id << " not found! Returning 0.");
            //    return 0;
            //}
            //return ret.second.getBase();

            return 0; //ssk
        }


        double getModifiedAttribute(uint id)
        {
            if(mAttributes.ContainsKey(id))
            {
                return mAttributes[id].getModifiedAttribute();
            }
            else
            {
                Logger.Write(LogLevel.Debug, "Being::getModifiedAttribute: Attribute {0} not found! Returning 0.", id);
                return 0;
            }
        }

        public void setModAttribute(uint a, double b)
        {
            // No-op to satisfy shared structure.
            // The game-server calculates this manually.
            return;
        }

        bool recalculateBaseAttribute(uint attr)
        {
            //LOG_DEBUG("Being: Received update attribute recalculation request for "
            //          << attr << ".");
            //if (!mAttributes.count(attr))
            //{
            //    LOG_DEBUG("Being::recalculateBaseAttribute: " << attr << " not found!");
            //    return false;
            //}
            //double newBase = getAttribute(attr);

            //switch (attr)
            //{
            //case ATTR_HP_REGEN:
            //    {
            //        double hpPerSec = getModifiedAttribute(ATTR_VIT) * 0.05;
            //        newBase = (hpPerSec * TICKS_PER_HP_REGENERATION / 10);
            //    }
            //    break;
            //case ATTR_HP:
            //    double diff;
            //    if ((diff = getModifiedAttribute(ATTR_HP)
            //        - getModifiedAttribute(ATTR_MAX_HP)) > 0)
            //        newBase -= diff;
            //    break;
            //case ATTR_MAX_HP:
            //    newBase = ((getModifiedAttribute(ATTR_VIT) + 3)
            //               * (getModifiedAttribute(ATTR_VIT) + 20)) * 0.125;
            //    break;
            //case ATTR_MOVE_SPEED_TPS:
            //    newBase = 3.0 + getModifiedAttribute(ATTR_AGI) * 0.08; // Provisional.
            //    break;
            //case ATTR_MOVE_SPEED_RAW:
            //    newBase = utils::tpsToRawSpeed(
            //                  getModifiedAttribute(ATTR_MOVE_SPEED_TPS));
            //    break;
            //case ATTR_INV_CAPACITY:
            //    // Provisional
            //    newBase = 2000.0 + getModifiedAttribute(ATTR_STR) * 180.0;
            //    break;
            //}
            //if (newBase != getAttribute(attr))
            //{
            //    setAttribute(attr, newBase);
            //    return true;
            //}
            //LOG_DEBUG("Being: No changes to sync for attribute '" << attr << "'.");
            return false;
        }

        void updateDerivedAttributes(uint attr)
        {
            //LOG_DEBUG("Being: Updating derived attribute(s) of: " << attr);
            //switch (attr)
            //{
            //case ATTR_MAX_HP:
            //    updateDerivedAttributes(ATTR_HP);
            //case ATTR_HP:
            //    raiseUpdateFlags(UPDATEFLAG_HEALTHCHANGE);
            //    break;
            //case ATTR_MOVE_SPEED_TPS:
            //    if (getAttribute(attr) > 0.0f)
            //        setAttribute(ATTR_MOVE_SPEED_RAW, utils::tpsToRawSpeed(
            //                     getModifiedAttribute(ATTR_MOVE_SPEED_TPS)));
            //    break;
            //default:
            //    // Do nothing
            //    break;
            //}
        }

        public void applyStatusEffect(int id, int timer)
        {
            if(mAction==BeingAction.DEAD)
                return;

            StatusEffect statusEffect=StatusManager.getStatus(id);
            if(statusEffect!=null)
            {
                Status newStatus=new Status();
                newStatus.status=statusEffect;
                newStatus.time=(uint)timer;
                mStatus[id]=newStatus;
            }
            else
            {
                Logger.Write(LogLevel.Error, "No status effect with ID {0}", id);
            }
        }

        void removeStatusEffect(int id)
        {
            //setStatusEffectTime(id, 0);
        }

        bool hasStatusEffect(int id)
        {
            //StatusEffects::const_iterator it = mStatus.begin();
            //while (it != mStatus.end())
            //{
            //    if (it.second.status.getId() == id)
            //        return true;
            //    it++;
            //}
            return false;
        }

        uint getStatusEffectTime(int id)
        {
            //StatusEffects::const_iterator it = mStatus.find(id);
            //if (it != mStatus.end()) return it.second.time;
            //else return 0;

            return 0; //ssk
        }

        /** Sets the name of the being. */
        public void setName(string name)
        {
            mName=name;
        }

        void setStatusEffectTime(int id, int time)
        {
            //StatusEffects::iterator it = mStatus.find(id);
            //if (it != mStatus.end()) it.second.time = time;
        }

        void update()
        {
            ////update timers
            //for (Timers::iterator i = mTimers.begin(); i != mTimers.end(); i++)
            //{
            //    if (i.second > -1) i.second--;
            //}

            //int oldHP = getModifiedAttribute(ATTR_HP);
            //int newHP = oldHP;
            //int maxHP = getModifiedAttribute(ATTR_MAX_HP);

            //// Regenerate HP
            //if (mAction != DEAD && !isTimerRunning(T_B_HP_REGEN))
            //{
            //    setTimerHard(T_B_HP_REGEN, TICKS_PER_HP_REGENERATION);
            //    newHP += getModifiedAttribute(ATTR_HP_REGEN);
            //}
            //// Cap HP at maximum
            //if (newHP > maxHP)
            //{
            //    newHP = maxHP;
            //}
            //// Only update HP when it actually changed to avoid network noise
            //if (newHP != oldHP)
            //{
            //    setAttribute(ATTR_HP, newHP);
            //    raiseUpdateFlags(UPDATEFLAG_HEALTHCHANGE);
            //}

            //// Update lifetime of effects.
            //for (AttributeMap::iterator it = mAttributes.begin();
            //     it != mAttributes.end();
            //     ++it)
            //    if (it.second.tick())
            //        updateDerivedAttributes(it.first);

            //// Update and run status effects
            //StatusEffects::iterator it = mStatus.begin();
            //while (it != mStatus.end())
            //{
            //    it.second.time--;
            //    if (it.second.time > 0 && mAction != DEAD)
            //        it.second.status.tick(this, it.second.time);

            //    if (it.second.time <= 0 || mAction == DEAD)
            //    {
            //        mStatus.erase(it);
            //        it = mStatus.begin();
            //    }
            //    it++;
            //}

            //// Check if being died
            //if (getModifiedAttribute(ATTR_HP) <= 0 && mAction != DEAD)
            //    died();
        }

        void inserted()
        {
            //Actor::inserted();

            //// Reset the old position, since after insertion it is important that it is
            //// in sync with the zone that we're currently present in.
            //mOld = getPosition();
        }

        void setTimerSoft(TimerID id, int value)
        {
            //Timers::iterator i = mTimers.find(id);
            //if (i == mTimers.end())
            //{
            //    mTimers[id] = value;
            //}
            //else if (i.second < value)
            //{
            //    i.second = value;
            //}
        }

        void setTimerHard(TimerID id, int value)
        {
            //mTimers[id] = value;
        }

        int getTimer(TimerID id)
        {
            //Timers::const_iterator i = mTimers.find(id);
            //return (i == mTimers.end()) ? -1 : i.second;

            return 0; //ssk
        }

        bool isTimerRunning(TimerID id)
        {
            return getTimer(id)>0;
        }

        bool isTimerJustFinished(TimerID id)
        {
            //return getTimer(id) == 0;

            //return 0;

            //TODO Funktion mit Original vergleichen
            return true; //ssk
        }
    }
}
