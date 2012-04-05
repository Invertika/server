//
//  StatusManager.cs
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

namespace invertika_game.Game
{
	public static class StatusManager
	{
		////typedef std::map< int, StatusEffect * > StatusEffectsMap;
		//static Dictionary< int, StatusEffect * > statusEffects;
		//static std::string statusReferenceFile;

		public static void initialize(string file)
		{
			//statusReferenceFile = file;
			//reload();
		}

		static void reload()
		{
			//    XML::Document doc(statusReferenceFile);
			//    xmlNodePtr rootNode = doc.rootNode();

			//    if (!rootNode || !xmlStrEqual(rootNode.name, BAD_CAST "status-effects"))
			//    {
			//        LOG_ERROR("Status Manager: Error while parsing status database ("
			//                  << statusReferenceFile << ")!");
			//        return;
			//    }

			//    LOG_INFO("Loading status reference: " << statusReferenceFile);
			//    for_each_xml_child_node(node, rootNode)
			//    {
			//        if (!xmlStrEqual(node.name, BAD_CAST "status-effect"))
			//            continue;

			//        int id = XML::getProperty(node, "id", 0);
			//        if (id < 1)
			//        {
			//            LOG_WARN("Status Manager: The status ID: " << id << " in "
			//                     << statusReferenceFile
			//                     << " is invalid and will be ignored.");
			//            continue;
			//        }

			//        std::string scriptFile = XML::getProperty(node, "script", std::string());
			//        //TODO: Get these modifiers
			///*
			//        modifiers.setAttributeValue(BASE_ATTR_PHY_ATK_MIN,      XML::getProperty(node, "attack-min",      0));
			//        modifiers.setAttributeValue(BASE_ATTR_PHY_ATK_DELTA,      XML::getProperty(node, "attack-delta",      0));
			//        modifiers.setAttributeValue(BASE_ATTR_HP,      XML::getProperty(node, "hp",      0));
			//        modifiers.setAttributeValue(BASE_ATTR_PHY_RES, XML::getProperty(node, "defense", 0));
			//        modifiers.setAttributeValue(CHAR_ATTR_STRENGTH,     XML::getProperty(node, "strength",     0));
			//        modifiers.setAttributeValue(CHAR_ATTR_AGILITY,      XML::getProperty(node, "agility",      0));
			//        modifiers.setAttributeValue(CHAR_ATTR_DEXTERITY,    XML::getProperty(node, "dexterity",    0));
			//        modifiers.setAttributeValue(CHAR_ATTR_VITALITY,     XML::getProperty(node, "vitality",     0));
			//        modifiers.setAttributeValue(CHAR_ATTR_INTELLIGENCE, XML::getProperty(node, "intelligence", 0));
			//        modifiers.setAttributeValue(CHAR_ATTR_WILLPOWER,    XML::getProperty(node, "willpower",    0));
			//*/
			//        StatusEffect *statusEffect = new StatusEffect(id);
			//        if (!scriptFile.empty())
			//        {
			//            std::stringstream filename;
			//            filename << "scripts/status/" << scriptFile;
			//            if (ResourceManager::exists(filename.str()))       // file exists!
			//            {
			//                LOG_INFO("Loading status script: " << filename.str());
			//                std::string engineName =
			//                        Script::determineEngineByFilename(filename.str());
			//                Script *s = Script::create(engineName);
			//                s.loadFile(filename.str());
			//                statusEffect.setScript(s);
			//            } else {
			//                LOG_WARN("Could not find script file \"" << filename.str()
			//                         << "\" for status #"<<id);
			//            }
			//        }
			//        statusEffects[id] = statusEffect;
			//    }
		}

		static void deinitialize()
		{
			//for (StatusEffectsMap::iterator i = statusEffects.begin(),
			//       i_end = statusEffects.end(); i != i_end; ++i)
			//{
			//    delete i.second;
			//}
			//statusEffects.clear();
		}

		static StatusEffect getStatus(int statusId)
		{
			//StatusEffectsMap::const_iterator i = statusEffects.find(statusId);
			//return i != statusEffects.end() ? i.second : NULL;

			return null; //ssk
		}
	}
}
