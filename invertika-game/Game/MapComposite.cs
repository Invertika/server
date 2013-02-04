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
using invertika_game.Scripting;
using ISL.Server.Common;

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
        Map mMap;            /**< Actual map. */
        public MapContent mContent; /**< Entities on the map. */
        Script mScript;      /**< Script associated to this map. */
        string mName;    /**< Name of the map. */
        ushort mID;   /**< ID of the map. */
        /** Cached persistent variables */
        Dictionary<string, string> mScriptVariables;
        PvPRules mPvPRules;

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
        {
            mID=(ushort)id;
            mName=name;
        }

        ~MapComposite()
        {
            //delete mMap;
            //delete mContent;
            //delete mScript;
        }

        /**
         * Returns whether the map is active on this server or not.
         */
        public bool isActive()
        {
            return mMap!=null?true:false;
        }

        public bool activate()
        {
            //assert(!isActive());

            string file="maps/"+mName+".tmx";
            if(!ResourceManager.exists(file))
                file+=".gz";

            mMap=MapReader.readMap(file);
            if(mMap==null)
                return false;

            initializeContent();

            string sPvP=mMap.getProperty("pvp");
            if(sPvP=="")
                sPvP=Configuration.getValue("game_defaultPvp", "");

            if(sPvP=="free")
                mPvPRules=PvPRules.PVP_FREE;
            else
                mPvPRules=PvPRules.PVP_NONE;

            Script s=getScript();
            if(s!=null)
            {
                //TODO Skript implementieren
                //s.setMap(this);
                //s.prepare("initialize");
                //s.execute();
            }

            return true;
        }

        /**
         * Gets the associated script. Returns 0 when no scripts or inline
         * NPCs are used on this map!
         */
        public Script getScript()
        {
            return mScript;
        }

        ZoneIterator getAroundPointIterator(Point p, int radius)
        {
            //MapRegion r;
            //mContent.fillRegion(r, p, radius);
            //return ZoneIterator(r, mContent);

            return null; //ssk
        }

        ZoneIterator getAroundActorIterator(Actor obj, int radius)
        {
            //MapRegion r;
            //mContent.fillRegion(r, obj.getPosition(), radius);
            //return ZoneIterator(r, mContent);

            return null; //ssk
        }

        ZoneIterator getInsideRectangleIterator(Rectangle p)
        {
            //MapRegion r;
            //mContent.fillRegion(r, p);
            //return ZoneIterator(r, mContent);

            return null; //ssk
        }

        ZoneIterator getAroundBeingIterator(Being obj, int radius)
        {
            //MapRegion r1;
            //mContent.fillRegion(r1, obj.getOldPosition(), radius);
            //MapRegion r2 = r1;
            //for (MapRegion::iterator i = r1.begin(), i_end = r1.end(); i != i_end; ++i)
            //{
            //    /* Fills region with destinations taken around the old position.
            //       This is necessary to detect two moving objects changing zones at the
            //       same time and at the border, and going in opposite directions (or
            //       more simply to detect teleportations, if any). */
            //    MapRegion &r4 = mContent.zones[*i].destinations;
            //    if (!r4.empty())
            //    {
            //        MapRegion r3;
            //        r3.reserve(r2.size() + r4.size());
            //        std::set_union(r2.begin(), r2.end(), r4.begin(), r4.end(),
            //                       std::back_insert_iterator< MapRegion >(r3));
            //        r2.swap(r3);
            //    }
            //}
            //mContent.fillRegion(r2, obj.getPosition(), radius);
            //return ZoneIterator(r2, mContent);

            return null; //ssk
        }

        public bool insert(Thing ptr)
        {
            if(ptr.isVisible())
            {
                if(ptr.canMove()&&!mContent.allocate((Being)ptr))
                {
                    return false;
                }

                Actor obj=(Actor)ptr;
                mContent.getZone(obj.getPosition()).insert(obj);
            }


            ptr.setMap(this);
            mContent.things.Add(ptr);
            return true;
        }

        void remove(Thing ptr)
        {
            //for (std::vector<Thing*>::iterator i = mContent.things.begin(),
            //     i_end = mContent.things.end(); i != i_end; ++i)
            //{
            //    if ((*i).canFight())
            //    {
            //        Being *being = static_cast<Being*>(*i);
            //        if (being.getTarget() == ptr)
            //        {
            //            being.setTarget(NULL);
            //        }
            //    }
            //    if (*i == ptr)
            //    {
            //        i = mContent.things.erase(i);
            //    }
            //}

            //if (ptr.isVisible())
            //{
            //    Actor *obj = static_cast< Actor * >(ptr);
            //    mContent.getZone(obj.getPosition()).remove(obj);

            //    if (ptr.canMove())
            //    {
            //        mContent.deallocate(static_cast< Being * >(ptr));
            //    }
            //}
        }

        public void update()
        {
            //for (int i = 0; i < mContent.mapHeight * mContent.mapWidth; ++i)
            //{
            //    mContent.zones[i].destinations.clear();
            //}

            //// Cannot use a WholeMap iterator as objects will change zones under its feet.
            //for (std::vector< Thing * >::iterator i = mContent.things.begin(),
            //     i_end = mContent.things.end(); i != i_end; ++i)
            //{
            //    if (!(*i).canMove())
            //        continue;

            //    Being *obj = static_cast< Being * >(*i);

            //    const Point &pos1 = obj.getOldPosition(),
            //                &pos2 = obj.getPosition();

            //    MapZone &src = mContent.getZone(pos1),
            //            &dst = mContent.getZone(pos2);
            //    if (&src != &dst)
            //    {
            //        addZone(src.destinations, &dst - mContent.zones);
            //        src.remove(obj);
            //        dst.insert(obj);
            //    }
            //}
        }

        public List<Thing> getEverything()
        {
            return mContent.things;
        }

        string getVariable(string key)
        {
            //std::map<std::string, std::string>::const_iterator i = mScriptVariables.find(key);
            //if (i != mScriptVariables.end())
            //    return i.second;
            //else
            //    return std::string();

            return ""; //ssk
        }

        void setVariable(string key, string value)
        {
            //// check if the value actually changed
            //std::map<std::string, std::string>::iterator i = mScriptVariables.find(key);
            //if (i == mScriptVariables.end() || i.second != value)
            //{
            //    // changed value or unknown variable
            //    mScriptVariables[key] = value;
            //    // update accountserver
            //    accountHandler.updateMapVar(this, key, value);
            //}
        }

        /**
		 * Initializes the map content. This creates the warps, spawn areas, npcs and
		 * other scripts.
		 */
        void initializeContent()
        {
            mContent=new MapContent(mMap);

            List<MapObject> objects=mMap.getObjects();

            for(int i = 0;i < objects.Count;++i)
            {
                MapObject obj=objects[i];
                string type=obj.getType();

                if(type=="WARP")
                {
                    string destMapName=obj.getProperty("DEST_MAP");
                    int destX=Convert.ToInt32(obj.getProperty("DEST_X"));
                    int destY=Convert.ToInt32(obj.getProperty("DEST_Y"));

                    //if(destMapName!=""&&destX&&destY) //TODO Check
                    if(destMapName!="")
                    {
                        MapComposite destMap=MapManager.getMap(destMapName);
                        if(destMap!=null)
                        {
                            WarpAction action=new WarpAction(destMap, destX, destY);
                            insert(new TriggerArea(this, obj.getBounds(),
                                                   action, false));
                        }
                    }
                    else
                    {
                        Logger.Write(LogLevel.Warning, "Unrecognized warp format");
                    }
                }
                else if(type=="SPAWN")
                {
                    MonsterClass monster=null;
                    int maxBeings=Convert.ToInt32(obj.getProperty("MAX_BEINGS"));
                    int spawnRate=Convert.ToInt32(obj.getProperty("SPAWN_RATE"));
                    string monsterName=obj.getProperty("MONSTER_ID");
                    int monsterId=Convert.ToInt32(monsterName);

                    if(monsterId>0)
                    {
                        monster=Program.monsterManager.getMonster(monsterId);
                        if(monster==null)
                        {
                            Logger.Write(LogLevel.Warning, "Couldn't find monster ID {0} for spawn area", monsterId);
                        }
                    }
                    else
                    {
                        monster=Program.monsterManager.getMonsterByName(monsterName);
                        if(monster==null)
                        {
                            Logger.Write(LogLevel.Warning, "Couldn't find monster {0} for spawn area", monsterName);
                        }
                    }

                    if(monster!=null&&maxBeings>0&&spawnRate>0)
                    {
                        insert(new SpawnArea(this, monster, obj.getBounds(),
                                             maxBeings, spawnRate));
                    }
                }
                else if(type=="NPC")
                {
                    int npcId=Convert.ToInt32(obj.getProperty("NPC_ID"));
                    string scriptText=obj.getProperty("SCRIPT");

                    if(mScript!=null)
                    {
                        // Determine script engine by xml property
                        string scriptEngineName=obj.getProperty("ENGINE");
                        if(scriptEngineName=="")
                        {
                            // Set engine to default value and print warning
                            scriptEngineName=Configuration.getValue("script_defaultEngine", "lua");
                            Logger.Write(LogLevel.Warning, "No script engine specified for map script {0} falling back to default", mName);
                        }
                        mScript=Script.create(scriptEngineName);
                    }

                    if(npcId>0&&scriptText!="")
                    {
                        //TODO Implementieren
//                        mScript.loadNPC(obj.getName(), npcId,
//                                        obj.getX(), obj.getY(),
//                                         scriptText);
                    }
                    else
                    {
                        Logger.Write(LogLevel.Warning, "Unrecognized format for npc");
                    }
                }
                else if(type=="SCRIPT")
                {
                    string scriptFilename=obj.getProperty("FILENAME");
                    string scriptText=obj.getProperty("TEXT");

                    if(mScript!=null)
                    {
                        // Determine script engine by xml property
                        string scriptEngineName=obj.getProperty("ENGINE");
                        if(scriptFilename!=""&&scriptEngineName=="")
                        {
                            // Engine property is empty - determine by filename
                            scriptEngineName=Script.determineEngineByFilename(scriptFilename);
                        }
                        else if(scriptEngineName=="")
                        {
                            // Set engine to default value and print warning
                            scriptEngineName=Configuration.getValue("script_defaultEngine", "lua");
                            Logger.Write(LogLevel.Warning, "No script engine specified for map script {0}, falling back to default", mName);
                        }
                        mScript=Script.create(scriptEngineName);
                    }

                    if(scriptFilename!="")
                    {
                        mScript.loadFile(scriptFilename);
                    }
                    else if(scriptText!=null)
                    {
                        //TODO Implementieren
//                        string name="'"+obj.getName()+"'' in "+mName;
//                        mScript.load(scriptText, name);
                    }
                    else
                    {
                        Logger.Write(LogLevel.Warning, "Unrecognized format for script");
                    }
                }
            }
        }
		
		
        /**
         * Changes a script variable without notifying the database server
         * about the change
         */
        public void setVariableFromDbserver(string key, string value)
        { 
            mScriptVariables[key]=@value; 
        }
		
        /**
         * Gets the game ID of this map.
         */
        public int getID()
        {
            return mID;
        }
		
        /**
         * Gets the underlying pathfinding map.
         */
        public Map getMap()
        {
            return mMap;
        }

        /**
         * Gets the name of this map.
         */
        public string getName()
        {
            return mName;
        }
    }
}
