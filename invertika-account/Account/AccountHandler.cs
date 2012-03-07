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

		Dictionary<int, DateTime> mLastLoginAttemptForIP;

		/**
 * Token collector for connecting a client coming from a game server
 * without having to provide username and password a second time.
 */
		public TokenCollector<AccountHandler, AccountClient, int> mTokenCollector;


		public AccountHandler(string attributesFile)
		//mTokenCollector(this),
		//mStartingPoints(0),
		//mAttributeMinimum(0),
		//mAttributeMaximum(0),
		//mNumHairStyles(Configuration::getValue("char_numHairStyles", 17)),
		//mNumHairColors(Configuration::getValue("char_numHairColors", 11)),
		//mNumGenders(Configuration::getValue("char_numGenders", 2)),
		//mMinNameLength(Configuration::getValue("char_minNameLength", 4)),
		//mMaxNameLength(Configuration::getValue("char_maxNameLength", 25)),
		//mMaxCharacters(Configuration::getValue("account_maxCharacters", 3)),
		//mRegistrationAllowed(Configuration::getBoolValue("account_allowRegister", true)),
		//mUpdateHost(Configuration::getValue("net_defaultUpdateHost", std::string())),
		//mDataUrl(Configuration::getValue("net_clientDataUrl", std::string()))
		{
			//XML::Document doc(attributesFile);
			//xmlNodePtr node = doc.rootNode();

			//if (!node || !xmlStrEqual(node->name, BAD_CAST "attributes"))
			//{
			//    LOG_FATAL("Account handler: " << attributesFile << ": "
			//              << " is not a valid database file!");
			//    exit(EXIT_XML_BAD_PARAMETER);
			//}

			//for_each_xml_child_node(attributenode, node)
			//{
			//    if (xmlStrEqual(attributenode->name, BAD_CAST "attribute"))
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
			//    else if (xmlStrEqual(attributenode->name, BAD_CAST "points"))
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
			MessageOut charInfo=new MessageOut(ManaServ.APMSG_CHAR_INFO);

			//charInfo.writeInt8(ch.getCharacterSlot());
			//charInfo.writeString(ch.getName());
			//charInfo.writeInt8(ch.getGender());
			//charInfo.writeInt8(ch.getHairStyle());
			//charInfo.writeInt8(ch.getHairColor());
			//charInfo.writeInt16(ch.getLevel());
			//charInfo.writeInt16(ch.getCharacterPoints());
			//charInfo.writeInt16(ch.getCorrectionPoints());

			//for (AttributeMap::const_iterator it = ch.mAttributes.begin(),
			//                                  it_end = ch.mAttributes.end();
			//    it != it_end;
			//    ++it)
			//{
			//    // {id, base value in 256ths, modified value in 256ths }*
			//    charInfo.writeInt32(it->first);
			//    charInfo.writeInt32((int) (it->second.base * 256));
			//    charInfo.writeInt32((int) (it->second.modified * 256));
			//}

			//client.send(charInfo);
		}

		void handleLoginRandTriggerMessage(AccountClient client, MessageIn msg)
		{
			//std::string salt = getRandomString(4);
			//std::string username = msg.readString();

			//if (Account *acc = storage->getAccount(username))
			//{
			//    acc->setRandomSalt(salt);
			//    mPendingAccounts.push_back(acc);
			//}
			//MessageOut reply(APMSG_LOGIN_RNDTRGR_RESPONSE);
			//reply.writeString(salt);
			//client.send(reply);
		}

		void handleLoginMessage(AccountClient client, MessageIn msg)
		{
			//MessageOut reply(APMSG_LOGIN_RESPONSE);

			//if (client.status != CLIENT_LOGIN)
			//{
			//    reply.writeInt8(ERRMSG_FAILURE);
			//    client.send(reply);
			//    return;
			//}

			//const int clientVersion = msg.readInt32();

			//if (clientVersion < PROTOCOL_VERSION)
			//{
			//    reply.writeInt8(LOGIN_INVALID_VERSION);
			//    client.send(reply);
			//    return;
			//}

			//// Check whether the last login attempt for this IP is still too fresh
			//const int address = client.getIP();
			//const time_t now = time(NULL);
			//IPsToTime::const_iterator it = mLastLoginAttemptForIP.find(address);
			//if (it != mLastLoginAttemptForIP.end())
			//{
			//    const time_t lastAttempt = it->second;
			//    if (now < lastAttempt + 1)
			//    {
			//        reply.writeInt8(LOGIN_INVALID_TIME);
			//        client.send(reply);
			//        return;
			//    }
			//}
			//mLastLoginAttemptForIP[address] = now;

			//const std::string username = msg.readString();
			//const std::string password = msg.readString();

			//if (stringFilter->findDoubleQuotes(username))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    return;
			//}

			//const unsigned maxClients =
			//        (unsigned) Configuration::getValue("net_maxClients", 1000);

			//if (getClientCount() >= maxClients)
			//{
			//    reply.writeInt8(ERRMSG_SERVER_FULL);
			//    client.send(reply);
			//    return;
			//}

			//// Check if the account exists
			//Account *acc = 0;
			//std::list<Account*>::iterator ita;
			//for ( ita = mPendingAccounts.begin() ; ita != mPendingAccounts.end(); ita++ )
			//    if ((*ita)->getName() == username)
			//        acc = *ita;
			//mPendingAccounts.remove(acc);

			//if (!acc || sha256(acc->getPassword() + acc->getRandomSalt()) != password)
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    delete acc;
			//    return;
			//}

			//if (acc->getLevel() == AL_BANNED)
			//{
			//    reply.writeInt8(LOGIN_BANNED);
			//    client.send(reply);
			//    delete acc;
			//    return;
			//}

			//// The client successfully logged in...

			//// Set lastLogin date of the account.
			//time_t login;
			//time(&login);
			//acc->setLastLogin(login);
			//storage->updateLastLogin(acc);

			//// Associate account with connection.
			//client.setAccount(acc);
			//client.status = CLIENT_CONNECTED;

			//reply.writeInt8(ERRMSG_OK);
			//addServerInfo(&reply);
			//client.send(reply); // Acknowledge login

			//// Return information about available characters
			//Characters &chars = acc->getCharacters();

			//// Send characters list
			//for (Characters::const_iterator i = chars.begin(), i_end = chars.end();
			//     i != i_end; ++i)
			//    sendCharacterData(client, *(*i).second);
		}

		void handleLogoutMessage(AccountClient client)
		{
			MessageOut reply=new MessageOut(ManaServ.APMSG_LOGOUT_RESPONSE);

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
			//int clientVersion = msg.readInt32();
			//std::string username = msg.readString();
			//std::string password = msg.readString();
			//std::string email = msg.readString();
			//std::string captcha = msg.readString();

			//MessageOut reply(APMSG_REGISTER_RESPONSE);

			//if (client.status != CLIENT_LOGIN)
			//{
			//    reply.writeInt8(ERRMSG_FAILURE);
			//}
			//else if (!mRegistrationAllowed)
			//{
			//    reply.writeInt8(ERRMSG_FAILURE);
			//}
			//else if (clientVersion < PROTOCOL_VERSION)
			//{
			//    reply.writeInt8(REGISTER_INVALID_VERSION);
			//}
			//else if (stringFilter->findDoubleQuotes(username)
			//         || stringFilter->findDoubleQuotes(email)
			//         || username.length() < mMinNameLength
			//         || username.length() > mMaxNameLength
			//         || !stringFilter->isEmailValid(email)
			//         || !stringFilter->filterContent(username))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (storage->doesUserNameExist(username))
			//{
			//    reply.writeInt8(REGISTER_EXISTS_USERNAME);
			//}
			//else if (storage->doesEmailAddressExist(sha256(email)))
			//{
			//    reply.writeInt8(REGISTER_EXISTS_EMAIL);
			//}
			//else if (!checkCaptcha(client, captcha))
			//{
			//    reply.writeInt8(REGISTER_CAPTCHA_WRONG);
			//}
			//else
			//{
			//    Account *acc = new Account;
			//    acc->setName(username);
			//    acc->setPassword(sha256(password));
			//    // We hash email server-side for additional privacy
			//    // we ask for it again when we need it and verify it
			//    // through comparing it with the hash.
			//    acc->setEmail(sha256(email));
			//    acc->setLevel(AL_PLAYER);

			//    // Set the date and time of the account registration, and the last login
			//    time_t regdate;
			//    time(&regdate);
			//    acc->setRegistrationDate(regdate);
			//    acc->setLastLogin(regdate);

			//    storage->addAccount(acc);
			//    reply.writeInt8(ERRMSG_OK);
			//    addServerInfo(&reply);

			//    // Associate account with connection
			//    client.setAccount(acc);
			//    client.status = CLIENT_CONNECTED;
			//}

			//client.send(reply);
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

			//if (stringFilter->findDoubleQuotes(username))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    return;
			//}

			//// See whether the account exists
			//Account *acc = storage->getAccount(username);

			//if (!acc || acc->getPassword() != sha256(password))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    delete acc;
			//    return;
			//}

			//// Delete account and associated characters
			//LOG_INFO("Unregistered \"" << username
			//         << "\", AccountID: " << acc->getID());
			//storage->delAccount(acc);
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
			MessageOut reply=new MessageOut(ManaServ.APMSG_EMAIL_CHANGE_RESPONSE);

			//Account acc = client.getAccount();
			//if (!acc)
			//{
			//    reply.writeInt8(ERRMSG_NO_LOGIN);
			//    client.send(reply);
			//    return;
			//}

			//const std::string email = msg.readString();
			//const std::string emailHash = sha256(email);

			//if (!stringFilter->isEmailValid(email))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (stringFilter->findDoubleQuotes(email))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (storage->doesEmailAddressExist(emailHash))
			//{
			//    reply.writeInt8(ERRMSG_EMAIL_ALREADY_EXISTS);
			//}
			//else
			//{
			//    acc->setEmail(emailHash);
			//    // Keep the database up to date otherwise we will go out of sync
			//    storage->flush(acc);
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
			//else if (stringFilter->findDoubleQuotes(newPassword))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (oldPassword != acc->getPassword())
			//{
			//    reply.writeInt8(ERRMSG_FAILURE);
			//}
			//else
			//{
			//    acc->setPassword(newPassword);
			//    // Keep the database up to date otherwise we will go out of sync
			//    storage->flush(acc);
			//    reply.writeInt8(ERRMSG_OK);
			//}

			//client.send(reply);
		}

		void handleCharacterCreateMessage(AccountClient client, MessageIn msg)
		{
			//std::string name = msg.readString();
			//int hairStyle = msg.readInt8();
			//int hairColor = msg.readInt8();
			//int gender = msg.readInt8();

			//// Avoid creation of character from old clients.
			//int slot = -1;
			//if (msg.getUnreadLength() > 7)
			//    slot = msg.readInt8();

			//MessageOut reply(APMSG_CHAR_CREATE_RESPONSE);

			//Account *acc = client.getAccount();
			//if (!acc)
			//{
			//    reply.writeInt8(ERRMSG_NO_LOGIN);
			//}
			//else if (!stringFilter->filterContent(name))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (stringFilter->findDoubleQuotes(name))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else if (hairStyle > mNumHairStyles)
			//{
			//    reply.writeInt8(CREATE_INVALID_HAIRSTYLE);
			//}
			//else if (hairColor > mNumHairColors)
			//{
			//    reply.writeInt8(CREATE_INVALID_HAIRCOLOR);
			//}
			//else if (gender > mNumGenders)
			//{
			//    reply.writeInt8(CREATE_INVALID_GENDER);
			//}
			//else if ((name.length() < mMinNameLength) ||
			//         (name.length() > mMaxNameLength))
			//{
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//}
			//else
			//{
			//    if (storage->doesCharacterNameExist(name))
			//    {
			//        reply.writeInt8(CREATE_EXISTS_NAME);
			//        client.send(reply);
			//        return;
			//    }

			//    // An account shouldn't have more
			//    // than <account_maxCharacters> characters.
			//    Characters &chars = acc->getCharacters();
			//    if (slot < 1 || slot > mMaxCharacters
			//        || !acc->isSlotEmpty((unsigned int) slot))
			//    {
			//        reply.writeInt8(CREATE_INVALID_SLOT);
			//        client.send(reply);
			//        return;
			//    }

			//    if ((int)chars.size() >= mMaxCharacters)
			//    {
			//        reply.writeInt8(CREATE_TOO_MUCH_CHARACTERS);
			//        client.send(reply);
			//        return;
			//    }

			//    // TODO: Add race, face and maybe special attributes.

			//    // Customization of character's attributes...
			//    std::vector<int> attributes = std::vector<int>(mModifiableAttributes.size(), 0);
			//    for (unsigned int i = 0; i < mModifiableAttributes.size(); ++i)
			//        attributes[i] = msg.readInt16();

			//    int totalAttributes = 0;
			//    for (unsigned int i = 0; i < mModifiableAttributes.size(); ++i)
			//    {
			//        // For good total attributes check.
			//        totalAttributes += attributes.at(i);

			//        // For checking if all stats are >= min and <= max.
			//        if (attributes.at(i) < mAttributeMinimum
			//            || attributes.at(i) > mAttributeMaximum)
			//        {
			//            reply.writeInt8(CREATE_ATTRIBUTES_OUT_OF_RANGE);
			//            client.send(reply);
			//            return;
			//        }
			//    }

			//    if (totalAttributes > mStartingPoints)
			//    {
			//        reply.writeInt8(CREATE_ATTRIBUTES_TOO_HIGH);
			//    }
			//    else if (totalAttributes < mStartingPoints)
			//    {
			//        reply.writeInt8(CREATE_ATTRIBUTES_TOO_LOW);
			//    }
			//    else
			//    {
			//        Character *newCharacter = new Character(name);

			//        // Set the initial attributes provided by the client
			//        for (unsigned int i = 0; i < mModifiableAttributes.size(); ++i)
			//        {
			//            newCharacter->mAttributes.insert(
			//                        std::make_pair(mModifiableAttributes.at(i), attributes[i]));
			//        }

			//        newCharacter->mAttributes.insert(mDefaultAttributes.begin(),
			//                                         mDefaultAttributes.end());
			//        newCharacter->setAccount(acc);
			//        newCharacter->setCharacterSlot(slot);
			//        newCharacter->setGender(gender);
			//        newCharacter->setHairStyle(hairStyle);
			//        newCharacter->setHairColor(hairColor);
			//        newCharacter->setMapId(Configuration::getValue("char_startMap", 1));
			//        Point startingPos(Configuration::getValue("char_startX", 1024),
			//                          Configuration::getValue("char_startY", 1024));
			//        newCharacter->setPosition(startingPos);
			//        acc->addCharacter(newCharacter);

			//        LOG_INFO("Character " << name << " was created for "
			//                 << acc->getName() << "'s account.");

			//        storage->flush(acc); // flush changes

			//        // log transaction
			//        Transaction trans;
			//        trans.mCharacterId = newCharacter->getDatabaseID();
			//        trans.mAction = TRANS_CHAR_CREATE;
			//        trans.mMessage = acc->getName() + " created character ";
			//        trans.mMessage.append("called " + name);
			//        storage->addTransaction(trans);

			//        reply.writeInt8(ERRMSG_OK);
			//        client.send(reply);

			//        // Send new characters infos back to client
			//        sendCharacterData(client, *chars[slot]);
			//        return;
			//    }
			//}

			//client.send(reply);
		}

		void handleCharacterSelectMessage(AccountClient client, MessageIn msg)
		{
			//MessageOut reply(APMSG_CHAR_SELECT_RESPONSE);

			//Account *acc = client.getAccount();
			//if (!acc)
			//{
			//    reply.writeInt8(ERRMSG_NO_LOGIN);
			//    client.send(reply);
			//    return; // not logged in
			//}

			//int slot = msg.readInt8();
			//Characters &chars = acc->getCharacters();

			//if (chars.find(slot) == chars.end())
			//{
			//    // Invalid char selection
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    return;
			//}

			//Character *selectedChar = chars[slot];

			//std::string address;
			//int port;
			//if (!GameServerHandler::getGameServerFromMap
			//        (selectedChar->getMapId(), address, port))
			//{
			//    LOG_ERROR("Character Selection: No game server for map #"<<selectedChar->getMapId());
			//    reply.writeInt8(ERRMSG_FAILURE);
			//    client.send(reply);
			//    return;
			//}

			//reply.writeInt8(ERRMSG_OK);

			//LOG_DEBUG(selectedChar->getName() << " is trying to enter the servers.");

			//std::string magic_token(utils::getMagicToken());
			//reply.writeString(magic_token, MAGIC_TOKEN_LENGTH);
			//reply.writeString(address);
			//reply.writeInt16(port);

			//// Give address and port for the chat server
			//reply.writeString(Configuration::getValue("net_chatHost",
			//                                          "localhost"));

			//// When the chatListenToClientPort is set, we use it.
			//// Otherwise, we use the accountListenToClientPort + 2 if the option is set.
			//// If neither, the DEFAULT_SERVER_PORT + 2 is used.
			//const int alternativePort =
			//    Configuration::getValue("net_accountListenToClientPort",
			//                            DEFAULT_SERVER_PORT) + 2;
			//reply.writeInt16(Configuration::getValue("net_chatListenToClientPort",
			//                                         alternativePort));

			//GameServerHandler::registerClient(magic_token, selectedChar);
			//registerChatClient(magic_token, selectedChar->getName(), acc->getLevel());

			//client.send(reply);

			//// log transaction
			//Transaction trans;
			//trans.mCharacterId = selectedChar->getDatabaseID();
			//trans.mAction = TRANS_CHAR_SELECTED;
			//storage->addTransaction(trans);
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
			//Characters &chars = acc->getCharacters();

			//if (slot < 1 || acc->isSlotEmpty(slot))
			//{
			//    // Invalid char selection
			//    reply.writeInt8(ERRMSG_INVALID_ARGUMENT);
			//    client.send(reply);
			//    return;
			//}

			//std::string characterName = chars[slot]->getName();
			//LOG_INFO("Character deleted:" << characterName);

			//// Log transaction
			//Transaction trans;
			//trans.mCharacterId = chars[slot]->getDatabaseID();
			//trans.mAction = TRANS_CHAR_DELETED;
			//trans.mMessage = chars[slot]->getName() + " deleted by ";
			//trans.mMessage.append(acc->getName());
			//storage->addTransaction(trans);

			//acc->delCharacter(slot);
			//storage->flush(acc);

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
			MessageOut reply=new MessageOut(ManaServ.APMSG_RECONNECT_RESPONSE);

			//Associate account with connection.
			Account acc=Program.storage.getAccount(accountID);
			client.setAccount(acc);
			client.status=AccountClientStatus.CLIENT_CONNECTED;

			reply.writeInt8(ManaServ.ERRMSG_OK);
			client.send(reply);

			// Return information about available characters
			Dictionary<uint, Character> chars = acc.getCharacters();

			// Send characters list
			foreach(Character character in chars.Values)
			{
				sendCharacterData(client, character);
			}
		}

		void deletePendingClient(AccountClient client)
		{
			MessageOut msg=new MessageOut(ManaServ.APMSG_RECONNECT_RESPONSE);
			msg.writeInt8(ManaServ.ERRMSG_TIME_OUT);
			client.disconnect(msg);
			// The client will be deleted when the disconnect event is processed
		}

		protected override void processMessage(NetComputer comp, MessageIn message)
		{
			AccountClient client=(AccountClient)(comp);

			switch(message.getId())
			{
				case ManaServ.PAMSG_LOGIN_RNDTRGR:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_LOGIN_RANDTRIGGER");
						handleLoginRandTriggerMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_LOGIN:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_LOGIN");
						handleLoginMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_LOGOUT:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_LOGOUT");
						handleLogoutMessage(client);
						break;
					}
				case ManaServ.PAMSG_RECONNECT:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_RECONNECT");
						handleReconnectMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_REGISTER:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_REGISTER");
						handleRegisterMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_UNREGISTER:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_UNREGISTER");
						handleUnregisterMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_REQUEST_REGISTER_INFO:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... REQUEST_REGISTER_INFO");
						handleRequestRegisterInfoMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_EMAIL_CHANGE:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_EMAIL_CHANGE");
						handleEmailChangeMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_PASSWORD_CHANGE:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_PASSWORD_CHANGE");
						handlePasswordChangeMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_CHAR_CREATE:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_CHAR_CREATE");
						handleCharacterCreateMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_CHAR_SELECT:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_CHAR_SELECT");
						handleCharacterSelectMessage(client, message);
						break;
					}
				case ManaServ.PAMSG_CHAR_DELETE:
					{
						Logger.Add(LogLevel.Debug, "Received msg ... PAMSG_CHAR_DELETE");
						handleCharacterDeleteMessage(client, message);
						break;
					}
				default:
					{
						Logger.Add(LogLevel.Warning, "AccountHandler::processMessage, Invalid message type {0}", message.getId());
						MessageOut result=new MessageOut((int)ManaServ.XXMSG_INVALID);
						client.send(result);
						break;
					}
			}
		}
	}
}
