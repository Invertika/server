//
//  AttributeManager.cs
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
	public class AttributeManager
	{
		public AttributeManager(string file)
        {
			 //mAttributeReferenceFile(file)
		}

		public void initialize()
		{
			//reload();
		}

		void reload()
		{
			//mTagMap.clear();
			//mAttributeMap.clear();
			//for (unsigned int i = 0; i < MaxScope; ++i)
			//    mAttributeScopes[i].clear();

			//readAttributesFile();

			//LOG_DEBUG("attribute map:");
			//LOG_DEBUG("Stackable is " << Stackable << ", NonStackable is " << NonStackable
			//          << ", NonStackableBonus is " << NonStackableBonus << ".");
			//LOG_DEBUG("Additive is " << Additive << ", Multiplicative is " << Multiplicative << ".");
			//const std::string *tag;
			//unsigned int count = 0;
			//for (AttributeMap::const_iterator i = mAttributeMap.begin();
			//     i != mAttributeMap.end(); ++i)
			//{
			//    unsigned int lCount = 0;
			//    LOG_DEBUG("  "<<i.first<<" : ");
			//    for (std::vector<struct AttributeInfoType>::const_iterator j = i.second.second.begin();
			//         j != i.second.second.end();
			//         ++j)
			//    {
			//        tag = getTag(ModifierLocation(i.first, lCount));
			//        std::string end = tag ? "tag of '" + (*tag) + "'." : "no tag.";
			//        LOG_DEBUG("    stackableType: " << j.stackableType
			//                  << ", effectType: " << j.effectType << ", and " << end);
			//        ++lCount;
			//        ++count;
			//    }
			//}
			//LOG_INFO("Loaded '" << mAttributeMap.size() << "' attributes with '"
			//         << count << "' modifier layers.");

			//for (TagMap::const_iterator i = mTagMap.begin(), i_end = mTagMap.end();
			//     i != i_end; ++i)
			//{
			//    LOG_DEBUG("Tag '" << i.first << "': '" << i.second.attributeId
			//              << "', '" << i.second.layer << "'.");
			//}

			//LOG_INFO("Loaded '" << mTagMap.size() << "' modifier tags.");
		}

		List<AttributeInfoType> getAttributeInfo(int id)
		{
			//AttributeMap::const_iterator ret = mAttributeMap.find(id);
			//if (ret == mAttributeMap.end())
			//    return 0;
			//return &ret.second.second;

			return null; //ssk
		}

		//typedef std::map<int, std::vector<struct AttributeInfoType> *> AttributeScope;


		public Dictionary<int, List<AttributeInfoType>> getAttributeScope(ScopeType type)
		{
			//return mAttributeScopes[type];

			return null; //ssk
		}

		bool isAttributeDirectlyModifiable(int id)
		{
			//AttributeMap::const_iterator ret = mAttributeMap.find(id);
			//if (ret == mAttributeMap.end())
			//    return false;
			//return ret.second.first;

			return true; //ssk
		}

		ModifierLocation getLocation(string tag)
		{
			//if (mTagMap.find(tag) != mTagMap.end())
			//    return mTagMap.at(tag);
			//else
			//    return ModifierLocation(0, 0);

			return null; //ssk
		}

		string getTag(ModifierLocation location)
		{
			//for (TagMap::const_iterator it = mTagMap.begin(),
			//     it_end = mTagMap.end(); it != it_end; ++it)
			//    if (it.second == location)
			//        return &it.first;
			return null;
		}

		void readAttributesFile()
		{
			//XML::Document doc(mAttributeReferenceFile);
			//xmlNodePtr node = doc.rootNode();

			//if (!node || !xmlStrEqual(node.name, BAD_CAST "attributes"))
			//{
			//    LOG_FATAL("Attribute Manager: " << mAttributeReferenceFile
			//              << " is not a valid database file!");
			//    exit(EXIT_XML_BAD_PARAMETER);
			//}

			//LOG_INFO("Loading attribute reference...");

			//for_each_xml_child_node(childNode, node)
			//{
			//    if (xmlStrEqual(childNode.name, BAD_CAST "attribute"))
			//        readAttributeNode(childNode);
			//}
		}

		void readAttributeNode()//xmlNodePtr attributeNode)
		{
			//int id = XML::getProperty(attributeNode, "id", 0);

			//if (id <= 0)
			//{
			//    LOG_WARN("Attribute manager: attribute '" << id
			//             << "' has an invalid id and will be ignored.");
			//    return;
			//}

			//mAttributeMap[id] =
			//        AttributeInfoMap(false, std::vector<struct AttributeInfoType>());

			//for_each_xml_child_node(subNode, attributeNode)
			//{
			//    if (xmlStrEqual(subNode.name, BAD_CAST "modifier"))
			//    {
			//        readModifierNode(subNode, id);
			//    }
			//}

			//const std::string scope = utils::toUpper(
			//            XML::getProperty(attributeNode, "scope", std::string()));

			//if (scope.empty())
			//{
			//    // Give a warning unless scope has been explicitly set to "NONE"
			//    LOG_WARN("Attribute manager: attribute '" << id
			//             << "' has no default scope.");
			//}
			//else if (scope == "CHARACTER")
			//{
			//    mAttributeScopes[CharacterScope][id] = &mAttributeMap.at(id).second;
			//    LOG_DEBUG("Attribute manager: attribute '" << id
			//              << "' added to default character scope.");
			//}
			//else if (scope == "MONSTER")
			//{
			//    mAttributeScopes[MonsterScope][id] = &mAttributeMap.at(id).second;
			//    LOG_DEBUG("Attribute manager: attribute '" << id
			//              << "' added to default monster scope.");
			//}
			//else if (scope == "BEING")
			//{
			//    mAttributeScopes[BeingScope][id] = &mAttributeMap.at(id).second;
			//    LOG_DEBUG("Attribute manager: attribute '" << id
			//              << "' added to default being scope.");
			//}
			//else if (scope == "NONE")
			//{
			//    LOG_DEBUG("Attribute manager: attribute '" << id
			//              << "' set to have no default scope.");
			//}
		}

		void readModifierNode()//xmlNodePtr modifierNode, int attributeId)
		{
			//const std::string stackableTypeString = utils::toUpper(
			//            XML::getProperty(modifierNode, "stacktype", std::string()));
			//const std::string effectTypeString = utils::toUpper(
			//            XML::getProperty(modifierNode, "modtype", std::string()));
			//const std::string tag = XML::getProperty(modifierNode, "tag",
			//                                         std::string());

			//if (stackableTypeString.empty())
			//{
			//    LOG_WARN("Attribute manager: attribute '" << attributeId <<
			//             "' has undefined stackable type, skipping modifier!");
			//    return;
			//}

			//if (effectTypeString.empty())
			//{
			//    LOG_WARN("Attribute manager: attribute '" << attributeId
			//             << "' has undefined modification type, skipping modifier!");
			//    return;
			//}

			//StackableType stackableType;
			//ModifierEffectType effectType;

			//if (stackableTypeString == "STACKABLE")
			//    stackableType = Stackable;
			//else if (stackableTypeString == "NON STACKABLE")
			//    stackableType = NonStackable;
			//else if (stackableTypeString == "NON STACKABLE BONUS")
			//    stackableType = NonStackableBonus;
			//else
			//{
			//    LOG_WARN("Attribute manager: attribute '"
			//             << attributeId << "' has unknown stackable type '"
			//             << stackableTypeString << "', skipping modifier!");
			//    return;
			//}

			//if (effectTypeString == "ADDITIVE")
			//    effectType = Additive;
			//else if (effectTypeString == "MULTIPLICATIVE")
			//    effectType = Multiplicative;
			//else
			//{
			//    LOG_WARN("Attribute manager: attribute '" << attributeId
			//             << "' has unknown modification type '"
			//             << effectTypeString << "', skipping modifier!");
			//    return;
			//}

			//mAttributeMap[attributeId].second.push_back(
			//            AttributeInfoType(stackableType, effectType));

			//if (!tag.empty())
			//{
			//    const int layer = mAttributeMap[attributeId].second.size() - 1;
			//    mTagMap.insert(std::make_pair(tag, ModifierLocation(attributeId,
			//                                                        layer)));
			//}
		}
	}
}
