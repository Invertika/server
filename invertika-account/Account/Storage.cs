//
//  Storage.cs
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
using invertika_account.Chat;
using invertika_account.Common;
using invertika_account.DAL;
using System.Data;
using ISL.Server.Common;
using CSCL.Database;
using ISL.Server.Utilities;
using ISL.Server.Account;

namespace invertika_account.Account
{
    public class Storage
    {
        const string DEFAULT_ITEM_FILE="items.xml";

        // Defines the supported db version
        const string DB_VERSION_PARAMETER="database_version";

        Database mDb;         /**< the data provider */
        uint mItemDbVersion;    /**< Version of the item database. */

        /*
		 * MySQL specificities:
		 *     - TINYINT is an integer (1 byte) type defined as an extension to
		 *       the SQL standard.
		 *     - all integer types can have an optional (non-standard) attribute
		 *       UNSIGNED (http://dev.mysql.com/doc/mysql/en/numeric-types.html)
		 *
		 * SQLite3 specificities:
		 *     - any column (but only one for each table) with the exact type of
		 *       'INTEGER PRIMARY KEY' is taken as auto-increment.
		 *     - the supported data types are: NULL, INTEGER, REAL, TEXT and BLOB
		 *       (http://www.sqlite.org/datatype3.html)
		 *     - the size of TEXT cannot be set, it is just ignored by the engine.
		 *     - IMPORTANT: foreign key constraints are not yet supported
		 *       (http://www.sqlite.org/omitted.html). Included in case of future
		 *       support.
		 *
		 * Notes:
		 *     - the SQL queries will take advantage of the most appropriate data
		 *       types supported by a particular database engine in order to
		 *       optimize the server database size.
		 *
		 * TODO: Fix problem with PostgreSQL null primary key's.
		 */

        // Table names
        const string ACCOUNTS_TBL_NAME="mana_accounts";
        const string CHARACTERS_TBL_NAME="mana_characters";
        const string CHAR_ATTR_TBL_NAME="mana_char_attr";
        const string CHAR_SKILLS_TBL_NAME="mana_char_skills";
        const string CHAR_STATUS_EFFECTS_TBL_NAME="mana_char_status_effects";
        const string CHAR_KILL_COUNT_TBL_NAME="mana_char_kill_stats";
        const string CHAR_SPECIALS_TBL_NAME="mana_char_specials";
        const string CHAR_EQUIPS_TBL_NAME="mana_char_equips";
        const string INVENTORIES_TBL_NAME="mana_inventories";
        const string ITEMS_TBL_NAME="mana_items";
        const string GUILDS_TBL_NAME="mana_guilds";
        const string GUILD_MEMBERS_TBL_NAME="mana_guild_members";
        const string QUESTS_TBL_NAME="mana_quests";
        const string WORLD_STATES_TBL_NAME="mana_world_states";
        const string POST_TBL_NAME="mana_post";
        const string POST_ATTACHMENTS_TBL_NAME="mana_post_attachments";
        const string AUCTION_TBL_NAME="mana_auctions";
        const string AUCTION_BIDS_TBL_NAME="mana_auction_bids";
        const string ONLINE_USERS_TBL_NAME="mana_online_list";
        const string TRANSACTION_TBL_NAME="mana_transactions";
        const string FLOOR_ITEMS_TBL_NAME="mana_floor_items";

        public Storage()
        {
            mDb=DataProviderFactory.createDataProvider();
            mItemDbVersion=0;
        }

        ~Storage()
        {
            if(mDb.Connected)
                close();
        }

        public void open()
        {
            // Do nothing if already connected.
            if(mDb.Connected)
                return;

            // Open a connection to the database.
            mDb.Connect();

            // Check database version here
            string dbversionVal=getWorldStateVar(DB_VERSION_PARAMETER, -1);
            int dbversion=Convert.ToInt32(dbversionVal);
            int supportedDbVersion=ManaServ.SUPPORTED_DB_VERSION;

            if(dbversion!=supportedDbVersion)
            {
                string errmsg=String.Format("Database version is not supported. Needed version: '{0}', current version: '", supportedDbVersion, dbversion);
                throw new Exception(errmsg);
            }

            // Synchronize base data from xml files
            syncDatabase();

            // Clean list of online users, this should be empty after restart
            string sql=String.Format("DELETE FROM {0}", ONLINE_USERS_TBL_NAME);
            mDb.ExecuteNonQuery(sql);

            // In case where the server shouldn't keep floor item in database,
            // we remove remnants at startup
            if(Configuration.getValue("game_floorItemDecayTime", 0)>0)
            {
                sql=String.Format("DELETE FROM {0}", FLOOR_ITEMS_TBL_NAME);
                mDb.ExecuteNonQuery(sql);
            }
        }

        void close()
        {
            mDb.Disconnect();
        }
		
        /**
         * Returns the version of the local item database.
         *
         * @return the database version number.
         */
        public uint getItemDatabaseVersion()
        { 
            return mItemDbVersion; 
        }

        DateTime ToDateTime(string ticks)
        {
            return new DateTime(Convert.ToInt64(ticks));
        }

        ISL.Server.Account.Account getAccountBySQL(int accountID)
        {
            string sql=String.Format("SELECT * FROM {0} WHERE id = {1};", ACCOUNTS_TBL_NAME, accountID);
            DataTable table=mDb.ExecuteQuery(sql);

            if(table.Rows.Count==0)
                return null;

            // Create an Account instance
            // and initialize it with information about the user.
            ISL.Server.Account.Account account=new ISL.Server.Account.Account(accountID);
            account.setName(table.Rows[0]["username"].ToString());
            account.setPassword(table.Rows[0]["password"].ToString());
            account.setEmail(table.Rows[0]["email"].ToString());

            account.setRegistrationDate(ToDateTime(table.Rows[0]["registration"].ToString()));
            account.setLastLogin(ToDateTime(table.Rows[0]["lastlogin"].ToString()));

            int level=Convert.ToInt32(table.Rows[0]["level"]);

            // Check if the user is permanently banned, or temporarily banned.
            if(level==(int)AccessLevel.AL_BANNED||DateTime.Now<=ToDateTime(table.Rows[0]["banned"].ToString()))
            {
                account.setLevel((int)AccessLevel.AL_BANNED);
                // It is, so skip character loading.
                return account;
            }
            account.setLevel(level);

            // Correct on-the-fly the old 0 slot characters
            // NOTE: Will be deprecated and removed at some point.
            //fixCharactersSlot(id);

            // Load the characters associated with the account.
            sql=String.Format("SELECT id FROM {0} WHERE user_id = '{1}';", CHARACTERS_TBL_NAME, accountID);
            DataTable charInfo=mDb.ExecuteQuery(sql);

            if(charInfo.Rows.Count>0)
            {
                int size=charInfo.Rows.Count;
                Dictionary<uint, Character> characters=new Dictionary<uint, Character>();

                Logger.Write(LogLevel.Debug, "Account {0} has {1} character(s) in database.", accountID, size);

                // Two steps: it seems like multiple requests cannot be alive
                // at the same time.
                List<uint> characterIDs=new List<uint>();

                for(int k = 0;k < size;++k)
                {
                    characterIDs.Add(Convert.ToUInt32(charInfo.Rows[k]["id"]));
                }

                for(int k = 0;k < size;++k)
                {
                    Character ptr=getCharacter((int)characterIDs[k], account);

                    if(ptr!=null)
                    {
                        characters[ptr.getCharacterSlot()]=ptr;
                    }
                    else
                    {
                        Logger.Write(LogLevel.Error, "Failed to get character {0} for account {1}.", characterIDs[k], accountID);
                    }
                }

                account.setCharacters(characters);
            }

            return account;
        }

        void fixCharactersSlot(int accountId)
        {
            //try
            //{
            //    // Obtain all the characters slots from an account.
            //    std::ostringstream sql;
            //    sql << "SELECT id, slot FROM " << CHARACTERS_TBL_NAME
            //        << " where user_id = " << accountId;
            //    const dal::RecordSet &charInfo = mDb.execSql(sql.str());

            //    // If the account is not even in the database then
            //    // we can quit now.
            //    if (charInfo.isEmpty())
            //        return;

            //    // Specialize the string_to functor to convert
            //    // a string to an unsigned int.
            //    string_to< unsigned > toUint;
            //    std::map<unsigned, unsigned> slotsToUpdate;

            //    int characterNumber = charInfo.rows();
            //    unsigned currentSlot = 1;

            //    // We parse all the characters slots to see how many are to be
            //    // corrected.
            //    for (int k = 0; k < characterNumber; ++k)
            //    {
            //        // If the slot found is equal to 0.
            //        if (toUint(charInfo(k, 1)) == 0)
            //        {
            //            // Find the new slot number to assign.
            //            for (int l = 0; l < characterNumber; ++l)
            //            {
            //                if (toUint(charInfo(l, 1)) == currentSlot)
            //                    currentSlot++;
            //            }
            //            slotsToUpdate.insert(std::make_pair(toUint(charInfo(k, 0)),
            //                                                currentSlot));
            //        }
            //    }

            //    if (slotsToUpdate.size() > 0)
            //    {
            //        dal::PerformTransaction transaction(mDb);

            //        // Update the slots in database.
            //        for (std::map<unsigned, unsigned>::iterator i =
            //                                                      slotsToUpdate.begin(),
            //            i_end = slotsToUpdate.end(); i != i_end; ++i)
            //        {
            //            // Update the character slot.
            //            sql.clear();
            //            sql.str("");
            //            sql << "UPDATE " << CHARACTERS_TBL_NAME
            //                << " SET slot = " << i.second
            //                << " where id = " << i.first;
            //            mDb.execSql(sql.str());
            //        }

            //        transaction.commit();
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::fixCharactersSlots) "
            //                      "SQL query failure: ", e);
            //}
        }

        public ISL.Server.Account.Account getAccount(string userName)
        {
            string sql=String.Format("SELECT id FROM {0} WHERE username = '{1}';", ACCOUNTS_TBL_NAME, userName);
            DataTable tmp=mDb.ExecuteQuery(sql);

            int id=Convert.ToInt32(tmp.Rows[0]["id"]);
            return getAccountBySQL(id);
        }

        public ISL.Server.Account.Account getAccount(int accountID)
        {
            return getAccountBySQL(accountID);
        }

        Character getCharacterBySQL(ISL.Server.Account.Account owner)
        {
            Character character=null;

            string sql=String.Format("SELECT * FROM {0} WHERE id = {1}", CHARACTERS_TBL_NAME, owner.getID());
            DataTable charInfo=mDb.ExecuteQuery(sql);

            // If the character is not even in the database then
            // we have no choice but to return nothing.
            if(charInfo.Rows.Count==0)
                return null;

            character=new Character(charInfo.Rows[0]["name"].ToString(), Convert.ToInt32(charInfo.Rows[0]["id"]));
            character.setGender(Convert.ToInt32(charInfo.Rows[0]["gender"]));
            character.setHairStyle(Convert.ToInt32(charInfo.Rows[0]["hair_style"]));
            character.setHairColor(Convert.ToInt32(charInfo.Rows[0]["hair_color"]));
            character.setLevel(Convert.ToInt32(charInfo.Rows[0]["level"]));
            character.setCharacterPoints(Convert.ToInt32(charInfo.Rows[0]["char_pts"]));
            character.setCorrectionPoints(Convert.ToInt32(charInfo.Rows[0]["correct_pts"]));

            Point pos=new Point(Convert.ToInt32(charInfo.Rows[0]["x"]), Convert.ToInt32(charInfo.Rows[0]["y"]));
            character.setPosition(pos);

            int mapId=Convert.ToInt32(charInfo.Rows[0]["map_id"]);
            if(mapId>0)
            {
                character.setMapId(mapId);
            }
            else
            {
                // Set character to default map and one of the default location
                // Default map is to be 1, as not found return value will be 0.
                character.setMapId(Configuration.getValue("char_defaultMap", 1));
            }

            character.setCharacterSlot(Convert.ToUInt32(charInfo.Rows[0]["slot"]));

            // Fill the account-related fields. Last step, as it may require a new
            // SQL query.
            if(owner!=null)
            {
                character.setAccount(owner);
            }
            else
            {
                int id=Convert.ToInt32(charInfo.Rows[0]["user_id"]);
                character.setAccountID(id);
                    
                string s=String.Format("SELECT level FROM {0} WHERE id = '{1}';", ACCOUNTS_TBL_NAME, id);
                DataTable levelInfo=mDb.ExecuteQuery(s);

                character.setAccountLevel(Convert.ToInt32(levelInfo.Rows[0]["level"]), true);
            }

            // Load attributes."
            string s2=String.Format("SELECT attr_id, attr_base, attr_mod FROM {0} WHERE char_id = {1};", CHAR_ATTR_TBL_NAME, character.getDatabaseID());
            DataTable attrInfo=mDb.ExecuteQuery(s2);

            if(attrInfo.Rows.Count>0)
            {
                uint nRows=(uint)attrInfo.Rows.Count;

                for(uint row = 0;row < nRows;++row)
                {
                    uint id=Convert.ToUInt32(charInfo.Rows[0]["attr_id"]);
                    character.setAttribute(id, Convert.ToDouble(charInfo.Rows[0]["attr_base"]));
                    character.setModAttribute(id, Convert.ToDouble(charInfo.Rows[0]["attr_mod"]));
                }
            }

            // Load the skills of the char from CHAR_SKILLS_TBL_NAME
            string s3=String.Format("SELECT status_id, status_time FROM {0} WHERE char_id = {1};", CHAR_STATUS_EFFECTS_TBL_NAME, character.getDatabaseID());
            DataTable skillInfo=mDb.ExecuteQuery(s3);
                
            if(skillInfo.Rows.Count>0)
            {
                uint nRows=(uint)skillInfo.Rows.Count;
                for(uint row = 0;row < nRows;row++)
                {
                    character.setExperience(
                            Convert.ToInt32(skillInfo.Rows[0]["status_id"]),  // Skill Id
                            Convert.ToInt32(skillInfo.Rows[0]["status_time"])); // Experience
                }
            }

            // Load the status effect
            string s4=String.Format("SELECT status_id, status_time FROM {0} WHERE char_id = {1};", CHAR_STATUS_EFFECTS_TBL_NAME, character.getDatabaseID());
            DataTable statusInfo=mDb.ExecuteQuery(s4);

            if(statusInfo.Rows.Count>0)
            {
                uint nRows=(uint)statusInfo.Rows.Count;
                for(uint row = 0;row < nRows;row++)
                {
                    character.applyStatusEffect(
                            Convert.ToInt32(statusInfo.Rows[0]["status_id"]), // Status Id
                            Convert.ToInt32(statusInfo.Rows[0]["status_time"])); // Time
                }
            }

            // Load the kill stats
            string s5=String.Format("SELECT monster_id, kills FROM {0} WHERE char_id = {1};", CHAR_KILL_COUNT_TBL_NAME, character.getDatabaseID());
            DataTable killsInfo=mDb.ExecuteQuery(s5);

            if(killsInfo.Rows.Count>0)
            {
                uint nRows=(uint)killsInfo.Rows.Count;
                for(uint row = 0;row < nRows;row++)
                {
                    character.setKillCount(
                            Convert.ToInt32(killsInfo.Rows[0]["monster_id"]), // MonsterID
                            Convert.ToInt32(killsInfo.Rows[0]["kills"])); // Kills
                }
            }

            // Load the special status
            string s6=String.Format("SELECT special_id FROM {0} WHERE char_id = {1};", CHAR_SPECIALS_TBL_NAME, character.getDatabaseID());
            DataTable specialsInfo=mDb.ExecuteQuery(s6);

            if(specialsInfo.Rows.Count>0)
            {
                uint nRows=(uint)specialsInfo.Rows.Count;
                for(uint row = 0;row < nRows;row++)
                {
                    //TODO Überprüfen ob so sinnvoll? (der 0 Paramater?)
                    character.giveSpecial(Convert.ToInt32(specialsInfo.Rows[0]["special_id"]), 0);
                }
            }
         
            Possessions poss=character.getPossessions();

            string s7=String.Format("SELECT slot_type, item_id, item_instance FROM {0} WHERE owner_id = '{1}' ORDER BY slot_type desc;", CHAR_EQUIPS_TBL_NAME, character.getDatabaseID());
            DataTable equipInfo=mDb.ExecuteQuery(s7);

            Dictionary< uint, EquipmentItem > equipData=new Dictionary<uint, EquipmentItem>();

            if(equipInfo.Rows.Count>0)
            {
                EquipmentItem equipItem=new EquipmentItem();

                for(int k = 0, size = equipInfo.Rows.Count;k < size;++k)
                {
                    equipItem.itemId=Convert.ToUInt32(equipInfo.Rows[0]["item_id"]);
                    equipItem.itemInstance=Convert.ToUInt32(equipInfo.Rows[0]["item_instance"]);
                    equipData.Add(Convert.ToUInt32(equipInfo.Rows[0]["slot_type"]), equipItem);
                }
            }

            poss.setEquipment(equipData);
     
            string s8=String.Format("SELECT * FROM {0} WHERE owner_id = '{1}' ORDER by slot ASC", INVENTORIES_TBL_NAME, character.getDatabaseID());
            DataTable itemInfo=mDb.ExecuteQuery(s8);

            Dictionary<uint, InventoryItem > inventoryData=new Dictionary<uint, InventoryItem>();
       
            if(itemInfo.Rows.Count>0)
            {
                for(int k = 0, size = itemInfo.Rows.Count;k < size;++k)
                {
                    InventoryItem item=new InventoryItem();
                    ushort slot=Convert.ToUInt16(itemInfo.Rows[0]["slot"]);
                    item.itemId=Convert.ToUInt32(itemInfo.Rows[0]["class_id"]); 
                    item.amount=Convert.ToUInt32(itemInfo.Rows[0]["amount"]);
                    inventoryData[slot]=item;
                }
            }

            poss.setInventory(inventoryData);

            return character;
        }

        public Character getCharacter(int id, ISL.Server.Account.Account owner)
        {
            string sql=String.Format("SELECT * FROM {0},  WHERE id = {1}", CHARACTERS_TBL_NAME, id);
            //TODO Überprüfen was hier genau passiert
            return getCharacterBySQL(owner);
        }

        public Character getCharacter(string name)
        {
            string sql=String.Format("SELECT * FROM {0} WHERE name = {1}", CHARACTERS_TBL_NAME, name);
            //TODO Überprüfen was hier genau passiert
            return getCharacterBySQL(null);
        }

        public bool doesUserNameExist(string name)
        {
            string sql=String.Format("SELECT COUNT(username) AS COUNT FROM {0}  WHERE username = \"{1}\"", ACCOUNTS_TBL_NAME, name);
            DataTable table=mDb.ExecuteQuery(sql);

            if((long)(table.Rows[0]["COUNT"])>0)
                return true;
            else
                return false;
        }

        public bool doesEmailAddressExist(string email)
        {
            string sql=String.Format("SELECT COUNT(email) AS COUNT FROM {0}  WHERE UPPER(email) = UPPER(\"{1}\")", ACCOUNTS_TBL_NAME, email);
            DataTable table=mDb.ExecuteQuery(sql);

            if((long)(table.Rows[0]["COUNT"])>0)
                return true;
            else
                return false;
        }

        public bool doesCharacterNameExist(string name)
        {
            string sql=String.Format("SELECT COUNT(name) AS COUNT FROM {0}  WHERE name = \"{1}\"", CHARACTERS_TBL_NAME, name);
            DataTable table=mDb.ExecuteQuery(sql);

            if((long)(table.Rows[0]["COUNT"])>0)
                return true;
            else
                return false;
        }

        public bool updateCharacter(Character character)
        {
            //dal::PerformTransaction transaction(mDb);

            //try
            //{
            //    // Update the database Character data (see CharacterData for details)
            //    std::ostringstream sqlUpdateCharacterInfo;
            //    sqlUpdateCharacterInfo
            //        << "update "        << CHARACTERS_TBL_NAME << " "
            //        << "set "
            //        << "gender = '"     << character.getGender() << "', "
            //        << "hair_style = '" << character.getHairStyle() << "', "
            //        << "hair_color = '" << character.getHairColor() << "', "
            //        << "level = '"      << character.getLevel() << "', "
            //        << "char_pts = '"   << character.getCharacterPoints() << "', "
            //        << "correct_pts = '"<< character.getCorrectionPoints() << "', "
            //        << "x = '"          << character.getPosition().x << "', "
            //        << "y = '"          << character.getPosition().y << "', "
            //        << "map_id = '"     << character.getMapId() << "', "
            //        << "slot = '"     << character.getCharacterSlot() << "' "
            //        << "where id = '"   << character.getDatabaseID() << "';";

            //    mDb.execSql(sqlUpdateCharacterInfo.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #1) "
            //                      "SQL query failure: ", e);
            //}

            //// Character attributes.
            //try
            //{
            //    for (AttributeMap::const_iterator it = character.mAttributes.begin(),
            //         it_end = character.mAttributes.end(); it != it_end; ++it)
            //        updateAttribute(character.getDatabaseID(), it.first,
            //                        it.second.base, it.second.modified);
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #2) "
            //                      "SQL query failure: ", e);
            //}

            //// Character's skills
            //try
            //{
            //    std::map<int, int>::const_iterator skill_it;
            //    for (skill_it = character.mExperience.begin();
            //         skill_it != character.mExperience.end(); skill_it++)
            //    {
            //        updateExperience(character.getDatabaseID(),
            //                         skill_it.first, skill_it.second);
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #3) "
            //                      "SQL query failure: ", e);
            //}

            //// Character's kill count
            //try
            //{
            //    std::map<int, int>::const_iterator kill_it;
            //    for (kill_it = character.getKillCountBegin();
            //         kill_it != character.getKillCountEnd(); kill_it++)
            //    {
            //        updateKillCount(character.getDatabaseID(),
            //                        kill_it.first, kill_it.second);
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #4) "
            //                      "SQL query failure: ", e);
            //}

            ////  Character's special actions
            //try
            //{
            //    // Out with the old
            //    std::ostringstream deleteSql("");
            //    std::ostringstream insertSql;
            //    deleteSql   << "DELETE FROM " << CHAR_SPECIALS_TBL_NAME
            //                << " WHERE char_id='"
            //                << character.getDatabaseID() << "';";
            //    mDb.execSql(deleteSql.str());
            //    // In with the new
            //    std::map<int, Special*>::const_iterator special_it;
            //    for (special_it = character.getSpecialBegin();
            //         special_it != character.getSpecialEnd(); special_it++)
            //    {
            //        insertSql.str("");
            //        insertSql   << "INSERT INTO " << CHAR_SPECIALS_TBL_NAME
            //                    << " (char_id, special_id) VALUES ("
            //                    << " '" << character.getDatabaseID() << "',"
            //                    << " '" << special_it.first << "');";
            //        mDb.execSql(insertSql.str());
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #5) "
            //                      "SQL query failure: ", e);;
            //}

            //// Character's inventory
            //// Delete the old inventory and equipment table first
            //try
            //{
            //    std::ostringstream sqlDeleteCharacterEquipment;
            //    sqlDeleteCharacterEquipment
            //        << "delete from " << CHAR_EQUIPS_TBL_NAME
            //        << " where owner_id = '" << character.getDatabaseID() << "';";
            //    mDb.execSql(sqlDeleteCharacterEquipment.str());

            //    std::ostringstream sqlDeleteCharacterInventory;
            //    sqlDeleteCharacterInventory
            //        << "delete from " << INVENTORIES_TBL_NAME
            //        << " where owner_id = '" << character.getDatabaseID() << "';";
            //    mDb.execSql(sqlDeleteCharacterInventory.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #6) "
            //                      "SQL query failure: ", e);
            //}

            //// Insert the new inventory data
            //try
            //{
            //    std::ostringstream sql;

            //    sql << "insert into " << CHAR_EQUIPS_TBL_NAME
            //        << " (owner_id, slot_type, item_id, item_instance) values ("
            //        << character.getDatabaseID() << ", ";
            //    std::string base = sql.str();

            //    const Possessions &poss = character.getPossessions();
            //    const EquipData &equipData = poss.getEquipment();
            //    for (EquipData::const_iterator it = equipData.begin(),
            //         it_end = equipData.end(); it != it_end; ++it)
            //    {
            //            sql.str("");
            //            sql << base << it.first << ", " << it.second.itemId
            //                << ", " << it.second.itemInstance << ");";
            //            mDb.execSql(sql.str());
            //    }

            //    sql.str("");

            //    sql << "insert into " << INVENTORIES_TBL_NAME
            //        << " (owner_id, slot, class_id, amount) values ("
            //        << character.getDatabaseID() << ", ";
            //    base = sql.str();

            //    const InventoryData &inventoryData = poss.getInventory();
            //    for (InventoryData::const_iterator j = inventoryData.begin(),
            //         j_end = inventoryData.end(); j != j_end; ++j)
            //    {
            //        sql.str("");
            //        unsigned short slot = j.first;
            //        unsigned int itemId = j.second.itemId;
            //        unsigned int amount = j.second.amount;
            //        assert(itemId);
            //        sql << base << slot << ", " << itemId << ", " << amount << ");";
            //        mDb.execSql(sql.str());
            //    }

            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #7) "
            //                      "SQL query failure: ", e);
            //}


            //// Update char status effects
            //try
            //{
            //    // Delete the old status effects first
            //    std::ostringstream sql;

            //    sql << "delete from " << CHAR_STATUS_EFFECTS_TBL_NAME
            //        << " where char_id = '" << character.getDatabaseID() << "';";

            //     mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #8) "
            //                      "SQL query failure: ", e);
            //}
            //try
            //{
            //    std::map<int, int>::const_iterator status_it;
            //    for (status_it = character.getStatusEffectBegin();
            //         status_it != character.getStatusEffectEnd(); status_it++)
            //    {
            //        insertStatusEffect(character.getDatabaseID(),
            //                           status_it.first, status_it.second);
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::updateCharacter #9) "
            //                      "SQL query failure: ", e);
            //}

            //transaction.commit();
            return true;
        }

        //void flushSkill(Character character, int skillId)
        //{
        //    // Note: Deprecated, use DALStorage::updateExperience instead!!!
        //    // TODO: Remove calls of flushSkill for updateExperience instead.
        //    //updateExperience(character.getDatabaseID(), skillId,
        //    //    character.getExperience(skillId));
        //}

        public void addAccount(ISL.Server.Account.Account account)
        {
            string sql=String.Format("INSERT INTO {0} (username, password, email, level, banned, registration, lastlogin) ", ACCOUNTS_TBL_NAME);
            sql+=String.Format("VALUES (\"{0}\", \"{1}\", \"{2}\", {3}, 0, {4}, {5});", account.getName(), account.getPassword(), account.getEmail(), account.getLevel(), account.getRegistrationDate().Ticks, account.getLastLogin().Ticks);

            mDb.ExecuteNonQuery(sql);
        }

        public void flush(ISL.Server.Account.Account account)
        {
            //assert(account.getID() >= 0);

#if !DEBUG
            try
            {
#endif
            mDb.StartTransaction();
            //PerformTransaction transaction(mDb);

            // Update the account
            string sqlUpdateAccountTable=String.Format("UPDATE {0} SET username=\"{1}\", password=\"{2}\", email=\"{3}\", level=\"{4}\", lastlogin=\"{5}\" WHERE id = {6}",
                                                           ACCOUNTS_TBL_NAME, account.getName(), account.getPassword(), account.getEmail(), account.getLevel(), account.getLastLogin(), account.getID());
            mDb.ExecuteNonQuery(sqlUpdateAccountTable);
           
            // Get the list of characters that belong to this account.
            Dictionary<uint, Character> characters=account.getCharacters();

            // Insert or update the characters.
            foreach(KeyValuePair<uint, Character> pair in characters)
            {
                Character character=pair.Value;

                if(character.getDatabaseID()>=0)
                {
                    updateCharacter(character);
                }
                else
                {
                    // Insert the character
                    // This assumes that the characters name has been checked for
                    // uniqueness
                    string sqlInsertCharactersTable=String.Format("insert into {0} (user_id, name, gender, hair_style, hair_color, level, char_pts, correct_pts, x, y, map_id, slot) values (", CHARACTERS_TBL_NAME);
                    sqlInsertCharactersTable+=String.Format("{0}, \"{1}\", {2}, {3}, {4}, ", account.getID(), character.getName(), character.getGender(), (int)character.getHairStyle(), (int)character.getHairColor());
                    sqlInsertCharactersTable+=String.Format("{0}, {1}, {2}, ", (int)character.getLevel(), character.getCharacterPoints(), character.getCorrectionPoints());
                    sqlInsertCharactersTable+=String.Format("{0}, {1}, {2}, {3});", character.getPosition().x, character.getPosition().y, character.getMapId(), character.getCharacterSlot());
                        
                    //mDb.ExecuteNonQuery(sqlInsertCharactersTable);
                    mDb.ExecuteNonQuery(sqlInsertCharactersTable);

                    //charID ermitteln
                    string sqlGetCharId=String.Format("SELECT id FROM {0} WHERE user_id={1} AND name='{2}'", CHARACTERS_TBL_NAME, account.getID(), character.getName());
                    DataTable tmp=mDb.ExecuteQuery(sqlGetCharId);
                    int lastID=Convert.ToInt32(tmp.Rows[0]["id"]);

                    // Update the character ID.
                    character.setDatabaseID(lastID);

                    // Update all attributes.
                    foreach(KeyValuePair<uint, AttributeValue> attributePair in character.mAttributes)
                    {
                        updateAttribute(character.getDatabaseID(), attributePair.Key, attributePair.Value.@base, attributePair.Value.modified);
                    }

                    // Update the characters skill
                    foreach(KeyValuePair<int, int> experiencePair in character.mExperience)
                    {
                        updateExperience(character.getDatabaseID(), experiencePair.Key, experiencePair.Value);
                    }
                }
            }

            // Existing characters in memory have been inserted
            // or updated in database.
            // Now, let's remove those who are no more in memory from database.
            string sqlSelectNameIdCharactersTable=String.Format("select name, id from {0} where user_id = '{1}';", CHARACTERS_TBL_NAME, account.getID());
            DataTable charInMemInfo=mDb.ExecuteQuery(sqlSelectNameIdCharactersTable);

            // We compare chars from memory and those existing in db,
            // and delete those not in mem but existing in db.
            bool charFound;
            for(uint i = 0;i < charInMemInfo.Rows.Count;++i) // In database
            {
                charFound=false;

                foreach(Character characterInMemory in characters.Values) // In memory
                {
                    if(charInMemInfo.Rows[(int)i][0].ToString()==characterInMemory.getName())
                    {
                        charFound=true;
                        break;
                    }
                }

                if(!charFound)
                {
                    // The char is in db but not in memory,
                    // it will be removed from database.
                    // We store the id of the char to delete,
                    // because as deleted, the RecordSet is also emptied,
                    // and that creates an error.
                    uint charId=(uint)(charInMemInfo.Rows[(int)i][1]);
                    delCharacter((int)charId);
                }
            }

            mDb.CommitTransaction();
             
#if !DEBUG
            }
            catch(Exception e)
            {
                Logger.Write(LogLevel.Error, "SQL query failure: {0}", e);
            }
#endif
        }

        void delAccount(ISL.Server.Account.Account account)
        {
            // Sync the account info into the database.
            flush(account);

            string sql=String.Format("DELETE FROM {0} WHERE id = '{1}';", ACCOUNTS_TBL_NAME, account.getID());
            mDb.ExecuteNonQuery(sql);

            // Remove the account's characters.
            account.setCharacters(account.getCharacters()); //TODO Überprüfen ob das so funktioniert?
        }

        public void updateLastLogin(ISL.Server.Account.Account account)
        {
            string sql=String.Format("UPDATE {0} SET lastlogin = '{1}' WHERE id = '{2}';", ACCOUNTS_TBL_NAME, account.getLastLogin(), account.getID());
            mDb.ExecuteNonQuery(sql);
        }

        public void updateCharacterPoints(int charId, int charPoints, int corrPoints)
        {
            string sql=String.Format("UPDATE {0} SET char_pts = '{1}', correct_pts = '{2}' WHERE id = '{3}';", CHARACTERS_TBL_NAME, charPoints, corrPoints, charId);
            mDb.ExecuteNonQuery(sql);
        }

        public void updateExperience(int charId, int skillId, int skillValue)
        {
            //try
            //{
            //    std::ostringstream sql;
            //    // If experience has decreased to 0 we don't store it anymore,
            //    // since it's the default behaviour.
            //    if (skillValue == 0)
            //    {
            //        sql << "DELETE FROM " << CHAR_SKILLS_TBL_NAME
            //            << " WHERE char_id = " << charId
            //            << " AND skill_id = " << skillId;
            //        mDb.execSql(sql.str());
            //        return;
            //    }

            //    // Try to update the skill
            //    sql.clear();
            //    sql.str("");
            //    sql << "UPDATE " << CHAR_SKILLS_TBL_NAME
            //        << " SET skill_exp = " << skillValue
            //        << " WHERE char_id = " << charId
            //        << " AND skill_id = " << skillId;
            //    mDb.execSql(sql.str());

            //    // Check if the update has modified a row
            //    if (mDb.getModifiedRows() > 0)
            //        return;

            //    sql.clear();
            //    sql.str("");
            //    sql << "INSERT INTO " << CHAR_SKILLS_TBL_NAME << " "
            //        << "(char_id, skill_id, skill_exp) VALUES ( "
            //        << charId << ", "
            //        << skillId << ", "
            //        << skillValue << ")";
            //    mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::updateExperience) SQL query failure: ",
            //                      e);
            //}
        }

        public void updateAttribute(int charId, uint attrId, double @base, double mod)
        {
            //try
            //{
            //    std::ostringstream sql;
            //    sql << "UPDATE " << CHAR_ATTR_TBL_NAME
            //        << " SET attr_base = '" << base << "', "
            //        << "attr_mod = '" << mod << "' "
            //        << "WHERE char_id = '" << charId << "' "
            //        << "AND attr_id = '" << attrId << "';";
            //    mDb.execSql(sql.str());

            //    // If this has modified a row, we're done, it updated sucessfully.
            //    if (mDb.getModifiedRows() > 0)
            //        return;

            //    // If it did not change anything,
            //    // then the record didn't previously exist. Create it.
            //    sql.clear();
            //    sql.str("");
            //    sql << "INSERT INTO " << CHAR_ATTR_TBL_NAME
            //        << " (char_id, attr_id, attr_base, attr_mod) VALUES ( "
            //        << charId << ", " << attrId << ", " << base << ", "
            //        << mod << ")";
            //    mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::updateAttribute) SQL query failure: ",
            //                      e);
            //}
        }

        void updateKillCount(int charId, int monsterId, int kills)
        {
            //try
            //{
            //    // Try to update the kill count
            //    std::ostringstream sql;
            //    sql << "UPDATE " << CHAR_KILL_COUNT_TBL_NAME
            //        << " SET kills = " << kills
            //        << " WHERE char_id = " << charId
            //        << " AND monster_id = " << monsterId;
            //    mDb.execSql(sql.str());

            //    // Check if the update has modified a row
            //    if (mDb.getModifiedRows() > 0)
            //        return;

            //    sql.clear();
            //    sql.str("");
            //    sql << "INSERT INTO " << CHAR_KILL_COUNT_TBL_NAME << " "
            //        << "(char_id, monster_id, kills) VALUES ( "
            //        << charId << ", "
            //        << monsterId << ", "
            //        << kills << ")";
            //    mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::updateKillCount) SQL query failure: ",
            //                      e);
            //}
        }

        void insertStatusEffect(int charId, int statusId, int time)
        {
            string sql=String.Format("INSERT INTO {0} (char_id, status_id, status_time) VALUES ({1}, {2}, {3})", CHAR_STATUS_EFFECTS_TBL_NAME, charId, statusId, time);
            mDb.ExecuteNonQuery(sql);
        }

        public void addGuild(Guild guild)
        {
            string sql=String.Format("INSERT INTO {0} name VALUES \"{1}\";", GUILDS_TBL_NAME, guild.getName());
            mDb.ExecuteNonQuery(sql);

            sql=String.Format("SELECT id FROM {0} WHERE name = \"{1}\"", GUILDS_TBL_NAME, guild.getName());
            DataTable table=mDb.ExecuteQuery(sql);

            long id=(long)table.Rows[0]["id"];
            guild.setId((int)id);
        }

        public void removeGuild(Guild guild)
        {
            string sql=String.Format("DELETE FROM {0} WHERE ID = '{0}';", GUILDS_TBL_NAME, guild.getId());
            mDb.ExecuteNonQuery(sql);
        }

        public void addGuildMember(int guildId, int memberId)
        {
            string sql=String.Format("INSERT INTO {0} (guild_id, member_id, rights) VALUES ({1}, \"{2}\", \"{3}\");", GUILD_MEMBERS_TBL_NAME, guildId, memberId, 0);
            mDb.ExecuteNonQuery(sql);
        }

        public void removeGuildMember(int guildId, int memberId)
        {
            string sql=String.Format("DELETE FROM {0} WHERE member_id = \"{1}\" and guild_id = '{2}';", GUILD_MEMBERS_TBL_NAME, memberId, guildId);
            mDb.ExecuteNonQuery(sql);
        }

        public void addFloorItem(int mapId, int itemId, int amount, int posX, int posY)
        {
            string sql=String.Format("INSERT INTO {0} (map_id, item_id, amount, pos_x, pos_y)  VALUES ({0}, {1}, {2} {3}, {4});", FLOOR_ITEMS_TBL_NAME, mapId, itemId, amount, posX, posY);
            mDb.ExecuteNonQuery(sql);
        }

        public void removeFloorItem(int mapId, int itemId, int amount, int posX, int posY)
        {
            string sql=String.Format("DELETE FROM {0} WHERE map_id = {1} AND item_id = {2} AND amount = {3} AND pos_x = {4} AND pos_y = {5};",
									FLOOR_ITEMS_TBL_NAME, mapId, itemId, amount, posX, posY);
            mDb.ExecuteNonQuery(sql);
        }

        public List<FloorItem> getFloorItemsFromMap(int mapId)
        {
            List<FloorItem> floorItems=new List<FloorItem>();
			 
            string sql=String.Format("SELECT * FROM {0}  WHERE map_id = {1}", FLOOR_ITEMS_TBL_NAME, mapId);
            DataTable table=mDb.ExecuteQuery(sql);

            foreach(DataRow row in table.Rows)
            {
                int itemID=Convert.ToInt32(row["item_id"]);
                int itemAmount=Convert.ToInt32(row["amount"]);
                int posX=Convert.ToInt32(row["pos_x"]);
                int posY=Convert.ToInt32(row["pos_y"]);

                FloorItem fitem=new FloorItem(itemID, itemAmount, posX, posY);
                floorItems.Add(fitem);
            }

            return floorItems;
        }

        public void setMemberRights(int guildId, int memberId, int rights)
        {
            string sql=String.Format("UPDATE {0} SET rights = '{1}' WHERE member_id = \"{2}\"", GUILD_MEMBERS_TBL_NAME, rights, memberId);
            mDb.ExecuteNonQuery(sql);
        }

        public List<Guild> getGuildList()
        {
            //std::list<Guild*> guilds;
            //std::stringstream sql;
            //string_to<short> toShort;


            //// Get the guilds stored in the db.
            //try
            //{
            //    sql << "select id, name from " << GUILDS_TBL_NAME << ";";
            //    const dal::RecordSet& guildInfo = mDb.execSql(sql.str());

            //    // Check that at least 1 guild was returned
            //    if (guildInfo.isEmpty())
            //        return guilds;

            //    // Loop through every row in the table and assign it to a guild
            //    for ( unsigned int i = 0; i < guildInfo.rows(); ++i)
            //    {
            //        Guild* guild = new Guild(guildInfo(i,1));
            //        guild.setId(toShort(guildInfo(i,0)));
            //        guilds.push_back(guild);
            //    }
            //    string_to< unsigned > toUint;

            //    // Add the members to the guilds.
            //    for (std::list<Guild*>::iterator itr = guilds.begin();
            //         itr != guilds.end(); ++itr)
            //    {
            //        std::ostringstream memberSql;
            //        memberSql << "select member_id, rights from "
            //                  << GUILD_MEMBERS_TBL_NAME
            //                  << " where guild_id = '" << (*itr).getId() << "';";
            //        const dal::RecordSet& memberInfo = mDb.execSql(memberSql.str());

            //        std::list<std::pair<int, int> > members;
            //        for (unsigned int j = 0; j < memberInfo.rows(); ++j)
            //        {
            //            members.push_back(std::pair<int, int>(toUint(memberInfo(j, 0)),
            //                                                 toUint(memberInfo(j, 1))));
            //        }

            //        std::list<std::pair<int, int> >::const_iterator i, i_end;
            //        for (i = members.begin(), i_end = members.end(); i != i_end; ++i)
            //        {
            //            Character *character = getCharacter((*i).first, 0);
            //            if (character)
            //            {
            //                character.addGuild((*itr).getName());
            //                (*itr).addMember(character.getDatabaseID(), (*i).second);
            //            }
            //        }
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure& e)
            //{
            //    utils::throwError("(DALStorage::getGuildList) SQL query failure: ", e);
            //}

            //return guilds;

            return null; //ssk
        }

        public string getQuestVar(int id, string name)
        {
            string query=String.Format("select value from {0}  WHERE owner_id = {1} AND name = {2}", QUESTS_TBL_NAME, id, name);
            DataTable table=mDb.ExecuteQuery(query);
            return table.Rows[0]["value"].ToString(); //TODO Testen ob Ergebnis kommt
        }

        string getWorldStateVar(string name, int mapId)
        {
            string query=String.Format("SELECT value FROM {0} WHERE state_name LIKE '{1}'", WORLD_STATES_TBL_NAME, name);
            if(mapId>=0)
                query+=String.Format(" AND map_id = {0}", mapId);

            DataTable rs=mDb.ExecuteQuery(query);
            return (string)rs.Rows[0][0];
        }

        public Dictionary<string, string> getAllWorldStateVars(int mapId)
        {
            Dictionary<string, string> variables=new Dictionary<string, string>();

            // Avoid a crash because prepared statements must have at least one binding.
            if(mapId<0)
            {
                Logger.Write(LogLevel.Error, "getAllWorldStateVars was called with a negative map Id: {0}", mapId);
                return variables;
            }

            string query=String.Format("SELECT `state_name`, `value` FROM {0}", WORLD_STATES_TBL_NAME);


            // Add map filter if map_id is given
            if(mapId>=0)
            {
                query+=String.Format(" WHERE `map_id` = {0}", mapId);
            }

            //query << ";"; <-- No ';' at the end of prepared statements.
            DataTable table=mDb.ExecuteQuery(query);

            foreach(DataRow row in table.Rows)
            {
                string state_name=row["state_name"].ToString();
                string state_value=row["value"].ToString();
                variables.Add(state_name, state_value);
            }

            return variables;
        }

        public void setWorldStateVar(string name, string value)
        {
            setWorldStateVar(name, -1, value);
        }

        public void setWorldStateVar(string name, int mapId, string value)
        {
            //Set the value to empty means: delete the variable
            if(value==null||value=="")
            {
                string deleteStateVar=String.Format("DELETE FROM {0} WHERE state_name = '{1}'", WORLD_STATES_TBL_NAME, name);

                if(mapId>=0)
                {
                    deleteStateVar+=String.Format(" AND map_id = '{0}'", mapId);
                }

                deleteStateVar+=";";

                mDb.ExecuteNonQuery(deleteStateVar);
                return;
            }

            //TODO vereinfachen

            // Try to update the variable in the database
            string updateStateVar=String.Format("UPDATE {0} SET value = '{1}', moddate = '{2}'  WHERE state_name = '{3}'", WORLD_STATES_TBL_NAME, value, DateTime.Now.Ticks, name);

            if(mapId>=0)
            {
                updateStateVar+=String.Format(" AND map_id = '{0}'", mapId);
            }

            updateStateVar+=";";

            int modifiedRows=mDb.ExecuteNonQuery(updateStateVar);

            // If we updated a row, were finished here
            if(modifiedRows>0)
                return;

            // Otherwise we have to add the new variable
            string insertStateVar=String.Format("INSERT INTO {0}  (state_name, map_id, value , moddate) VALUES ('{1}', ", WORLD_STATES_TBL_NAME, name);

            if(mapId>=0)
                insertStateVar+=String.Format("'{0}', ", mapId);
            else
                insertStateVar+="0 , ";

            insertStateVar+=String.Format("'{0}', '{1}');", value, DateTime.Now.Ticks);
            mDb.ExecuteNonQuery(insertStateVar);
        }

        public void setQuestVar(int id, string name, string value)
        {
            //try
            //{
            //    std::ostringstream query1;
            //    query1 << "delete from " << QUESTS_TBL_NAME
            //           << " where owner_id = '" << id << "' and name = '"
            //           << name << "';";
            //    mDb.execSql(query1.str());

            //    if (value.empty())
            //        return;

            //    std::ostringstream query2;
            //    query2 << "insert into " << QUESTS_TBL_NAME
            //           << " (owner_id, name, value) values ('"
            //           << id << "', '" << name << "', '" << value << "');";
            //    mDb.execSql(query2.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::setQuestVar) SQL query failure: ", e);
            //}
        }

        public void banCharacter(int id, int duration)
        {
            //try
            //{
            //    // check the account of the character
            //    std::ostringstream query;
            //    query << "select user_id from " << CHARACTERS_TBL_NAME
            //          << " where id = '" << id << "';";
            //    const dal::RecordSet &info = mDb.execSql(query.str());
            //    if (info.isEmpty())
            //    {
            //        LOG_ERROR("Tried to ban an unknown user.");
            //        return;
            //    }

            //    uint64_t bantime = (uint64_t)time(0) + (uint64_t)duration * 60u;
            //    // ban the character
            //    std::ostringstream sql;
            //    sql << "update " << ACCOUNTS_TBL_NAME
            //        << " set level = '" << AL_BANNED << "', banned = '"
            //        << bantime
            //        << "' where id = '" << info(0, 0) << "';";
            //    mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::banAccount) SQL query failure: ", e);
            //}
        }

        void delCharacter(int charId)
        {
            //try
            //{
            //    dal::PerformTransaction transaction(mDb);
            //    std::ostringstream sql;

            //    // Delete the inventory of the character
            //    sql << "DELETE FROM " << INVENTORIES_TBL_NAME
            //        << " WHERE owner_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Delete the skills of the character
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << CHAR_SKILLS_TBL_NAME
            //        << " WHERE char_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Delete from the quests table
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << QUESTS_TBL_NAME
            //        << " WHERE owner_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Delete from the guilds table
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << GUILD_MEMBERS_TBL_NAME
            //        << " WHERE member_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Delete auctions of the character
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << AUCTION_TBL_NAME
            //        << " WHERE char_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Delete bids made on auctions made by the character
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << AUCTION_BIDS_TBL_NAME
            //        << " WHERE char_id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    // Now delete the character itself.
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << CHARACTERS_TBL_NAME
            //        << " WHERE id = '" << charId << "';";
            //    mDb.execSql(sql.str());

            //    transaction.commit();
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::delCharacter) SQL query failure: ", e);
            //}
        }

        void delCharacter(Character character)
        {
            //delCharacter(character.getDatabaseID());
        }

        public void checkBannedAccounts()
        {
            //try
            //{
            //    // Update expired bans
            //    std::ostringstream sql;
            //    sql << "update " << ACCOUNTS_TBL_NAME
            //    << " set level = " << AL_PLAYER << ", banned = 0"
            //    << " where level = " << AL_BANNED
            //    << " AND banned <= " << time(0) << ";";
            //    mDb.execSql(sql.str());
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::checkBannedAccounts) "
            //                      "SQL query failure: ", e);
            //}
        }

        public void setAccountLevel(int id, int level)
        {
            string sql=String.Format("update {0}  set level = {1}  where id =  {3};", ACCOUNTS_TBL_NAME, level, id);
            mDb.ExecuteQuery(sql);
        }

        public void setPlayerLevel(int id, int level)
        {
            string sql=String.Format("update {0}  set level = {1}  where id =  {3};", CHARACTERS_TBL_NAME, level, id);
            mDb.ExecuteQuery(sql);
        }

        void storeLetter(Letter letter)
        {
            //try
            //{
            //    std::ostringstream sql;
            //    if (letter.getId() == 0)
            //    {
            //        // The letter was never saved before
            //        sql << "INSERT INTO " << POST_TBL_NAME << " VALUES ( "
            //            << "NULL, "
            //            << letter.getSender().getDatabaseID() << ", "
            //            << letter.getReceiver().getDatabaseID() << ", "
            //            << letter.getExpiry() << ", "
            //            << time(0) << ", "
            //            << "?)";
            //        if (mDb.prepareSql(sql.str()))
            //        {
            //            mDb.bindValue(1, letter.getContents());
            //            mDb.processSql();

            //            letter.setId(mDb.getLastId());

            //            // TODO: Store attachments in the database

            //            return;
            //        }
            //        else
            //        {
            //            utils::throwError("(DALStorage::storeLetter) "
            //                              "SQL query preparation failure #1.");
            //        }
            //    }
            //    else
            //    {
            //        // The letter has a unique id, update the record in the db
            //        sql << "UPDATE " << POST_TBL_NAME
            //            << "   SET sender_id       = '"
            //            << letter.getSender().getDatabaseID() << "', "
            //            << "       receiver_id     = '"
            //            << letter.getReceiver().getDatabaseID() << "', "
            //            << "       letter_type     = '" << letter.getType() << "', "
            //            << "       expiration_date = '" << letter.getExpiry() << "', "
            //            << "       sending_date    = '" << time(0) << "', "
            //            << "       letter_text = ? "
            //            << " WHERE letter_id       = '" << letter.getId() << "'";

            //        if (mDb.prepareSql(sql.str()))
            //        {
            //            mDb.bindValue(1, letter.getContents());

            //            mDb.processSql();

            //            if (mDb.getModifiedRows() == 0)
            //            {
            //                // This should never happen...
            //                utils::throwError("(DALStorage::storePost) "
            //                                  "trying to update nonexistant letter.");
            //            }

            //            // TODO: Update attachments in the database
            //        }
            //        else
            //        {
            //            utils::throwError("(DALStorage::storeLetter) "
            //                              "SQL query preparation failure #2.");
            //        }
            //    }
            //}
            //catch (const std::exception &e)
            //{
            //    utils::throwError("(DALStorage::storeLetter) Exception failure: ", e);
            //}
        }

        Post getStoredPost(int playerId)
        {
            //Post *p = new Post();

            //// Specialize the string_to functor to convert
            //// a string to an unsigned int.
            //string_to< unsigned > toUint;

            //try
            //{
            //    std::ostringstream sql;
            //    sql << "SELECT * FROM " << POST_TBL_NAME
            //        << " WHERE receiver_id = " << playerId;

            //    const dal::RecordSet &post = mDb.execSql(sql.str());

            //    if (post.isEmpty())
            //    {
            //        // There is no post waiting for the character
            //        return p;
            //    }

            //    for (unsigned int i = 0; i < post.rows(); i++ )
            //    {
            //        // Load sender and receiver
            //        Character *sender = getCharacter(toUint(post(i, 1)), 0);
            //        Character *receiver = getCharacter(toUint(post(i, 2)), 0);

            //        Letter *letter = new Letter(toUint( post(0,3) ), sender, receiver);

            //        letter.setId( toUint(post(0, 0)) );
            //        letter.setExpiry( toUint(post(0, 4)) );
            //        letter.addText( post(0, 6) );

            //        // TODO: Load attachments per letter from POST_ATTACHMENTS_TBL_NAME
            //        // needs redesign of struct ItemInventroy

            //        p.addLetter(letter);
            //    }
            //}
            //catch (const std::exception &e)
            //{
            //    utils::throwError("(DALStorage::getStoredPost) Exception failure: ", e);
            //}

            //return p;

            return null; //ssk
        }

        void deletePost(Letter letter)
        {
            //try
            //{
            //    dal::PerformTransaction transaction(mDb);
            //    std::ostringstream sql;

            //    // First delete all attachments of the letter
            //    // This could leave "dead" items in the item_instances table
            //    sql << "DELETE FROM " << POST_ATTACHMENTS_TBL_NAME
            //        << " WHERE letter_id = " << letter.getId();
            //    mDb.execSql(sql.str());

            //    // Delete the letter itself
            //    sql.clear();
            //    sql.str("");
            //    sql << "DELETE FROM " << POST_TBL_NAME
            //        << " WHERE letter_id = " << letter.getId();
            //    mDb.execSql(sql.str());

            //    transaction.commit();
            //    letter.setId(0);
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::deletePost) SQL query failure: ", e);
            //}
        }

        void syncDatabase()
        {
            //XML::Document doc(DEFAULT_ITEM_FILE);
            //xmlNodePtr rootNode = doc.rootNode();

            //if (!rootNode || !xmlStrEqual(rootNode.name, BAD_CAST "items"))
            //{
            //    std::ostringstream errMsg;
            //    errMsg << "Item Manager: Error while loading item database"
            //           << "(" << DEFAULT_ITEM_FILE << ")!";
            //    LOG_ERROR(errMsg);
            //    return;
            //}

            //dal::PerformTransaction transaction(mDb);
            //int itemCount = 0;
            //for_each_xml_child_node(node, rootNode)
            //{
            //    // Try to load the version of the item database.
            //    if (xmlStrEqual(node.name, BAD_CAST "version"))
            //    {
            //        std::string revision = XML::getProperty(node, "revision",
            //                                                std::string());
            //        mItemDbVersion = atoi(revision.c_str());
            //        LOG_INFO("Loading item database version " << mItemDbVersion);
            //    }

            //    if (!xmlStrEqual(node.name, BAD_CAST "item"))
            //        continue;

            //    if (xmlStrEqual(node.name, BAD_CAST "item"))
            //    {
            //        int id = XML::getProperty(node, "id", 0);

            //        if (id < 1)
            //            continue;

            //        int weight = XML::getProperty(node, "weight", 0);
            //        std::string type = XML::getProperty(node, "type", std::string());
            //        std::string name = XML::getProperty(node, "name", std::string());
            //        std::string desc = XML::getProperty(node, "description",
            //                                            std::string());
            //        std::string eff  = XML::getProperty(node, "effect", std::string());
            //        std::string image = XML::getProperty(node, "image", std::string());
            //        std::string dye;

            //        // Split image name and dye string
            //        size_t pipe = image.find("|");
            //        if (pipe != std::string::npos)
            //        {
            //            dye = image.substr(pipe + 1);
            //            image = image.substr(0, pipe);
            //        }

            //        try
            //        {
            //            std::ostringstream sql;
            //            sql << "UPDATE " << ITEMS_TBL_NAME
            //                << " SET name = ?, "
            //                << "     description = ?, "
            //                << "     image = '" << image << "', "
            //                << "     weight = " << weight << ", "
            //                << "     itemtype = '" << type << "', "
            //                << "     effect = ?, "
            //                << "     dyestring = '" << dye << "' "
            //                << " WHERE id = " << id;

            //            if (mDb.prepareSql(sql.str()))
            //            {
            //                mDb.bindValue(1, name);
            //                mDb.bindValue(2, desc);
            //                mDb.bindValue(3, eff);

            //                mDb.processSql();
            //                if (mDb.getModifiedRows() == 0)
            //                {
            //                    sql.clear();
            //                    sql.str("");
            //                    sql << "INSERT INTO " << ITEMS_TBL_NAME
            //                        << "  VALUES ( " << id << ", ?, ?, '"
            //                        << image << "', " << weight << ", '"
            //                        << type << "', ?, '" << dye << "' )";
            //                    if (mDb.prepareSql(sql.str()))
            //                    {
            //                        mDb.bindValue(1, name);
            //                        mDb.bindValue(2, desc);
            //                        mDb.bindValue(3, eff);
            //                        mDb.processSql();
            //                    }
            //                    else
            //                    {
            //                        utils::throwError("(DALStorage::SyncDatabase) "
            //                                       "SQL query preparation failure #2.");
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                utils::throwError("(DALStorage::SyncDatabase) "
            //                                  "SQL query preparation failure #1.");
            //            }
            //            itemCount++;
            //        }
            //        catch (const dal::DbSqlQueryExecFailure &e)
            //        {
            //            utils::throwError("(DALStorage::SyncDatabase) "
            //                              "SQL query failure: ", e);
            //        }
            //    }
            //}

            //transaction.commit();
        }

        public void setOnlineStatus(int charId, bool online)
        {
            //try
            //{
            //    std::ostringstream sql;
            //    if (online)
            //    {
            //        // First we try to update the online status. this prevents errors
            //        // in case we get the online status twice
            //        sql << "SELECT COUNT(*) FROM " << ONLINE_USERS_TBL_NAME
            //            << " WHERE char_id = " << charId;
            //        const std::string res = mDb.execSql(sql.str())(0, 0);

            //        if (res != "0")
            //            return;

            //        sql.clear();
            //        sql.str("");
            //        sql << "INSERT INTO " << ONLINE_USERS_TBL_NAME
            //            << " VALUES (" << charId << ", " << time(0) << ")";
            //        mDb.execSql(sql.str());
            //    }
            //    else
            //    {
            //        sql << "DELETE FROM " << ONLINE_USERS_TBL_NAME
            //            << " WHERE char_id = " << charId;
            //        mDb.execSql(sql.str());
            //    }


            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::setOnlineStatus) SQL query failure: ",
            //                      e);
            //}
        }

        public void addTransaction(Transaction trans)
        {
            string sql=String.Format("INSERT INTO {0} VALUES (NULL, {1}, '{2}', '{3}', {4});", TRANSACTION_TBL_NAME, trans.mCharacterId, trans.mAction, trans.mMessage, DateTime.Now.Ticks);
            mDb.ExecuteNonQuery(sql);
        }

        List<Transaction> getTransactions(uint num)
        {
            //std::vector<Transaction> transactions;
            //string_to<unsigned int> toUint;

            //try
            //{
            //    std::stringstream sql;
            //    sql << "SELECT * FROM " << TRANSACTION_TBL_NAME;
            //    const dal::RecordSet &rec = mDb.execSql(sql.str());

            //    int size = rec.rows();
            //    int start = size - num;
            //    // Get the last <num> records and store them in transactions
            //    for (int i = start; i < size; ++i)
            //    {
            //        Transaction trans;
            //        trans.mCharacterId = toUint(rec(i, 1));
            //        trans.mAction = toUint(rec(i, 2));
            //        trans.mMessage = rec(i, 3);
            //        transactions.push_back(trans);
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::getTransactions) SQL query failure: ",
            //                      e);
            //}

            //return transactions;

            return null; //ssk;
        }

        List<Transaction> getTransactions(DateTime date)
        {
            //std::vector<Transaction> transactions;
            //string_to<unsigned int> toUint;

            //try
            //{
            //    std::stringstream sql;
            //    sql << "SELECT * FROM " << TRANSACTION_TBL_NAME << " WHERE time > "
            //        << date;
            //    const dal::RecordSet &rec = mDb.execSql(sql.str());

            //    for (unsigned int i = 0; i < rec.rows(); ++i)
            //    {
            //        Transaction trans;
            //        trans.mCharacterId = toUint(rec(i, 1));
            //        trans.mAction = toUint(rec(i, 2));
            //        trans.mMessage = rec(i, 3);
            //        transactions.push_back(trans);
            //    }
            //}
            //catch (const dal::DbSqlQueryExecFailure &e)
            //{
            //    utils::throwError("(DALStorage::getTransactions) SQL query failure: ",
            //                      e);
            //}

            //return transactions;

            return null; //ssk;
        }
    }
}
