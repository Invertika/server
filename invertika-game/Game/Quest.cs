//  
//  Quest.cs
//  
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2012 seeseekey
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

namespace invertika_game.Game
{
	public static class Quest
	{
		
		static void recoverQuestVar(Character ch, string name, QuestCallback f)
		{
//    assert(ch->questCache.find(name) == ch->questCache.end());
//    int id = ch->getDatabaseID();
//    PendingQuests::iterator i = pendingQuests.lower_bound(id);
//    if (i == pendingQuests.end() || i->first != id)
//    {
//        i = pendingQuests.insert(i, std::make_pair(id, PendingQuest()));
//        i->second.character = ch;
//        /* Register a listener, because we cannot afford to get invalid
//           pointers, when we finally recover the variable. */
//        ch->addListener(&questDeathListener);
//    }
//    i->second.variables[name].push_back(f);
//    accountHandler->requestCharacterVar(ch, name);
		}

		public static void recoveredQuestVar(int id, string name, string value)
		{
//    PendingQuests::iterator i = pendingQuests.find(id);
//    if (i == pendingQuests.end()) return;
//
//    Character *ch = i->second.character;
//    ch->removeListener(&questDeathListener);
//
//    PendingVariables &variables = i->second.variables;
//    PendingVariables::iterator j = variables.find(name);
//    if (j == variables.end())
//    {
//        LOG_ERROR("Account server recovered an unexpected quest variable.");
//        return;
//    }
//
//    ch->questCache[name] = value;
//
//    // Call the registered callbacks.
//    for (QuestCallbacks::const_iterator k = j->second.begin(),
//         k_end = j->second.end(); k != k_end; ++k)
//    {
//        k->handler(ch, name, value, k->data);
//    }
//
//    variables.erase(j);
//    if (variables.empty())
//    {
//        pendingQuests.erase(i);
//    }
		}
	}
}

