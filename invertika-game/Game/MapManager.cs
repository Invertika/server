using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public static class MapManager
	{
		static Dictionary<int, MapComposite> maps;

		//const MapManager::Maps &MapManager::getMaps()
		//{
		//    return maps;
		//}

		public static int initialize(string mapReferenceFile)
		{
			//// Indicates the number of maps loaded successfully
			//int loadedMaps = 0;

			//XML::Document doc(mapReferenceFile);
			//xmlNodePtr rootNode = doc.rootNode();

			//if (!rootNode || !xmlStrEqual(rootNode->name, BAD_CAST "maps"))
			//{
			//    LOG_ERROR("Item Manager: Error while parsing map database ("
			//              << mapReferenceFile << ")!");
			//    return loadedMaps;
			//}

			//LOG_INFO("Loading map reference: " << mapReferenceFile);
			//for_each_xml_child_node(node, rootNode)
			//{
			//    if (!xmlStrEqual(node->name, BAD_CAST "map"))
			//        continue;

			//    int id = XML::getProperty(node, "id", 0);
			//    std::string name = XML::getProperty(node, "name", std::string());

			//    // Test id and map name
			//    if (id > 0 && !name.empty())
			//    {
			//        // Testing if the file is actually in the maps folder
			//        std::string file = std::string("maps/") + name + ".tmx";
			//        bool mapFileExists = ResourceManager::exists(file);

			//        // Try to fall back on fully compressed map
			//        if (!mapFileExists)
			//        {
			//            file += ".gz";
			//            mapFileExists = ResourceManager::exists(file);
			//        }

			//        if (mapFileExists)
			//        {
			//            maps[id] = new MapComposite(id, name);
			//            ++loadedMaps;
			//        }
			//    }
			//    else
			//    {
			//        if (name.empty())
			//        {
			//            LOG_WARN("Invalid unnamed map Id: " << id << '.');
			//        }
			//        else
			//        {
			//            LOG_WARN("Invalid map Id: " << id << " for map: "
			//                     << name << '.');
			//        }
			//    }
			//}

			//if (loadedMaps > 0)
			//    LOG_INFO(loadedMaps << " valid map file references were loaded.");

			//return loadedMaps;

			return 0;
		}

		static void deinitialize()
		{
			//for (Maps::iterator i = maps.begin(), i_end = maps.end(); i != i_end; ++i)
			//{
			//    delete i->second;
			//}
			//maps.clear();
		}

		static MapComposite getMap(int mapId)
		{
			//Maps::const_iterator i = maps.find(mapId);
			//return (i != maps.end()) ? i->second : NULL;

			return null; //ssk
		}

		static MapComposite getMap(string mapName)
		{
			//for (Maps::const_iterator i = maps.begin(); i != maps.end(); ++i)
			//    if (i->second->getName() == mapName)
			//        return i->second;

			return null;
		}

		static bool activateMap(int mapId)
		{
			//Maps::iterator i = maps.find(mapId);
			//assert(i != maps.end());
			//MapComposite *composite = i->second;

			//if (composite->isActive())
			//    return true;

			//if (composite->activate())
			//{
			//    LOG_INFO("Activated map \"" << composite->getName()
			//             << "\" (id " << mapId << ")");
			//    return true;
			//}
			//else
			//{
			//    LOG_WARN("Couldn't activate invalid map \"" << composite->getName()
			//             << "\" (id " << mapId << ")");
			//    return false;
			//}

			return true; //ssk
		}
	}
}
