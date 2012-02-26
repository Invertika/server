using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class SkillManager
	{
		public SkillManager(string skillFile)
		{
			//         mSkillFile(skillFile),
			//mDefaultSkillId(0)
		}

		~SkillManager()
		{
			clear();
		}

		void clear()
		{
			//for (SkillsInfo::iterator it = mSkillsInfo.begin(),
			//     it_end = mSkillsInfo.end(); it != it_end; ++it)
			//{
			//    delete it->second;
			//}

			//mSkillsInfo.clear();
			//mNamedSkillsInfo.clear();
		}

		public void initialize()
		{
			//clear();

			//XML::Document doc(mSkillFile);
			//xmlNodePtr rootNode = doc.rootNode();

			//if (!rootNode || !xmlStrEqual(rootNode->name, BAD_CAST "skills"))
			//{
			//    LOG_ERROR("Skill Manager: " << mSkillFile
			//              << " is not a valid database file!");
			//    return;
			//}

			//LOG_INFO("Loading skill reference: " << mSkillFile);

			//for_each_xml_child_node(setNode, rootNode)
			//{
			//    // The server will prefix the core name with the set, so we need one.
			//    if (!xmlStrEqual(setNode->name, BAD_CAST "set"))
			//        continue;

			//    std::string setName = XML::getProperty(setNode, "name", std::string());
			//    if (setName.empty())
			//    {
			//        LOG_WARN("The " << mSkillFile << " file is containing unamed <set> "
			//                 "tags and will be ignored.");
			//        continue;
			//    }

			//    setName = utils::toLower(setName);

			//    for_each_xml_child_node(skillNode, setNode)
			//        readSkillNode(skillNode, setName);
			//}

			//printDebugSkillTable();

			//if (!mDefaultSkillId)
			//    LOG_WARN("SkillManager: No default weapon-type id was given during "
			//             "Skill map loading. "
			//             "Players won't be able to earn XP when unarmed.");

			//LOG_INFO("Loaded " << mSkillsInfo.size() << " skills from "
			//         << mSkillFile);
		}

		void readSkillNode()//xmlNodePtr skillNode,       const std::string& setName)
		{
			//if (!xmlStrEqual(skillNode->name, BAD_CAST "skill"))
			//    return;

			//SkillInfo *skillInfo = new SkillInfo;
			//skillInfo->setName = setName;
			//skillInfo->skillName = utils::toLower(
			//    XML::getProperty(skillNode, "name", std::string()));
			//int id = XML::getProperty(skillNode, "id", 0);

			//if (id <= 0 || skillInfo->skillName.empty())
			//{
			//    LOG_WARN("Invalid skill (empty name or id <= 0) in set: " << setName);
			//    return;
			//}
			//skillInfo->id = (unsigned)id;

			//SkillsInfo::iterator it = mSkillsInfo.find(skillInfo->id);
			//if (it != mSkillsInfo.end())
			//{
			//    LOG_WARN("SkillManager: The same id: " << skillInfo->id
			//    << " is given for skill names: " << it->first
			//    << " and " << skillInfo->skillName);
			//    LOG_WARN("The skill reference: " << skillInfo->id
			//    << ": '" << skillInfo->skillName << "' will be ignored.");
			//    return;
			//}

			//if (XML::getBoolProperty(skillNode, "default", false))
			//{
			//    if (mDefaultSkillId)
			//    {
			//        LOG_WARN("SkillManager: "
			//        "Default Skill id already defined as "
			//        << mDefaultSkillId
			//        << ". Redefinit it as: " << skillInfo->id);
			//    }
			//    else
			//    {
			//        LOG_INFO("SkillManager: Defining skill id: " << skillInfo->id
			//        << " as default weapon-type id.");
			//    }
			//    mDefaultSkillId = skillInfo->id;
			//}

			//mSkillsInfo.insert(
			//    std::make_pair<unsigned int, SkillInfo*>(skillInfo->id, skillInfo));

			//std::string keyName = setName + "_" + skillInfo->skillName;
			//mNamedSkillsInfo.insert(keyName, skillInfo);
		}

		void printDebugSkillTable()
		{
			//if (::utils::Logger::mVerbosity >= ::utils::Logger::Debug)
			//{
			//    std::string lastSet;
			//    LOG_DEBUG("Skill map in " << mSkillFile << ":"
			//              << std::endl << "-----");
			//    for (SkillsInfo::iterator it = mSkillsInfo.begin();
			//        it != mSkillsInfo.end(); ++it)
			//    {
			//        if (!lastSet.compare(it->second->setName))
			//        {
			//            lastSet = it->second->setName;
			//            LOG_DEBUG("Skill set: " << lastSet);
			//        }

			//        if (it->first == mDefaultSkillId)
			//        {
			//            LOG_DEBUG("'" << it->first << "': " << it->second->skillName
			//                      << " (Default)");
			//        }
			//        else
			//        {
			//            LOG_DEBUG("'" << it->first << "': " << it->second->skillName);
			//        }
			//    }
			//    LOG_DEBUG("-----");
			//}
		}

		uint getId(string set, string name)
		{
			//std::string key = utils::toLower(set) + "_" + utils::toLower(name);
			//SkillInfo *skillInfo = mNamedSkillsInfo.value(key);
			//return skillInfo ? skillInfo->id : 0;

			return 0; //ssk
		}

		string getSkillName(uint id)
		{
			//SkillsInfo::const_iterator it = mSkillsInfo.find(id);
			//return it != mSkillsInfo.end() ? it->second->skillName : "";

			return ""; //ssk
		}

		string getSetName(uint id)
		{
			//SkillsInfo::const_iterator it = mSkillsInfo.find(id);
			//return it != mSkillsInfo.end() ? it->second->setName : "";

			return ""; //ssk
		}
	}
}