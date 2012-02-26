﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class MonsterManager
	{
		//      typedef std::map< int, MonsterClass * > MonsterClasses;
		//MonsterClasses mMonsterClasses; /**< Monster reference */
		//utils::NameMap<MonsterClass*> mMonsterClassesByName;

		//std::string mMonsterReferenceFile;

		public MonsterManager(string file)
		{
			//mMonsterReferenceFile(file)
		}

		void reload()
		{
			//deinitialize();
			//initialize();
		}

		public void initialize()
		{
			//XML::Document doc(mMonsterReferenceFile);
			//xmlNodePtr rootNode = doc.rootNode();

			//if (!rootNode || !xmlStrEqual(rootNode->name, BAD_CAST "monsters"))
			//{
			//    LOG_ERROR("Monster Manager: Error while parsing monster database ("
			//              << mMonsterReferenceFile << ")!");
			//    return;
			//}

			//LOG_INFO("Loading monster reference: " << mMonsterReferenceFile);
			//int nbMonsters = 0;
			//for_each_xml_child_node(node, rootNode)
			//{
			//    if (!xmlStrEqual(node->name, BAD_CAST "monster"))
			//        continue;

			//    int id = XML::getProperty(node, "id", 0);
			//    std::string name = XML::getProperty(node, "name", std::string());

			//    if (id < 1)
			//    {
			//        LOG_WARN("Monster Manager: Ignoring monster ("
			//                 << name << ") without Id in "
			//                 << mMonsterReferenceFile << "! It has been ignored.");
			//        continue;
			//    }

			//    MonsterClasses::iterator i = mMonsterClasses.find(id);
			//    if (i != mMonsterClasses.end())
			//    {
			//        LOG_WARN("Monster Manager: Ignoring duplicate definition of "
			//                 "monster '" << id << "'!");
			//        continue;
			//    }

			//    MonsterClass *monster = new MonsterClass(id);
			//    mMonsterClasses[id] = monster;

			//    if (!name.empty())
			//    {
			//        monster->setName(name);

			//        if (mMonsterClassesByName.contains(name))
			//            LOG_WARN("Monster Manager: Name not unique for monster " << id);
			//        else
			//            mMonsterClassesByName.insert(name, monster);
			//    }

			//    MonsterDrops drops;
			//    bool attributesSet = false;
			//    bool behaviorSet = false;

			//    for_each_xml_child_node(subnode, node)
			//    {
			//        if (xmlStrEqual(subnode->name, BAD_CAST "drop"))
			//        {
			//            MonsterDrop drop;
			//            drop.item = itemManager->getItem(
			//                                      XML::getProperty(subnode, "item", 0));
			//            drop.probability = XML::getFloatProperty(subnode, "percent",
			//                                                     0.0) * 100 + 0.5;

			//            if (drop.item && drop.probability)
			//                drops.push_back(drop);
			//        }
			//        else if (xmlStrEqual(subnode->name, BAD_CAST "attributes"))
			//        {
			//            attributesSet = true;

			//            const int hp = XML::getProperty(subnode, "hp", -1);
			//            monster->setAttribute(ATTR_MAX_HP, hp);
			//            monster->setAttribute(ATTR_HP, hp);

			//            monster->setAttribute(MOB_ATTR_PHY_ATK_MIN,
			//                XML::getProperty(subnode, "attack-min", -1));
			//            monster->setAttribute(MOB_ATTR_PHY_ATK_DELTA,
			//                XML::getProperty(subnode, "attack-delta", -1));
			//            monster->setAttribute(MOB_ATTR_MAG_ATK,
			//                XML::getProperty(subnode, "attack-magic", -1));
			//            monster->setAttribute(ATTR_DODGE,
			//                XML::getProperty(subnode, "evade", -1));
			//            monster->setAttribute(ATTR_MAGIC_DODGE,
			//                XML::getProperty(subnode, "magic-evade", -1));
			//            monster->setAttribute(ATTR_ACCURACY,
			//                XML::getProperty(subnode, "hit", -1));
			//            monster->setAttribute(ATTR_DEFENSE,
			//                XML::getProperty(subnode, "physical-defence", -1));
			//            monster->setAttribute(ATTR_MAGIC_DEFENSE,
			//                XML::getProperty(subnode, "magical-defence", -1));
			//            monster->setSize(XML::getProperty(subnode, "size", -1));
			//            float speed = (XML::getFloatProperty(subnode, "speed", -1.0f));
			//            monster->setMutation(XML::getProperty(subnode, "mutation", 0));
			//            std::string genderString = XML::getProperty(subnode, "gender",
			//                                                        std::string());
			//            monster->setGender(getGender(genderString));

			//            // Checking attributes for completeness and plausibility
			//            if (monster->getMutation() > MAX_MUTATION)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                << ": Mutation of monster Id:" << id << " more than "
			//                << MAX_MUTATION << "%. Defaulted to 0.");
			//                monster->setMutation(0);
			//            }

			//            bool attributesComplete = true;
			//            const AttributeScope &mobAttr =
			//                        attributeManager->getAttributeScope(MonsterScope);

			//            for (AttributeScope::const_iterator it = mobAttr.begin(),
			//                 it_end = mobAttr.end(); it != it_end; ++it)
			//            {
			//                if (!monster->mAttributes.count(it->first))
			//                {
			//                    LOG_WARN(mMonsterReferenceFile << ": No attribute "
			//                             << it->first << " for monster Id: "
			//                             << id << ". Defaulted to 0.");
			//                    attributesComplete = false;
			//                    monster->setAttribute(it->first, 0);
			//                }
			//            }

			//            if (monster->getSize() == -1)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": No size set for monster Id:" << id << ". "
			//                         << "Defaulted to " << DEFAULT_MONSTER_SIZE
			//                         << " pixels.");
			//                monster->setSize(DEFAULT_MONSTER_SIZE);
			//                attributesComplete = false;
			//            }

			//            if (speed == -1.0f)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": No speed set for monster Id:" << id << ". "
			//                         << "Defaulted to " << DEFAULT_MONSTER_SPEED
			//                         << " tiles/second.");
			//                speed = DEFAULT_MONSTER_SPEED;
			//                attributesComplete = false;
			//            }
			//            monster->setAttribute(ATTR_MOVE_SPEED_TPS, speed);

			//            if (!attributesComplete)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": Attributes incomplete for monster Id:" << id
			//                         << ". Defaults values may have been applied!");
			//            }

			//        }
			//        else if (xmlStrEqual(subnode->name, BAD_CAST "exp"))
			//        {
			//            xmlChar *exp = subnode->xmlChildrenNode->content;
			//            monster->setExp(atoi((const char*)exp));
			//            monster->setOptimalLevel(XML::getProperty(subnode, "level", 0));
			//        }
			//        else if (xmlStrEqual(subnode->name, BAD_CAST "behavior"))
			//        {
			//            behaviorSet = true;
			//            if (XML::getBoolProperty(subnode, "aggressive", false))
			//                monster->setAggressive(true);

			//            monster->setTrackRange(
			//                           XML::getProperty(subnode, "track-range", 1));
			//            monster->setStrollRange(
			//                           XML::getProperty(subnode, "stroll-range", 0));
			//            monster->setAttackDistance(
			//                           XML::getProperty(subnode, "attack-distance", 0));
			//        }
			//        else if (xmlStrEqual(subnode->name, BAD_CAST "attack"))
			//        {
			//            MonsterAttack *att = new MonsterAttack;
			//            att->id = XML::getProperty(subnode, "id", 0);
			//            att->priority = XML::getProperty(subnode, "priority", 1);
			//            att->damageFactor = XML::getFloatProperty(subnode,
			//                                                     "damage-factor", 1.0f);
			//            att->preDelay = XML::getProperty(subnode, "pre-delay", 1);
			//            att->aftDelay = XML::getProperty(subnode, "aft-delay", 0);
			//            att->range = XML::getProperty(subnode, "range", 0);
			//            att->scriptFunction = XML::getProperty(subnode,
			//                                                   "script-function",
			//                                                   std::string());
			//            std::string sElement = XML::getProperty(subnode,
			//                                                    "element", "neutral");
			//            att->element = elementFromString(sElement);
			//            std::string sType = XML::getProperty(subnode,
			//                                                 "type", "physical");

			//            bool validMonsterAttack = true;
			//            if (sType == "physical")
			//            {
			//                att->type = DAMAGE_PHYSICAL;
			//            }
			//            else if (sType == "magical" || sType == "magic")
			//            {
			//                att->type = DAMAGE_MAGICAL;
			//            }
			//            else if (sType == "other")
			//            {
			//                att->type = DAMAGE_OTHER;
			//            }
			//            else
			//            {
			//                LOG_WARN("Monster manager " << mMonsterReferenceFile
			//                          <<  ": unknown damage type '" << sType << "'.");
			//                validMonsterAttack = false;
			//            }

			//            if (att->id < 1)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": Attack without ID for monster Id:"
			//                         << id << " (" << name << ") - attack ignored");
			//                validMonsterAttack = false;
			//            }
			//            else if (att->element == ELEMENT_ILLEGAL)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": Attack with unknown element \""
			//                         << sElement << "\" for monster Id:" << id
			//                         << " (" << name << ") - attack ignored");
			//                validMonsterAttack = false;
			//            }
			//            else if (att->type == -1)
			//            {
			//                LOG_WARN(mMonsterReferenceFile
			//                         << ": Attack with unknown type \"" << sType << "\""
			//                         << " for monster Id:" << id
			//                         << " (" << name << ")");
			//                validMonsterAttack = false;
			//            }

			//            if (validMonsterAttack)
			//            {
			//                monster->addAttack(att);
			//            }
			//            else
			//            {
			//                delete att;
			//                att = 0;
			//            }

			//        }
			//        else if (xmlStrEqual(subnode->name, BAD_CAST "script"))
			//        {
			//            xmlChar *filename = subnode->xmlChildrenNode->content;
			//            std::string val = (char *)filename;
			//            monster->setScript(val);
			//        }
			//    }

			//    monster->setDrops(drops);
			//    if (!attributesSet)
			//    {
			//        LOG_WARN(mMonsterReferenceFile
			//                 << ": No attributes defined for monster Id:" << id
			//                 << " (" << name << ")");
			//    }
			//    if (!behaviorSet)
			//    {
			//        LOG_WARN(mMonsterReferenceFile
			//            << ": No behavior defined for monster Id:" << id
			//            << " (" << name << ")");
			//    }
			//    if (monster->getExp() == -1)
			//    {
			//        LOG_WARN(mMonsterReferenceFile
			//                << ": No experience defined for monster Id:" << id
			//                << " (" << name << ")");
			//        monster->setExp(0);
			//    }
			//    ++nbMonsters;
			//}

			//LOG_INFO("Loaded " << nbMonsters << " monsters from "
			//         << mMonsterReferenceFile << '.');
		}

		void deinitialize()
		{
			//for (MonsterClasses::iterator i = mMonsterClasses.begin(),
			//     i_end = mMonsterClasses.end(); i != i_end; ++i)
			//{
			//    delete i->second;
			//}
			//mMonsterClasses.clear();
			//mMonsterClassesByName.clear();
		}

		MonsterClass getMonsterByName(string name)
		{
			//return mMonsterClassesByName.value(name);

			return null; //ssk
		}

		MonsterClass getMonster(int id)
		{
			//MonsterClasses::const_iterator i = mMonsterClasses.find(id);
			//return i != mMonsterClasses.end() ? i->second : 0;

			return null; //ssk
		}
	}
}
