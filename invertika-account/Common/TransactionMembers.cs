//
//  TransactionMembers.cs
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

namespace invertika_account.Common
{
	public enum TransactionMembers
	{
		TRANS_CHAR_CREATE=1,
		TRANS_CHAR_SELECTED,
		TRANS_CHAR_DELETED,
		TRANS_MSG_PUBLIC,
		TRANS_MSG_ANNOUNCE,
		TRANS_MSG_PRIVATE,
		TRANS_CHANNEL_JOIN,
		TRANS_CHANNEL_KICK,
		TRANS_CHANNEL_MODE,
		TRANS_CHANNEL_QUIT,
		TRANS_CHANNEL_LIST,
		TRANS_CHANNEL_USERLIST,
		TRANS_CHANNEL_TOPIC,
		TRANS_CMD_BAN,
		TRANS_CMD_DROP,
		TRANS_CMD_ITEM,
		TRANS_CMD_MONEY,
		TRANS_CMD_SETGROUP,
		TRANS_CMD_SPAWN,
		TRANS_CMD_WARP,
		TRANS_ITEM_PICKUP,
		TRANS_ITEM_USED,
		TRANS_ITEM_DROP,
		TRANS_ITEM_MOVE,
		TRANS_ATTACK_TARGET,
		TRANS_ACTION_CHANGE,
		TRANS_TRADE_REQUEST,
		TRANS_TRADE_END,
		TRANS_TRADE_MONEY,
		TRANS_TRADE_ITEM,
		TRANS_ATTR_INCREASE,
		TRANS_ATTR_DECREASE,
		TRANS_CMD_MUTE,
		TRANS_CMD_EXP,
		TRANS_CMD_INVISIBLE,
		TRANS_CMD_COMBAT,
		TRANS_CMD_ANNOUNCE,
		TRANS_CMD_LOCALANNOUNCE,
		TRANS_CMD_KILL,
		TRANS_CMD_FX,
		TRANS_CMD_LOG,
		TRANS_CMD_KILLMONSTERS,
		TRANS_CMD_GOTO,
		TRANS_CMD_GONEXT,
		TRANS_CMD_GOPREV,
		TRANS_CMD_IPBAN,
		TRANS_CMD_WIPE_ITEMS,
		TRANS_CMD_WIPE_LEVEL,
		TRANS_CMD_SHUTDOWN_THIS,
		TRANS_CMD_SHUTDOWN_ALL,
		TRANS_CMD_RESTART_THIS,
		TRANS_CMD_RESTART_ALL,
		TRANS_CMD_ATTRIBUTE,
		TRANS_CMD_KICK
	}
}
