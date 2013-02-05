//
//  MapManager.cs
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
using CSCL;
using System.Xml;
using ISL.Server.Common;

namespace invertika_game.Game
{
    public static class MapManager
    {
        static Dictionary<int, MapComposite> maps=new Dictionary<int, MapComposite>();

        public static Dictionary<int, MapComposite> getMaps()
        {
            return maps;
        }

        public static int initialize(string mapReferenceFile)
        {
            // Indicates the number of maps loaded successfully
            int loadedMaps=0;

            XmlData doc=new XmlData(mapReferenceFile);

            if(doc.ExistElement("maps")==false)
            {
                Logger.Write(LogLevel.Error, "Item Manager: Error while parsing map database ({0})!", mapReferenceFile);
                return loadedMaps;
            }

            Logger.Write(LogLevel.Information, "Loading map reference: {0}", mapReferenceFile);

            //Für jeden Mapknoten
            List<XmlNode> nodes=doc.GetElements("maps.map");

            foreach(XmlNode node in nodes)
            {
                if(node.Name!="map")
                    continue;

                int id=Convert.ToInt32(node.Attributes["id"].Value);
                string name=node.Attributes["name"].Value;

                if(id>0&&name!="")
                {
                    // Testing if the file is actually in the maps folder
                    string file="maps/"+name+".tmx";
                    bool mapFileExists=ResourceManager.exists(file);

                    if(mapFileExists)
                    {
                        maps[id]=new MapComposite(id, name);
                        ++loadedMaps;
                    }
                }
                else
                {
                    if(name=="")
                    {
                        Logger.Write(LogLevel.Warning, "Invalid unnamed map Id: {0}.", id);
                    }
                    else
                    {
                        Logger.Write(LogLevel.Warning, "Invalid map Id: {0}.", id);
                    }
                }
            }

            if(loadedMaps>0)
            {
                Logger.Write(LogLevel.Information, "{0} valid map file references were loaded.", loadedMaps);
            }

            return loadedMaps;
        }

        static void deinitialize()
        {
            //for (Maps::iterator i = maps.begin(), i_end = maps.end(); i != i_end; ++i)
            //{
            //    delete i.second;
            //}
            //maps.clear();
        }

        public static MapComposite getMap(int mapId)
        {
            if(maps.ContainsKey(mapId))
            {
                return maps[mapId];
            }
            else
            {
                return null;
            }
        }

        public static MapComposite getMap(string mapName)
        {
            foreach(KeyValuePair<int, MapComposite> pair in maps)
            {
                if(pair.Value.getName()==mapName)
                {
                    return pair.Value;
                }
            }

            return null;
        }

        public static bool activateMap(int mapId)
        {
            MapComposite composite=maps[mapId];

            if(composite.isActive())
            {
                return true;
            }

            if(composite.activate())
            {
                Logger.Write(LogLevel.Information, "Activated map {0} (id {1})", composite.getName(), mapId);
                return true;
            }
            else
            {
                Logger.Write(LogLevel.Warning, "Couldn't activate invalid map {0} (id {1})", composite.getName(), mapId);
                return false;
            }
        }
    }
}
