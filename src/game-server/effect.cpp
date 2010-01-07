/*
 *  The Mana Server
 *  Copyright (C) 2004  The Mana World Development Team
 *
 *  This file is part of The Mana Server.
 *
 *  The Mana Server is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  any later version.
 *
 *  The Mana Server is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with The Mana Server.  If not, see <http://www.gnu.org/licenses/>.
 */

#include "game-server/effect.hpp"

#include "game-server/mapcomposite.hpp"
#include "game-server/state.hpp"

void Effect::update()
{
    if (mHasBeenShown)
        GameState::enqueueRemove(this);
}

namespace Effects
{
    void show(int id, MapComposite *map, const Point &pos)
    {
        Effect *effect = new Effect(id);
        effect->setMap(map);
        effect->setPosition(pos);
        GameState::enqueueInsert(effect);
    }
    void show(int id, MapComposite *map, Being *b)
    {
        Effect *effect = new Effect(id);
        effect->setMap(map);
        if (effect->setBeing(b))
            GameState::enqueueInsert(effect);
    }
}