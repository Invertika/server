//
//  MapComposite.cs
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
using ISL.Server.Utilities;

namespace invertika_game.Game
{
	public class MapComposite
	{
		/* TODO: Implement overlapping map zones instead of strict partitioning.
		   Purpose: to decrease the number of zone changes, as overlapping allows for
		   hysteresis effect and prevents an actor from changing zone each server
		   tick. It requires storing the zone in the actor since it will not be
		   uniquely defined any longer. */

		/* Pixel-based width and height of the squares used in partitioning the map.
		   Squares should be big enough so that an actor cannot cross several ones
		   in one world tick.
		   TODO: Tune for decreasing server load. The higher the value, the closer we
		   regress to quadratic behavior; the lower the value, the more we waste time
		   in dealing with zone changes. */
		const int zoneDiam=256;


		static void addZone(List<uint> r, uint z)
		{
			//MapRegion::iterator i_end = r.end(),
			//                    i = std::lower_bound(r.begin(), i_end, z);
			//if (i == i_end || *i != z)
			//{
			//    r.insert(i, z);
			//}
		}

		public MapComposite(int id, string name)
		//    :
		//mMap(NULL),
		//mContent(NULL),
		//mScript(NULL),
		//mName(name),
		//mID(id)
		{
		}

		~MapComposite()
		{
			//delete mMap;
			//delete mContent;
			//delete mScript;
		}

		bool activate()
		{
			//assert(!isActive());

			//std::string file = "maps/" + mName + ".tmx";
			//if (!ResourceManager::exists(file))
			//    file += ".gz";

			//mMap = MapReader::readMap(file);
			//if (!mMap)
			//    return false;

			//initializeContent();

			//std::string sPvP = mMap->getProperty("pvp");
			//if (sPvP.empty())
			//    sPvP = Configuration::getValue("game_defaultPvp", std::string());

			//if (sPvP == "free")
			//    mPvPRules = PVP_FREE;
			//else
			//    mPvPRules = PVP_NONE;

			//if (Script *s = getScript())
			//{
			//    s->setMap(this);
			//    s->prepare("initialize");
			//    s->execute();
			//}

			return true;
		}

		ZoneIterator getAroundPointIterator(Point p, int radius)
		{
			//MapRegion r;
			//mContent->fillRegion(r, p, radius);
			//return ZoneIterator(r, mContent);

			return null; //ssk
		}

		ZoneIterator getAroundActorIterator(Actor obj, int radius)
		{
			//MapRegion r;
			//mContent->fillRegion(r, obj->getPosition(), radius);
			//return ZoneIterator(r, mContent);

			return null; //ssk
		}

		ZoneIterator getInsideRectangleIterator(Rectangle p)
		{
			//MapRegion r;
			//mContent->fillRegion(r, p);
			//return ZoneIterator(r, mContent);

			return null; //ssk
		}

		ZoneIterator getAroundBeingIterator(Being obj, int radius)
		{
			//MapRegion r1;
			//mContent->fillRegion(r1, obj->getOldPosition(), radius);
			//MapRegion r2 = r1;
			//for (MapRegion::iterator i = r1.begin(), i_end = r1.end(); i != i_end; ++i)
			//{
			//    /* Fills region with destinations taken around the old position.
			//       This is necessary to detect two moving objects changing zones at the
			//       same time and at the border, and going in opposite directions (or
			//       more simply to detect teleportations, if any). */
			//    MapRegion &r4 = mContent->zones[*i].destinations;
			//    if (!r4.empty())
			//    {
			//        MapRegion r3;
			//        r3.reserve(r2.size() + r4.size());
			//        std::set_union(r2.begin(), r2.end(), r4.begin(), r4.end(),
			//                       std::back_insert_iterator< MapRegion >(r3));
			//        r2.swap(r3);
			//    }
			//}
			//mContent->fillRegion(r2, obj->getPosition(), radius);
			//return ZoneIterator(r2, mContent);

			return null; //ssk
		}

		bool insert(Thing ptr)
		{
			//if (ptr->isVisible())
			//{
			//    if (ptr->canMove() && !mContent->allocate(static_cast< Being * >(ptr)))
			//    {
			//        return false;
			//    }

			//    Actor *obj = static_cast< Actor * >(ptr);
			//    mContent->getZone(obj->getPosition()).insert(obj);
			//}

			//ptr->setMap(this);
			//mContent->things.push_back(ptr);
			return true;
		}

		void remove(Thing ptr)
		{
			//for (std::vector<Thing*>::iterator i = mContent->things.begin(),
			//     i_end = mContent->things.end(); i != i_end; ++i)
			//{
			//    if ((*i)->canFight())
			//    {
			//        Being *being = static_cast<Being*>(*i);
			//        if (being->getTarget() == ptr)
			//        {
			//            being->setTarget(NULL);
			//        }
			//    }
			//    if (*i == ptr)
			//    {
			//        i = mContent->things.erase(i);
			//    }
			//}

			//if (ptr->isVisible())
			//{
			//    Actor *obj = static_cast< Actor * >(ptr);
			//    mContent->getZone(obj->getPosition()).remove(obj);

			//    if (ptr->canMove())
			//    {
			//        mContent->deallocate(static_cast< Being * >(ptr));
			//    }
			//}
		}

		void update()
		{
			//for (int i = 0; i < mContent->mapHeight * mContent->mapWidth; ++i)
			//{
			//    mContent->zones[i].destinations.clear();
			//}

			//// Cannot use a WholeMap iterator as objects will change zones under its feet.
			//for (std::vector< Thing * >::iterator i = mContent->things.begin(),
			//     i_end = mContent->things.end(); i != i_end; ++i)
			//{
			//    if (!(*i)->canMove())
			//        continue;

			//    Being *obj = static_cast< Being * >(*i);

			//    const Point &pos1 = obj->getOldPosition(),
			//                &pos2 = obj->getPosition();

			//    MapZone &src = mContent->getZone(pos1),
			//            &dst = mContent->getZone(pos2);
			//    if (&src != &dst)
			//    {
			//        addZone(src.destinations, &dst - mContent->zones);
			//        src.remove(obj);
			//        dst.insert(obj);
			//    }
			//}
		}

		List<Thing> getEverything()
		{
			//return mContent->things;
			return null; //ssk
		}


		string getVariable(string key)
		{
			//std::map<std::string, std::string>::const_iterator i = mScriptVariables.find(key);
			//if (i != mScriptVariables.end())
			//    return i->second;
			//else
			//    return std::string();

			return ""; //ssk
		}

		void setVariable(string key, string value)
		{
			//// check if the value actually changed
			//std::map<std::string, std::string>::iterator i = mScriptVariables.find(key);
			//if (i == mScriptVariables.end() || i->second != value)
			//{
			//    // changed value or unknown variable
			//    mScriptVariables[key] = value;
			//    // update accountserver
			//    accountHandler->updateMapVar(this, key, value);
			//}
		}

		/**
		 * Initializes the map content. This creates the warps, spawn areas, npcs and
		 * other scripts.
		 */
		void initializeContent()
		{
			//mContent = new MapContent(mMap);

			//const std::vector<MapObject*> &objects = mMap->getObjects();

			//for (size_t i = 0; i < objects.size(); ++i)
			//{
			//    const MapObject *object = objects.at(i);
			//    const std::string &type = object->getType();

			//    if (utils::compareStrI(type, "WARP") == 0)
			//    {
			//        std::string destMapName = object->getProperty("DEST_MAP");
			//        int destX = utils::stringToInt(object->getProperty("DEST_X"));
			//        int destY = utils::stringToInt(object->getProperty("DEST_Y"));

			//        if (!destMapName.empty() && destX && destY)
			//        {
			//            if (MapComposite *destMap = MapManager::getMap(destMapName))
			//            {
			//                WarpAction *action = new WarpAction(destMap, destX, destY);
			//                insert(new TriggerArea(this, object->getBounds(),
			//                                       action, false));
			//            }
			//        }
			//        else
			//        {
			//            LOG_WARN("Unrecognized warp format");
			//        }
			//    }
			//    else if (utils::compareStrI(type, "SPAWN") == 0)
			//    {
			//        MonsterClass *monster = 0;
			//        int maxBeings = utils::stringToInt(object->getProperty("MAX_BEINGS"));
			//        int spawnRate = utils::stringToInt(object->getProperty("SPAWN_RATE"));
			//        std::string monsterName = object->getProperty("MONSTER_ID");
			//        int monsterId = utils::stringToInt(monsterName);

			//        if (monsterId)
			//        {
			//            monster = monsterManager->getMonster(monsterId);
			//            if (!monster)
			//            {
			//                LOG_WARN("Couldn't find monster ID " << monsterId <<
			//                         " for spawn area");
			//            }
			//        }
			//        else
			//        {
			//            monster = monsterManager->getMonsterByName(monsterName);
			//            if (!monster)
			//            {
			//                LOG_WARN("Couldn't find monster " << monsterName <<
			//                         " for spawn area");
			//            }
			//        }

			//        if (monster && maxBeings && spawnRate)
			//        {
			//            insert(new SpawnArea(this, monster, object->getBounds(),
			//                                 maxBeings, spawnRate));
			//        }
			//    }
			//    else if (utils::compareStrI(type, "NPC") == 0)
			//    {
			//        int npcId = utils::stringToInt(object->getProperty("NPC_ID"));
			//        std::string scriptText = object->getProperty("SCRIPT");

			//        if (!mScript)
			//        {
			//            // Determine script engine by xml property
			//            std::string scriptEngineName = object->getProperty("ENGINE");
			//            if (scriptEngineName.empty())
			//            {
			//                // Set engine to default value and print warning
			//                scriptEngineName = Configuration::getValue("script_defaultEngine", "lua");
			//                LOG_WARN("No script engine specified for map script \""
			//                        + mName + "\", falling back to default");
			//            }
			//            mScript = Script::create(scriptEngineName);
			//        }

			//        if (npcId && !scriptText.empty())
			//        {
			//            mScript->loadNPC(object->getName(), npcId,
			//                             object->getX(), object->getY(),
			//                             scriptText.c_str());
			//        }
			//        else
			//        {
			//            LOG_WARN("Unrecognized format for npc");
			//        }
			//    }
			//    else if (utils::compareStrI(type, "SCRIPT") == 0)
			//    {
			//        std::string scriptFilename = object->getProperty("FILENAME");
			//        std::string scriptText = object->getProperty("TEXT");

			//        if (!mScript)
			//        {
			//            // Determine script engine by xml property
			//            std::string scriptEngineName = object->getProperty("ENGINE");
			//            if (!scriptFilename.empty() && scriptEngineName.empty())
			//            {
			//                // Engine property is empty - determine by filename
			//                scriptEngineName = Script::determineEngineByFilename(scriptFilename);
			//            }
			//            else if (scriptEngineName.empty())
			//            {
			//                // Set engine to default value and print warning
			//                scriptEngineName = Configuration::getValue("script_defaultEngine", "lua");
			//                LOG_WARN("No script engine specified for map script \""
			//                        + mName + "\", falling back to default");
			//            }
			//            mScript = Script::create(scriptEngineName);
			//        }

			//        if (!scriptFilename.empty())
			//        {
			//            mScript->loadFile(scriptFilename);
			//        }
			//        else if (!scriptText.empty())
			//        {
			//            std::string name = "'" + object->getName() + "'' in " + mName;
			//            mScript->load(scriptText.c_str(), name.c_str());
			//        }
			//        else
			//        {
			//            LOG_WARN("Unrecognized format for script");
			//        }
			//    }
			//}
		}
	}
}
