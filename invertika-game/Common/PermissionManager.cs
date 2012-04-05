//
//  PermissionManager.cs
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

namespace invertika_game.Common
{
	public static class PermissionManager
	{

		//static std::map<std::string, unsigned char> permissions;
		//static std::map<std::string, unsigned char> aliases;
		//static std::string permissionFile;

		static void addPermission(string permission, char mask)
		{
			//std::map<std::string, unsigned char>::iterator i = permissions.find(permission);
			//if (i == permissions.end())
			//{
			//    permissions.insert(std::make_pair<std::string, unsigned char>(permission, mask));
			//} else {
			//    i.second |= mask;
			//}
		}

		public static void initialize(string file)
		{
			//permissionFile = file;
			//reload();
		}

		static void reload()
		{
			//XML::Document doc(permissionFile);
			//xmlNodePtr rootNode = doc.rootNode();

			//if (!rootNode || !xmlStrEqual(rootNode.name, BAD_CAST "permissions"))
			//{
			//    LOG_ERROR("Permission Manager: " << permissionFile
			//              << " is not a valid database file!");
			//    return;
			//}

			//LOG_INFO("Loading permission reference...");
			//for_each_xml_child_node(node, rootNode)
			//{
			//    unsigned char classmask = 0x01;
			//    if (!xmlStrEqual(node.name, BAD_CAST "class"))
			//    {
			//        continue;
			//    }
			//    int level = XML::getProperty(node, "level", 0);
			//    if (level < 1 || level > 8)
			//    {
			//        LOG_WARN("PermissionManager: Illegal class level "
			//                <<level
			//                <<" in "
			//                <<permissionFile
			//                <<" (allowed range: 1..8)");
			//        continue;
			//    }
			//    classmask = classmask << (level-1);


			//    xmlNodePtr perNode;
			//    for (perNode = node.xmlChildrenNode; perNode != NULL; perNode = perNode.next)
			//    {
			//        if (xmlStrEqual(perNode.name, BAD_CAST "allow"))
			//        {
			//            const char* permission = (const char*)perNode.xmlChildrenNode.content;
			//            if (permission && strlen(permission) > 0)
			//            {
			//                addPermission(permission, classmask);
			//            }
			//        } else if (xmlStrEqual(perNode.name, BAD_CAST "deny")){
			//            //const char* permission = (const char*)perNode.xmlChildrenNode.content;
			//            // To be implemented
			//        } else if (xmlStrEqual(perNode.name, BAD_CAST "alias")){
			//            const char* alias = (const char*)perNode.xmlChildrenNode.content;
			//            if (alias && strlen(alias) > 0)
			//            aliases[alias] = classmask;
			//        }
			//    }
			//}
		}


		static Result checkPermission(Character character, string permission)
		{
			//return checkPermission(character.getAccountLevel(), permission);

			return Result.PMR_ALLOWED; //ssk
		}

		static Result checkPermission(byte level, string permission)
		{
			//std::map<std::string, unsigned char>::iterator iP = permissions.find(permission);

			//if (iP == permissions.end())
			//{
			//    LOG_WARN("PermissionManager: Check for unknown permission \""<<permission<<"\" requested.");
			//    return PMR_UNKNOWN;
			//}
			//if (level & iP.second)
			//{
			//    return PMR_ALLOWED;
			//} else {
			//    return PMR_DENIED;
			//}

			return Result.PMR_ALLOWED; //ssk
		}

		static byte getMaskFromAlias(string alias)
		{
			//std::map<std::string, unsigned char>::iterator i = aliases.find(alias);

			//if (i == aliases.end())
			//{
			//    return 0x00;
			//} else {
			//    return i.second;
			//}

			return 0; //ssk
		}

		static List<string> getPermissionList(Character character)
		{
			//std::list<std::string> result;
			//std::map<std::string, unsigned char>::iterator i;

			//unsigned char mask = character.getAccountLevel();

			//for (i = permissions.begin(); i != permissions.end(); i++)
			//{
			//    if (i.second & mask)
			//    {
			//        result.push_back(i.first);
			//    }
			//}

			//return result;

			return null; //ssk
		}

		static List<string> getClassList(Character character)
		{
			//std::list<std::string> result;
			//std::map<std::string, unsigned char>::iterator i;

			//unsigned char mask = character.getAccountLevel();

			//for (i = aliases.begin(); i != aliases.end(); i++)
			//{
			//    if (i.second & mask)
			//    {
			//        result.push_back(i.first);
			//    }
			//}

			//return result;

			return null; //ssk
		}
	}
}
