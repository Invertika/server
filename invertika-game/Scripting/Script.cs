//
//  Script.cs
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
using invertika_game.Game;
using invertika_game.Common;
using ISL.Server.Game;

namespace invertika_game.Scripting
{
    public class Script
    {
        //typedef std::map< std::string, Script::Factory > Engines;

        //static Engines *engines = NULL;
        //Script *Script::globalEventScript = NULL;

        //Script::Script():
        //    mMap(NULL),
        //    mEventListener(&scriptEventDispatch)
        //{}

        //void registerEngine(string name, Factory f)
        //{
        //    //if (!engines)
        //    //{
        //    //    /* The "engines" variable is a pointer instead of an object, because
        //    //       this file may not have been initialized at the time this function
        //    //       is called. So we take care of the initialization by hand, in order
        //    //       to ensure we will not segfault at startup. */
        //    //    engines = new Engines;
        //    //}
        //    //(*engines)[name] = f;
        //}

        Script create(string engine)
        {
            //if (engines)
            //{
            //    Engines::const_iterator i = engines.find(engine);
            //    if (i != engines.end())
            //    {
            //        return i.second();
            //    }
            //}
            //LOG_ERROR("No scripting engine named " << engine);
            return null;
        }

        public void update()
        {
            //prepare("update");
            //execute();
        }

        //static char skipPotentialBom(char *text)
        //{
        //    // Based on the C version of bomstrip
        //    const char * const utf8Bom = "\xef\xbb\xbf";
        //    const int bomLength = strlen(utf8Bom);
        //    return (strncmp(text, utf8Bom, bomLength) == 0) ? text + bomLength : text;
        //}

        bool loadFile(string name)
        {
            //int size;
            //char *buffer = ResourceManager::loadFile(name, size);
            //if (buffer)
            //{
            //    mScriptFile = name;
            //    load(skipPotentialBom(buffer), name.c_str());
            //    free(buffer);
            //    return true;
            //} else {
            //    return false;
            //}

            return true; //ssk
        }

        void loadNPC(string name, int id, int x, int y, char prog)
        {
            //load(prog, name.c_str());
            //prepare("create_npc_delayed");
            //push(name);
            //push(id);
            //push(x);
            //push(y);
            //execute();
        }

        public static bool loadGlobalEventScript(string file)
        {
            //std::string engineName = determineEngineByFilename(file);
            //if (Script *script = Script::create(engineName))
            //{
            //    globalEventScript = script;
            //    return globalEventScript.loadFile(file);
            //}
            return false;
        }

        bool executeGlobalEventFunction(string function, Being obj)
        {
            bool isScriptHandled=false;
            //if (Script *script = globalEventScript)
            //{
            //    script.setMap(obj.getMap());
            //    script.prepare(function);
            //    script.push(obj);
            //    script.execute();
            //    script.setMap(NULL);
            //    isScriptHandled = true; // TODO: don't set to true when execution failed
            //}
            return isScriptHandled;
        }

        void addDataToSpecial(int id, Special special)
        {
            /* currently only gets the recharge cost.
			  TODO: get any other info in a similar way, but
					first we have to agree on what other
					info we actually want to provide.
			*/
            //if (special)
            //{
            //    if (Script *script = globalEventScript)
            //    {
            //        script.prepare("get_special_recharge_cost");
            //        script.push(id);
            //        int scriptReturn = script.execute();
            //        special.neededMana = scriptReturn;
            //    }
            //}

        }

        bool performSpecialAction(int specialId, Being caster)
        {
            //if (Script *script = globalEventScript)
            //{
            //    script.prepare("use_special");
            //    script.push(caster);
            //    script.push(specialId);
            //    script.execute();
            //}
            return true;
        }

        bool performCraft(Being crafter, List<InventoryItem> recipe)
        {
            //if (Script *script = globalEventScript)
            //{
            //    script.prepare("on_craft");
            //    script.push(crafter);
            //    script.push(recipe);
            //    script.execute();
            //}
            return true;
        }

        //string determineEngineByFilename(string filename)
        //{
        //    //std::string ext = filename.substr(filename.find_last_of(".") + 1);

        //    //if (ext == "lua")
        //    //{
        //    //    return "lua";
        //    //}
        //    //else
        //    //{
        //    //    // Set to default engine and print warning
        //    //    LOG_WARN("Unknown file extension for script \""
        //    //            + filename + "\", falling back to default script engine");
        //    //    return Configuration::getValue("script_defaultEngine", "lua");
        //    //}
        //}
    }
}
