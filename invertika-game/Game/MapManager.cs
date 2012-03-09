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

		//const MapManager::Maps &MapManager::getMaps()
		//{
		//    return maps;
		//}

		public static int initialize(string mapReferenceFile)
		{
			// Indicates the number of maps loaded successfully
			int loadedMaps=0;

			XmlData doc=new XmlData(mapReferenceFile);

			if(doc.ExistElement("maps")==false)
			{
				Logger.Add(LogLevel.Error, "Item Manager: Error while parsing map database ({0})!", mapReferenceFile);
				return loadedMaps;
			}

			Logger.Add(LogLevel.Information, "Loading map reference: {0}", mapReferenceFile);

			//Für jeden Mapknoten
			List<XmlNode> nodes=doc.GetElements("maps.map");

			foreach(XmlNode node in nodes)
			{
				if(node.Name!="map") continue;

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
						Logger.Add(LogLevel.Warning, "Invalid unnamed map Id: {0}.", id);
					}
					else
					{
						Logger.Add(LogLevel.Warning, "Invalid map Id: {0}.", id);
					}
				}
			}

			if(loadedMaps>0)
			{
				Logger.Add(LogLevel.Information, "{0} valid map file references were loaded.", loadedMaps);
			}

			return loadedMaps;
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
