//
//  AccountHandler.cs
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
using invertika_account.Common;
using System.Net.Sockets;
using invertika_account.Utilities;
using ISL.Server.Utilities;
using ISL.Server.Network;
using ISL.Server.Common;
using ISL.Server.Account;
using ISL.Server.Enums;
using System.Net;
using ISL.Server.Account;
using CSCL;
using CSCL.Crypto;
using invertika_account.Chat;

namespace invertika_account.Account
{
    public class AccountHandler : ConnectionHandler
    {
        int mStartingPoints;   /**< Character's starting points. */
        int mAttributeMinimum; /**< Minimum value for customized attributes. */
        int mAttributeMaximum; /**< Maximum value for customized attributes. */

        int mNumHairStyles;
        int mNumHairColors;
        int mNumGenders;
        uint mMinNameLength;
        uint mMaxNameLength;
        int mMaxCharacters;
        bool mRegistrationAllowed;
        string mUpdateHost;
        string mDataUrl;
        Dictionary<IPAddress, DateTime> mLastLoginAttemptForIP=new Dictionary<IPAddress, DateTime>();

        /** List of all accounts which requested a random seed, but are not logged
     *  yet. This list will be regularly remove (after timeout) old accounts
     */
        List<ISL.Server.Account.Account> mPendingAccounts=new List<ISL.Server.Account.Account>();
           
        //Token collector for connecting a client coming from a game server
        //without having to provide username and password a second time.
        public TokenCollector<AccountHandler, AccountClient, int> mTokenCollector;

        /** List of attributes that the client can send at account creation. */
        List<int> mModifiableAttributes=new List<int>();

        Dictionary<uint, Attribute> mDefaultAttributes=new  Dictionary<uint, Attribute>();

        public AccountHandler(string attributesFile)
        {
            mTokenCollector=new TokenCollector<AccountHandler, AccountClient, int>();
            mNumHairStyles=Configuration.getValue("char_numHairStyles", 17);
            mNumHairColors=Configuration.getValue("char_numHairColors", 11);
            mNumGenders=Configuration.getValue("char_numGenders", 2);
            mMinNameLength=(uint)Configuration.getValue("char_minNameLength", 4);
            mMaxNameLength=(uint)Configuration.getValue("char_maxNameLength", 25);
            mMaxCharacters=Configuration.getValue("account_maxCharacters", 3);
            mRegistrationAllowed=Configuration.getBoolValue("account_allowRegister", true);
            mUpdateHost=Configuration.getValue("net_defaultUpdateHost", "");
            mDataUrl=Configuration.getValue("net_clientDataUrl", "");

            //XML::Document doc(attributesFile);
            //xmlNodePtr node = doc.rootNode();

            //if (!node || !xmlStrEqual(node.name, BAD_CAST "attributes"))
            //{
            //    LOG_FATAL("Account handler: " << attributesFile << ": "
            //              << " is not a valid database file!");
            //    exit(EXIT_XML_BAD_PARAMETER);
            //}

            //for_each_xml_child_node(attributenode, node)
            //{
            //    if (xmlStrEqual(attributenode.name, BAD_CAST "attribute"))
            //    {
            //        int id = XML::getProperty(attributenode, "id", 0);
            //        if (!id)
            //        {
            //            LOG_WARN("Account handler: " << attributesFile << ": "
            //                     << "An invalid attribute id value (0) has been found "
            //                     << "and will be ignored.");
            //            continue;
            //        }

            //        if (XML::getBoolProperty(attributenode, "modifiable", false))
            //            mModifiableAttributes.push_back(id);

            //        // Store as string initially to check
            //        // that the property is defined.
            //        std::string defStr = XML::getProperty(attributenode, "default",
            //                                              std::string());
            //        if (!defStr.empty())
            //        {
            //            const double val = string_to<double>()(defStr);
            //            mDefaultAttributes.insert(std::make_pair(id, val));
            //        }
            //    }
            //    else if (xmlStrEqual(attributenode.name, BAD_CAST "points"))
            //    {
            //        mStartingPoints = XML::getProperty(attributenode, "start", 0);
            //        mAttributeMinimum = XML::getProperty(attributenode, "minimum", 0);
            //        mAttributeMaximum = XML::getProperty(attributenode, "maximum", 0);

            //        // Stops if not all the values are given.
            //        if (!mStartingPoints || !mAttributeMinimum || !mAttributeMaximum)
            //        {
            //            LOG_FATAL("Account handler: " << attributesFile << ": "
            //                      << " The characters starting points "
            //                      << "are incomplete or not set!");
            //            exit(EXIT_XML_BAD_PARAMETER);
            //        }
            //    }
            //} // End for each XML nodes

            //if (mModifiableAttributes.empty())
            //{
            //    LOG_FATAL("Account handler: " << attributesFile << ": "
            //              << "No modifiable attributes found!");
            //    exit(EXIT_XML_BAD_PARAMETER);
            //}

            //int attributeCount = (int) mModifiableAttributes.size();
            //if (attributeCount * mAttributeMaximum < mStartingPoints ||
            //    attributeCount * mAttributeMinimum > mStartingPoints)
            //{
            //    LOG_FATAL("Account handler: " << attributesFile << ": "
            //              << "Character's point values make "
            //              << "the character's creation impossible!");
            //    exit(EXIT_XML_BAD_PARAMETER);
            //}

            //LOG_DEBUG("Character start points: " << mStartingPoints << " (Min: "
            //          << mAttributeMinimum << ", Max: " << mAttributeMaximum << ")");
        }

        protected override NetComputer computerConnected(TcpClient peer)
        {
            return new AccountClient(peer);
        }

        protected override void computerDisconnected(NetComputer comp)
        {
            AccountClient client=(AccountClient)(comp);

            if(client.status==AccountClientStatus.CLIENT_QUEUED)
            {
                // Delete it from the pendingClient list
                mTokenCollector.deletePendingClient(client);
            }

            //delete client; // ~AccountClient unsets the account
        }

        void sendCharacterData(AccountClient client, Character ch)
        {
            MessageOut charInfo=new MessageOut(Protocol.APMSG_CHAR_INFO);

            charInfo.writeInt8((int)ch.getCharacterSlot());
            charInfo.writeString(ch.getName());
            charInfo.writeInt8(ch.getGender());
            charInfo.writeInt8(ch.getHairStyle());
            charInfo.writeInt8(ch.getHairColor());
            charInfo.writeInt16(ch.getLevel());
            charInfo.writeInt16(ch.getCharacterPoints());
            charInfo.writeInt16(ch.getCorrectionPoints());

            foreach(KeyValuePair<uint, AttributeValue> at in ch.mAttributes)
            {
                charInfo.writeInt32((int)at.Key);
                charInfo.writeInt32((int)(at.Value.@base*256));
                charInfo.writeInt32((int)(at.Value.modified*256));
            }

            client.send(charInfo);
        }

        Random rnd=new Random();

        string getRandomString(int length)
        {
            StringBuilder s=new StringBuilder();
            // No need to care about zeros. They can be handled.
            // But care for endianness
            for(int i = 0;i < length;++i)
            {
                s.Append((char)(rnd.Next(52)+65));
            }

            return s.ToString();
        }

        void handleLoginRandTriggerMessage(AccountClient client, MessageIn msg)
        {
            string salt=getRandomString(4);
            string username=msg.readString();

            ISL.Server.Account.Account acc=Program.storage.getAccount(username);

            if(acc!=null)
            {
                acc.setRandomSalt(salt);
                mPendingAccounts.Add(acc);
            }

            MessageOut reply=new MessageOut(Protocol.APMSG_LOGIN_RNDTRGR_RESPONSE);
            reply.writeString(salt);
            client.send(reply);
        }

        void handleLoginMessage(AccountClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.APMSG_LOGIN_RESPONSE);

            //Überprüfung ob es sich um einen Login Request handelt
            if(client.status!=AccountClientStatus.CLIENT_LOGIN)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                client.send(reply);
                return;
            }

            int clientVersion=msg.readInt32();

            if(clientVersion<ManaServ.PROTOCOL_VERSION)
            {
                reply.writeInt8((int)Login.LOGIN_INVALID_VERSION);
                client.send(reply);
                return;
            }

            // Check whether the last login attempt for this IP is still too fresh
            IPAddress address=client.getIP();
            DateTime now=DateTime.Now;

            if(mLastLoginAttemptForIP.ContainsKey(address)) //TODO Schauen ob der Vergleich gegen das IPAdress Objekt funktioniert
            {
                DateTime lastAttempt=mLastLoginAttemptForIP[address];
                lastAttempt.AddSeconds(1); //TODO schauen ob hier im Original wirklich Sekunden gemeint sind

                if(now<lastAttempt)
                {
                    reply.writeInt8((int)Login.LOGIN_INVALID_TIME);
                    client.send(reply);
                    return;
                }
            }

            mLastLoginAttemptForIP[address]=now;

            string username=msg.readString();
            string password=msg.readString();

            if(Program.stringFilter.findDoubleQuotes(username))
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
                client.send(reply);
                return;
            }

            uint maxClients=(uint)Configuration.getValue("net_maxClients", 1000);

            if(getClientCount()>=maxClients)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_SERVER_FULL);
                client.send(reply);
                return;
            }

            // Check if the account exists
            ISL.Server.Account.Account acc=null;

            foreach(ISL.Server.Account.Account tmp in mPendingAccounts)
            {
                if(tmp.getName()==username)
                {
                    acc=tmp;
                    break;
                }
            }

            mPendingAccounts.Remove(acc); 

            //TODO Überprüfen ob SHA256 das gewünschte Ergebniss liefert
            if(acc!=null)
            {
                if(SHA256.HashString(acc.getPassword()+acc.getRandomSalt())!=password)
                {
                    reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
                    client.send(reply);
                    //delete acc;
                    return;
                }
            }

            if(acc.getLevel()==(int)AccessLevel.AL_BANNED)
            {
                reply.writeInt8((int)Login.LOGIN_BANNED);
                client.send(reply);
                //delete acc;
                return;
            }

            // The client successfully logged in...

            // Set lastLogin date of the account.
            DateTime login=DateTime.Now;
            acc.setLastLogin(login);
            Program.storage.updateLastLogin(acc);  

            // Associate account with connection.
            client.setAccount(acc);
            client.status=AccountClientStatus.CLIENT_CONNECTED;

            reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
            addServerInfo(reply);
            client.send(reply); // Acknowledge login

            // Return information about available characters
            Dictionary<uint, Character> chars=acc.getCharacters();

            // Send characters list
            foreach(ISL.Server.Account.Character character in chars.Values)
            {
                sendCharacterData(client, character);
            }
        }

        void handleLogoutMessage(AccountClient client)
        {
            MessageOut reply=new MessageOut(Protocol.APMSG_LOGOUT_RESPONSE);

            //if (client.status == CLIENT_LOGIN)
            //{
            //    reply.writeInt8(ERRMSG_NO_LOGIN);
            //}
            //else if (client.status == CLIENT_CONNECTED)
            //{
            //    client.unsetAccount();
            //    client.status = CLIENT_LOGIN;
            //    reply.writeInt8(ERRMSG_OK);
            //}
            //else if (client.status == CLIENT_QUEUED)
            //{
            //    // Delete it from the pendingClient list
            //    mTokenCollector.deletePendingClient(&client);
            //    client.status = CLIENT_LOGIN;
            //    reply.writeInt8(ERRMSG_OK);
            //}
            //client.send(reply);
        }

        void handleReconnectMessage(AccountClient client, MessageIn msg)
        {
            //if (client.status != CLIENT_LOGIN)
            //{
            //    LOG_DEBUG("Account tried to reconnect, but was already logged in "
            //              "or queued.");
            //    return;
            //}

            //std::string magic_token = msg.readString(MAGIC_TOKEN_LENGTH);
            //client.status = CLIENT_QUEUED; // Before the addPendingClient
            //mTokenCollector.addPendingClient(magic_token, &client);
        }

        void handleRegisterMessage(AccountClient client, MessageIn msg)
        {
            int clientVersion=msg.readInt32();
            string username=msg.readString();
            string password=msg.readString();
            string email=msg.readString();
            string captcha=msg.readString();

            MessageOut reply=new MessageOut(Protocol.APMSG_REGISTER_RESPONSE);

            if(client.status!=AccountClientStatus.CLIENT_LOGIN)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }
            else if(!mRegistrationAllowed)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
            }
            else if(clientVersion<ManaServ.PROTOCOL_VERSION)
            {
                reply.writeInt8((int)Register.REGISTER_INVALID_VERSION);
            }
            else if(Program.stringFilter.findDoubleQuotes(username)
                ||Program.stringFilter.findDoubleQuotes(email)
                ||username.Length<mMinNameLength
                ||username.Length>mMaxNameLength
                ||!Program.stringFilter.isEmailValid(email)
                ||!Program.stringFilter.filterContent(username))
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else if(Program.storage.doesUserNameExist(username))
            {
                reply.writeInt8((int)Register.REGISTER_EXISTS_USERNAME);
            }
            else if(Program.storage.doesEmailAddressExist(SHA256.HashString(email)))
            {
                reply.writeInt8((int)Register.REGISTER_EXISTS_EMAIL);
            }
            else if(!checkCaptcha(client, captcha))
            {
                reply.writeInt8((int)Register.REGISTER_CAPTCHA_WRONG);
            }
            else
            {
                ISL.Server.Account.Account acc=new ISL.Server.Account.Account();
                acc.setName(username);
                acc.setPassword(SHA256.HashString(password));

                // We hash email server-side for additional privacy
                // we ask for it again when we need it and verify it
                // through comparing it with the hash.
                acc.setEmail(SHA256.HashString(email));
                acc.setLevel((int)AccessLevel.AL_PLAYER);

                // Set the date and time of the account registration, and the last login
                DateTime regdate=DateTime.Now;
                acc.setRegistrationDate(regdate);
                acc.setLastLogin(regdate);

                Program.storage.addAccount(acc);
                reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
                addServerInfo(reply);

                // Associate account with connection
                client.setAccount(acc);
                client.status=AccountClientStatus.CLIENT_CONNECTED;
            }

            client.send(reply);
        }

        //TODO Captcha Unterstüzung evt ganz raus?
        static bool checkCaptcha(AccountClient client, string captcha)
        {
            // TODO
            return true;
        }

        void handleUnregisterMessage(AccountClient client, MessageIn msg)
        {
            //LOG_DEBUG("AccountHandler::handleUnregisterMessage");

            //MessageOut reply(APMSG_UNREGISTER_RESPONSE);

            //if (client.status != CLIENT_CONNECTED)
            //{
            //    reply.writeInt8(ERRMSG_FAILURE);
            //    client.send(reply);
            //    return;
            //}

            //std::string username = msg.readString();
            //std::string password = msg.readString();

            //if (stringFilter.findDoubleQuotes(username))
            //{
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //    client.send(reply);
            //    return;
            //}

            //// See whether the account exists
            //Account *acc = storage.getAccount(username);

            //if (!acc || acc.getPassword() != sha256(password))
            //{
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //    client.send(reply);
            //    delete acc;
            //    return;
            //}

            //// Delete account and associated characters
            //LOG_INFO("Unregistered \"" << username
            //         << "\", AccountID: " << acc.getID());
            //storage.delAccount(acc);
            //reply.writeInt8(ERRMSG_OK);

            //client.send(reply);
        }

        void handleRequestRegisterInfoMessage(AccountClient client, MessageIn msg)
        {
            //LOG_INFO("AccountHandler::handleRequestRegisterInfoMessage");
            //MessageOut reply(APMSG_REGISTER_INFO_RESPONSE);
            //if (!Configuration::getBoolValue("account_allowRegister", true))
            //{
            //    reply.writeInt8(false);
            //    reply.writeString(Configuration::getValue(
            //                          "account_denyRegisterReason", std::string()));
            //}
            //else
            //{
            //    reply.writeInt8(true);
            //    reply.writeInt8(mMinNameLength);
            //    reply.writeInt8(mMaxNameLength);
            //    reply.writeString("http://www.server.example/captcha.png");
            //    reply.writeString("<instructions for solving captcha>");
            //}
            //client.send(reply);
        }

        void handleEmailChangeMessage(AccountClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.APMSG_EMAIL_CHANGE_RESPONSE);

            //Account acc = client.getAccount();
            //if (!acc)
            //{
            //    reply.writeInt8(ERRMSG_NO_LOGIN);
            //    client.send(reply);
            //    return;
            //}

            //const std::string email = msg.readString();
            //const std::string emailHash = sha256(email);

            //if (!stringFilter.isEmailValid(email))
            //{
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //}
            //else if (stringFilter.findDoubleQuotes(email))
            //{
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //}
            //else if (storage.doesEmailAddressExist(emailHash))
            //{
            //    reply.writeInt8(ERRMSG_EMAIL_ALREADY_EXISTS);
            //}
            //else
            //{
            //    acc.setEmail(emailHash);
            //    // Keep the database up to date otherwise we will go out of sync
            //    storage.flush(acc);
            //    reply.writeInt8(ERRMSG_OK);
            //}
            //client.send(reply);
        }

        void handlePasswordChangeMessage(AccountClient client, MessageIn msg)
        {
            //std::string oldPassword = sha256(msg.readString());
            //std::string newPassword = sha256(msg.readString());

            //MessageOut reply(APMSG_PASSWORD_CHANGE_RESPONSE);

            //Account *acc = client.getAccount();
            //if (!acc)
            //{
            //    reply.writeInt8(ERRMSG_NO_LOGIN);
            //}
            //else if (stringFilter.findDoubleQuotes(newPassword))
            //{
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //}
            //else if (oldPassword != acc.getPassword())
            //{
            //    reply.writeInt8(ERRMSG_FAILURE);
            //}
            //else
            //{
            //    acc.setPassword(newPassword);
            //    // Keep the database up to date otherwise we will go out of sync
            //    storage.flush(acc);
            //    reply.writeInt8(ERRMSG_OK);
            //}

            //client.send(reply);
        }

        void handleCharacterCreateMessage(AccountClient client, MessageIn msg)
        {
            string name=msg.readString();
            int hairStyle=msg.readInt8();
            int hairColor=msg.readInt8();
            int gender=msg.readInt8();

            // Avoid creation of character from old clients.
//            int slot=-1;
//            if(msg.getUnreadLength()>7)
//            {
            int slot=msg.readInt8();
//            }

            MessageOut reply=new MessageOut(Protocol.APMSG_CHAR_CREATE_RESPONSE);

            ISL.Server.Account.Account acc=client.getAccount();

            if(acc==null)
            {
                reply.writeInt8((byte)ErrorMessage.ERRMSG_NO_LOGIN);
            }
            else if(!Program.stringFilter.filterContent(name))
            {
                reply.writeInt8((byte)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else if(Program.stringFilter.findDoubleQuotes(name))
            {
                reply.writeInt8((byte)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else if(hairStyle>mNumHairStyles)
            {
                reply.writeInt8((byte)Create.CREATE_INVALID_HAIRSTYLE);
            }
            else if(hairColor>mNumHairColors)
            {
                reply.writeInt8((byte)Create.CREATE_INVALID_HAIRCOLOR);
            }
            else if(gender>mNumGenders)
            {
                reply.writeInt8((byte)Create.CREATE_INVALID_GENDER);
            }
            else if((name.Length<mMinNameLength)||
                (name.Length>mMaxNameLength))
            {
                reply.writeInt8((byte)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
            }
            else
            {
                if(Program.storage.doesCharacterNameExist(name))
                {
                    reply.writeInt8((byte)Create.CREATE_EXISTS_NAME);
                    client.send(reply);
                    return;
                }

                // An account shouldn't have more
                // than <account_maxCharacters> characters.
                Dictionary<uint, ISL.Server.Account.Character> chars=acc.getCharacters();

                if(slot<1||slot>mMaxCharacters||!acc.isSlotEmpty((uint)slot))
                {
                    reply.writeInt8((byte)Create.CREATE_INVALID_SLOT);
                    client.send(reply);
                    return;
                }

                if((int)chars.Count>=mMaxCharacters)
                {
                    reply.writeInt8((byte)Create.CREATE_TOO_MUCH_CHARACTERS);
                    client.send(reply);
                    return;
                }

                // TODO: Add race, face and maybe special attributes.

                // Customization of character's attributes...
                List<int> attributes=new List<int>();
                //std::vector<int>(mModifiableAttributes.size(), 0);
                for(uint i = 0;i < mModifiableAttributes.Count;++i)
                {
                    attributes.Add(msg.readInt16());
                }

                int totalAttributes=0;
                for(uint i = 0;i < mModifiableAttributes.Count;++i)
                {
                    // For good total attributes check.
                    totalAttributes+=attributes[(int)i];

                    // For checking if all stats are >= min and <= max.
                    if(attributes[(int)i]<mAttributeMinimum
                        ||attributes[(int)i]>mAttributeMaximum)
                    {
                        reply.writeInt8((byte)Create.CREATE_ATTRIBUTES_OUT_OF_RANGE);
                        client.send(reply);
                        return;
                    }
                }

                if(totalAttributes>mStartingPoints)
                {
                    reply.writeInt8((byte)Create.CREATE_ATTRIBUTES_TOO_HIGH);
                }
                else if(totalAttributes<mStartingPoints)
                {
                    reply.writeInt8((byte)Create.CREATE_ATTRIBUTES_TOO_LOW);
                }
                else
                {
                    Character newCharacter=new Character(name);

                    // Set the initial attributes provided by the client
                    for(uint i = 0;i < mModifiableAttributes.Count;++i)
                    {
                        //TODO schauen was hier genau passieren muss
                        //newCharacter.mAttributes.Add((uint)(mModifiableAttributes[(int)i]), mModifiableAttributes[i]);
                        //newCharacter.mAttributes.Add((uint)mModifiableAttributes[(int)i], attributes[(int)i]);
                    }


                    foreach(KeyValuePair<uint, Attribute> defaultAttributePair in mDefaultAttributes)
                    {
                        //TODO schauen was hier genau passieren muss
                        // newCharacter.mAttributes.Add(defaultAttributePair.Key, defaultAttributePair.Value);
                    }

                    newCharacter.setAccount(acc);
                    newCharacter.setCharacterSlot((uint)slot);
                    newCharacter.setGender(gender);
                    newCharacter.setHairStyle(hairStyle);
                    newCharacter.setHairColor(hairColor);
                    newCharacter.setMapId(Configuration.getValue("char_startMap", 1));
                    Point startingPos=new Point(Configuration.getValue("char_startX", 1024),
                                      Configuration.getValue("char_startY", 1024));
                    newCharacter.setPosition(startingPos);
                    acc.addCharacter(newCharacter);

                    Logger.Write(LogLevel.Information, "Character {0} was created for {1}'s account.", name, acc.getName());

                    Program.storage.flush(acc); // flush changes

                    // log transaction
                    Transaction trans=new Transaction();
                    trans.mCharacterId=(uint)newCharacter.getDatabaseID();
                    trans.mAction=(uint)TransactionMembers.TRANS_CHAR_CREATE;
                    trans.mMessage=acc.getName()+" created character ";
                    trans.mMessage+="called "+name;
                    Program.storage.addTransaction(trans);

                    reply.writeInt8((byte)ErrorMessage.ERRMSG_OK);
                    client.send(reply);

                    // Send new characters infos back to client
                    sendCharacterData(client, chars[(uint)slot]);
                    return;
                }
            }

            client.send(reply);
        }

        void handleCharacterSelectMessage(AccountClient client, MessageIn msg)
        {
            MessageOut reply=new MessageOut(Protocol.APMSG_CHAR_SELECT_RESPONSE);

            ISL.Server.Account.Account acc=client.getAccount();

            if(acc==null)
            {
                reply.writeInt8((int)ErrorMessage.ERRMSG_NO_LOGIN);
                client.send(reply);
                return; // not logged in
            }

            int slot=msg.readInt8();
            Dictionary<uint, Character> chars=acc.getCharacters();

            if(chars.ContainsKey((uint)slot)==false)
            {
                // Invalid char selection
                reply.writeInt8((int)ErrorMessage.ERRMSG_INVALID_ARGUMENT);
                client.send(reply);
                return;
            }

            Character selectedChar=chars[(uint)slot];

            string address;
            int port;

            if(!GameServerHandler.getGameServerFromMap(selectedChar.getMapId(), out address, out port))
            {
                Logger.Write(LogLevel.Error, "Character Selection: No game server for map #{0}", selectedChar.getMapId());
                reply.writeInt8((int)ErrorMessage.ERRMSG_FAILURE);
                client.send(reply);
                return;
            }

            reply.writeInt8((int)ErrorMessage.ERRMSG_OK);

            Logger.Write(LogLevel.Debug, "{0} is trying to enter the servers.", selectedChar.getName());

            string magic_token=Various.GetUniqueID();
            reply.writeString(magic_token);
            reply.writeString(address);
            reply.writeInt16(port);

            // Give address and port for the chat server
            reply.writeString(Configuration.getValue("net_chatHost", "localhost"));

            // When the chatListenToClientPort is set, we use it.
            // Otherwise, we use the accountListenToClientPort + 2 if the option is set.
            // If neither, the DEFAULT_SERVER_PORT + 2 is used.
            int alternativePort=Configuration.getValue("net_accountListenToClientPort", Configuration.DEFAULT_SERVER_PORT)+2;
            reply.writeInt16(Configuration.getValue("net_chatListenToClientPort", alternativePort));

            GameServerHandler.registerClient(magic_token, selectedChar);
            ChatHandler.registerChatClient(magic_token, selectedChar.getName(), acc.getLevel());

            client.send(reply);

            // log transaction
            Transaction trans=new Transaction();
            trans.mCharacterId=(uint)selectedChar.getDatabaseID();
            trans.mAction=(uint)TransactionMembers.TRANS_CHAR_SELECTED;

            Program.storage.addTransaction(trans);
        }

        void handleCharacterDeleteMessage(AccountClient client, MessageIn msg)
        {
            //MessageOut reply(APMSG_CHAR_DELETE_RESPONSE);

            //Account *acc = client.getAccount();
            //if (!acc)
            //{
            //    reply.writeInt8(ERRMSG_NO_LOGIN);
            //    client.send(reply);
            //    return; // not logged in
            //}

            //int slot = msg.readInt8();
            //Characters &chars = acc.getCharacters();

            //if (slot < 1 || acc.isSlotEmpty(slot))
            //{
            //    // Invalid char selection
            //    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
            //    client.send(reply);
            //    return;
            //}

            //std::string characterName = chars[slot].getName();
            //LOG_INFO("Character deleted:" << characterName);

            //// Log transaction
            //Transaction trans;
            //trans.mCharacterId = chars[slot].getDatabaseID();
            //trans.mAction = TRANS_CHAR_DELETED;
            //trans.mMessage = chars[slot].getName() + " deleted by ";
            //trans.mMessage.append(acc.getName());
            //storage.addTransaction(trans);

            //acc.delCharacter(slot);
            //storage.flush(acc);

            //reply.writeInt8(ERRMSG_OK);
            //client.send(reply);
        }

        /**
         * Adds server specific info to the current message
         *
         * The info are made of:
         * (String) Update Host URL (or "")
         * (String) Client Data URL (or "")
         * (Byte)   Number of maximum character slots (empty or not)
         */
        void addServerInfo(MessageOut msg)
        {
            msg.writeString(mUpdateHost);
            /*
             * This is for developing/testing an experimental new resource manager that
             * downloads only the files it needs on demand.
             */
            msg.writeString(mDataUrl);
            msg.writeInt8(mMaxCharacters);
        }

        void tokenMatched(AccountClient client, int accountID)
        {
            MessageOut reply=new MessageOut(Protocol.APMSG_RECONNECT_RESPONSE);

            //Associate account with connection.
            ISL.Server.Account.Account acc=Program.storage.getAccount(accountID);
            client.setAccount(acc);
            client.status=AccountClientStatus.CLIENT_CONNECTED;

            reply.writeInt8((int)ErrorMessage.ERRMSG_OK);
            client.send(reply);

            // Return information about available characters
            Dictionary<uint, Character> chars=acc.getCharacters();

            // Send characters list
            foreach(Character character in chars.Values)
            {
                sendCharacterData(client, character);
            }
        }

        void deletePendingClient(AccountClient client)
        {
            MessageOut msg=new MessageOut(Protocol.APMSG_RECONNECT_RESPONSE);
            msg.writeInt8((int)ErrorMessage.ERRMSG_TIME_OUT);
            client.disconnect(msg);
            // The client will be deleted when the disconnect event is processed
        }

        protected override void processMessage(NetComputer comp, MessageIn message)
        {
            AccountClient client=(AccountClient)(comp);

            switch(message.getId())
            {
                case Protocol.PAMSG_LOGIN_RNDTRGR:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_LOGIN_RANDTRIGGER");
                        handleLoginRandTriggerMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_LOGIN:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_LOGIN");
                        handleLoginMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_LOGOUT:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_LOGOUT");
                        handleLogoutMessage(client);
                        break;
                    }
                case Protocol.PAMSG_RECONNECT:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_RECONNECT");
                        handleReconnectMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_REGISTER:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_REGISTER");
                        handleRegisterMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_UNREGISTER:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_UNREGISTER");
                        handleUnregisterMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_REQUEST_REGISTER_INFO:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... REQUEST_REGISTER_INFO");
                        handleRequestRegisterInfoMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_EMAIL_CHANGE:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_EMAIL_CHANGE");
                        handleEmailChangeMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_PASSWORD_CHANGE:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_PASSWORD_CHANGE");
                        handlePasswordChangeMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_CHAR_CREATE:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_CHAR_CREATE");
                        handleCharacterCreateMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_CHAR_SELECT:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_CHAR_SELECT");
                        handleCharacterSelectMessage(client, message);
                        break;
                    }
                case Protocol.PAMSG_CHAR_DELETE:
                    {
                        Logger.Write(LogLevel.Debug, "Received msg ... PAMSG_CHAR_DELETE");
                        handleCharacterDeleteMessage(client, message);
                        break;
                    }
                default:
                    {
                        Logger.Write(LogLevel.Warning, "AccountHandler::processMessage, Invalid message type {0}", message.getId());
                        MessageOut result=new MessageOut(Protocol.XXMSG_INVALID);
                        client.send(result);
                        break;
                    }
            }
        }
    }
}
