﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Serialize
{
	public class CharacterData
	{
		
//template< class T >
//void serializeCharacterData(const T &data, MessageOut &msg)
//{
//    // general character properties
//    msg.writeInt8(data.getAccountLevel());
//    msg.writeInt8(data.getGender());
//    msg.writeInt8(data.getHairStyle());
//    msg.writeInt8(data.getHairColor());
//    msg.writeInt16(data.getLevel());
//    msg.writeInt16(data.getCharacterPoints());
//    msg.writeInt16(data.getCorrectionPoints());

//    msg.writeInt16(data.mAttributes.size());
//    AttributeMap::const_iterator attr_it, attr_it_end;
//    for (attr_it = data.mAttributes.begin(),
//         attr_it_end = data.mAttributes.end();
//         attr_it != attr_it_end;
//         ++attr_it)
//    {
//        msg.writeInt16(attr_it->first);
//        msg.writeDouble(data.getAttrBase(attr_it));
//        msg.writeDouble(data.getAttrMod(attr_it));
//    }

//    // character skills
//    msg.writeInt16(data.getSkillSize());

//    std::map<int, int>::const_iterator skill_it;
//    for (skill_it = data.getSkillBegin(); skill_it != data.getSkillEnd() ; skill_it++)
//    {
//        msg.writeInt16(skill_it->first);
//        msg.writeInt32(skill_it->second);
//    }

//    // status effects currently affecting the character
//    msg.writeInt16(data.getStatusEffectSize());
//    std::map<int, int>::const_iterator status_it;
//    for (status_it = data.getStatusEffectBegin(); status_it != data.getStatusEffectEnd(); status_it++)
//    {
//        msg.writeInt16(status_it->first);
//        msg.writeInt16(status_it->second);
//    }

//    // location
//    msg.writeInt16(data.getMapId());
//    const Point &pos = data.getPosition();
//    msg.writeInt16(pos.x);
//    msg.writeInt16(pos.y);

//    // kill count
//    msg.writeInt16(data.getKillCountSize());
//    std::map<int, int>::const_iterator kills_it;
//    for (kills_it = data.getKillCountBegin(); kills_it != data.getKillCountEnd(); kills_it++)
//    {
//        msg.writeInt16(kills_it->first);
//        msg.writeInt32(kills_it->second);
//    }

//    // character specials
//    std::map<int, Special*>::const_iterator special_it;
//    msg.writeInt16(data.getSpecialSize());
//    for (special_it = data.getSpecialBegin(); special_it != data.getSpecialEnd() ; special_it++)
//    {
//        msg.writeInt32(special_it->first);
//    }

//    // inventory - must be last because size isn't transmitted
//    const Possessions &poss = data.getPossessions();
//    const EquipData &equipData = poss.getEquipment();
//    msg.writeInt16(equipData.size()); // number of equipment
//    for (EquipData::const_iterator k = equipData.begin(),
//             k_end = equipData.end(); k != k_end; ++k)
//    {
//        msg.writeInt16(k->first);                 // Equip slot id
//        msg.writeInt16(k->second.itemId);         // ItemId
//        msg.writeInt16(k->second.itemInstance);   // Item Instance id
//    }

//    const InventoryData &inventoryData = poss.getInventory();
//    for (InventoryData::const_iterator j = inventoryData.begin(),
//         j_end = inventoryData.end(); j != j_end; ++j)
//    {
//        msg.writeInt16(j->first);           // slot id
//        msg.writeInt16(j->second.itemId);   // item id
//        msg.writeInt16(j->second.amount);   // amount
//    }
//}

//template< class T >
//void deserializeCharacterData(T &data, MessageIn &msg)
//{
//    // general character properties
//    data.setAccountLevel(msg.readInt8());
//    data.setGender(ManaServ::getGender(msg.readInt8()));
//    data.setHairStyle(msg.readInt8());
//    data.setHairColor(msg.readInt8());
//    data.setLevel(msg.readInt16());
//    data.setCharacterPoints(msg.readInt16());
//    data.setCorrectionPoints(msg.readInt16());

//    // character attributes
//    unsigned int attrSize = msg.readInt16();
//    for (unsigned int i = 0; i < attrSize; ++i)
//    {
//        unsigned int id = msg.readInt16();
//        double base = msg.readDouble(),
//               mod  = msg.readDouble();
//        data.setAttribute(id, base);
//        data.setModAttribute(id, mod);
//    }

//    // character skills
//    int skillSize = msg.readInt16();

//    for (int i = 0; i < skillSize; ++i)
//    {
//        int skill = msg.readInt16();
//        int level = msg.readInt32();
//        data.setExperience(skill,level);
//    }

//    // status effects currently affecting the character
//    int statusSize = msg.readInt16();

//    for (int i = 0; i < statusSize; i++)
//    {
//        int status = msg.readInt16();
//        int time = msg.readInt16();
//        data.applyStatusEffect(status, time);
//    }

//    // location
//    data.setMapId(msg.readInt16());

//    Point temporaryPoint;
//    temporaryPoint.x = msg.readInt16();
//    temporaryPoint.y = msg.readInt16();
//    data.setPosition(temporaryPoint);

//    // kill count
//    int killSize = msg.readInt16();
//    for (int i = 0; i < killSize; i++)
//    {
//        int monsterId = msg.readInt16();
//        int kills = msg.readInt32();
//        data.setKillCount(monsterId, kills);
//    }

//    // character specials
//    int specialSize = msg.readInt16();
//    data.clearSpecials();
//    for (int i = 0; i < specialSize; i++)
//    {
//        data.giveSpecial(msg.readInt32());
//    }


//    Possessions &poss = data.getPossessions();
//    EquipData equipData;
//    int equipSlotsSize = msg.readInt16();
//    unsigned int eqSlot;
//    EquipmentItem equipItem;
//    for (int j = 0; j < equipSlotsSize; ++j)
//    {
//        eqSlot  = msg.readInt16();
//        equipItem.itemId = msg.readInt16();
//        equipItem.itemInstance = msg.readInt16();
//        equipData.insert(equipData.end(),
//                               std::make_pair(eqSlot, equipItem));
//    }
//    poss.setEquipment(equipData);

//    // Loads inventory - must be last because size isn't transmitted
//    InventoryData inventoryData;
//    while (msg.getUnreadLength())
//    {
//        InventoryItem i;
//        int slotId = msg.readInt16();
//        i.itemId   = msg.readInt16();
//        i.amount   = msg.readInt16();
//        inventoryData.insert(inventoryData.end(), std::make_pair(slotId, i));
//    }
//    poss.setInventory(inventoryData);
//}
	}
}
