//
//  MonsterClass.cs
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
using invertika_game.Common;

namespace invertika_game.Game
{
	/**
 * Class describing the characteristics of a generic monster.
 */
	public class MonsterClass
	{
		ushort mId;
		string mName;
		BeingGender mGender;

		List<MonsterDrop> mDrops;
		Dictionary<int, double> mAttributes; /**< Base attributes of the monster. */
		float mSpeed; /**< The monster class speed in tiles per second */
		int mSize;
		int mExp;

		bool mAggressive;
		uint mTrackRange;
		uint mStrollRange;
		uint mMutation;
		uint mAttackDistance;
		int mOptimalLevel;
		List<MonsterAttack> mAttacks;
		string mScript;

		//friend class MonsterManager;
		//friend class Monster;

		MonsterClass(int id)
		{
			//mId(id),
			//mName("unnamed"),
			//mGender(GENDER_UNSPECIFIED),
			//mSpeed(1),
			//mSize(16),
			//mExp(-1),
			//mAggressive(false),
			//mTrackRange(1),
			//mStrollRange(0),
			//mMutation(0),
			//mAttackDistance(0),
			//mOptimalLevel(0)
		}

		/**
		 * Returns monster type. This is the Id of the monster class.
		 */
		int getId()
		{ return mId; }

		/**
		 * Returns the name of the monster type
		 */
		string getName()
		{ return mName; }

		/**
		 * Sets the name of the monster type
		 */
		void setName(string name)
		{ mName=name; }

		void setGender(BeingGender gender)
		{ mGender=gender; }

		BeingGender getGender()
		{ return mGender; }

		/**
		 * Sets monster drops. These are the items the monster drops when it
		 * dies.
		 */
		void setDrops(List<MonsterDrop> v)
		{ mDrops=v; }

		/**
		 * Sets a being base attribute.
		 */
		void setAttribute(int attribute, double value)
		{ mAttributes[attribute]=value; }

		/**
		 * Returns a being base attribute.
		 */
		double getAttribute(int attribute)
		{ return mAttributes[attribute]; }

		/**
		 * Returns whether the monster has got the attribute.
		 */
		bool hasAttribute(int attribute)
		{
			//return (mAttributes.find(attribute) != mAttributes.end()); 
			return true; //ssk
		}

		/** Sets collision circle radius. */
		void setSize(int size) { mSize=size; }

		/** Returns collision circle radius. */
		int getSize() { return mSize; }

		/** Sets experience reward for killing the monster. */
		void setExp(int exp) { mExp=exp; }

		/** Returns experience reward for killing the monster. */
		int getExp() { return mExp; }

		/** Gets maximum skill level after which exp reward is reduced */
		void setOptimalLevel(int level) { mOptimalLevel=level; }

		/** Sets maximum skill level after which exp reward is reduced. */
		int getOptimalLevel() { return mOptimalLevel; }

		/** Sets if the monster attacks without being attacked first. */
		void setAggressive(bool aggressive) { mAggressive=aggressive; }

		/** Returns if the monster attacks without being attacked first. */
		bool isAggressive() { return mAggressive; }

		/** Sets range in tiles in which the monster searches for enemies. */
		void setTrackRange(uint range) { mTrackRange=range; }

		/**
		 * Returns range in tiles in which the monster searches for enemies.
		 */
		uint getTrackRange() { return mTrackRange; }

		/** Sets range in pixels in which the monster moves around when idle. */
		void setStrollRange(uint range) { mStrollRange=range; }

		/**
		 * Returns range in pixels in which the monster moves around when idle.
		 */
		uint getStrollRange() { return mStrollRange; }

		/** Sets mutation factor in percent. */
		void setMutation(uint factor) { mMutation=factor; }

		/** Returns mutation factor in percent. */
		uint getMutation() { return mMutation; }

		/** Sets preferred combat distance in pixels. */
		void setAttackDistance(uint distance)
		{ mAttackDistance=distance; }

		/** Returns preferred combat distance in pixels. */
		uint getAttackDistance() { return mAttackDistance; }

		/** Adds an attack to the monsters repertoire. */
		void addAttack(MonsterAttack type) { mAttacks.Add(type); }

		/** Returns all attacks of the monster. */
		List<MonsterAttack> getAttacks() { return mAttacks; }

		/** sets the script file for the monster */
		void setScript(string filename) { mScript=filename; }

		/** Returns script filename */
		string getScript() { return mScript; }
	}
}
