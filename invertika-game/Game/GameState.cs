//
//  GameState.cs
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
using ISL.Server.Network;
using invertika_game.Scripting;
using ISL.Server.Common;
using ISL.Server.Utilities;
using invertika_game.Enums;
using System.Diagnostics;

namespace invertika_game.Game
{
    public static class GameState
    {
        /**
		 * List of delayed events.
		 */
        static Dictionary<Actor, DelayedEvent> delayedEvents=new Dictionary<Actor, DelayedEvent>();

        /**
		 * Cached persistent script variables
		 */
        static Dictionary<string, string> mScriptVariables;


        /**
		 * Updates object states on the map.
		 */
        static void updateMap(MapComposite map)
		{
			// 1. update object status.
			List< Thing  > things = map.getEverything(); //TODO das könnte man auch weiter unten benutzen?

			foreach(Thing thing in things)
			{
				thing.update();
			}

			// 2. run scripts.
			Script s = map.getScript();
			if(s != null)
			{
				s.update();
			}

			// 3. perform actions.
			if(map.mContent!=null)
			{
			foreach(Thing thing in map.mContent.things)
			{
				if(thing is Being)
				{
					Being being = (Being)thing;
					being.perform();
				}
			}
			}

			//Alter Code
			// for (BeingIterator i(map.getWholeMapIterator()); i; ++i)
			// {
			//     (*i).perform();
			// }

			// 4. move objects around and update zones.
			foreach(Thing thing in map.mContent.things)
			{
				if(thing is Being)
				{
					Being being = (Being)thing;
					being.move();
				}
			}

			// Alter Code
			//for (BeingIterator i(map.getWholeMapIterator()); i; ++i)
			//{
			//    (*i).move();
			//}

			map.update();
		}

        /**
		 * Sets message fields describing character look.
		 */
        static void serializeLooks(Character ch, MessageOut msg, bool full)
        {
            //const Possessions &poss = ch.getPossessions();
            //unsigned int nb_slots = itemManager.getVisibleSlotCount();

            //// Bitmask describing the changed entries.
            //int changed = (1 << nb_slots) - 1;
            //if (!full)
            //{
            //    // TODO: do not assume the whole equipment changed,
            //    // when an update is asked for.
            //    changed = (1 << nb_slots) - 1;
            //}

            //std::vector<unsigned int> items;
            //items.resize(nb_slots, 0);
            //// Partially build both kinds of packet, to get their sizes.
            //unsigned int mask_full = 0, mask_diff = 0;
            //unsigned int nb_full = 0, nb_diff = 0;
            //std::map<unsigned int, unsigned int>::const_iterator it =
            //                                                poss.equipSlots.begin();
            //for (unsigned int i = 0; i < nb_slots; ++i)
            //{
            //    if (changed & (1 << i))
            //    {
            //        // Skip slots that have not changed, when sending an update.
            //        ++nb_diff;
            //        mask_diff |= 1 << i;
            //    }
            //    if (it == poss.equipSlots.end() || it.first > i) continue;
            //    ItemClass *eq;
            //    items[i] = it.first && (eq = itemManager.getItem(it.first)) ?
            //               eq.getSpriteID() : 0;
            //    if (items[i])
            //    {
            //        /* If we are sending the whole equipment, only filled slots have to
            //           be accounted for, as the other ones will be automatically cleared. */
            //        ++nb_full;
            //        mask_full |= 1 << i;
            //    }
            //}

            //// Choose the smaller payload.
            //if (nb_full <= nb_diff) full = true;

            ///* Bitmask enumerating the sent slots.
            //   Setting the upper bit tells the client to clear the slots beforehand. */
            //int mask = full ? mask_full | (1 << 7) : mask_diff;

            //msg.writeInt8(mask);
            //for (unsigned int i = 0; i < nb_slots; ++i)
            //{
            //    if (mask & (1 << i)) msg.writeInt16(items[i]);
            //}
        }

        /**
		 * Informs a player of what happened around the character.
		 */
        static void informPlayer(MapComposite map, Character p, int worldTime)
        {
            //MessageOut moveMsg(GPMSG_BEINGS_MOVE);
            //MessageOut damageMsg(GPMSG_BEINGS_DAMAGE);
            //const Point &pold = p.getOldPosition(), ppos = p.getPosition();
            //int pid = p.getPublicID(), pflags = p.getUpdateFlags();
            //int visualRange = Configuration::getValue("game_visualRange", 448);

            //// Inform client about activities of other beings near its character
            //for (BeingIterator i(map.getAroundBeingIterator(p, visualRange)); i; ++i)
            //{
            //    Being *o = *i;

            //    const Point &oold = o.getOldPosition(), opos = o.getPosition();
            //    int otype = o.getType();
            //    int oid = o.getPublicID(), oflags = o.getUpdateFlags();
            //    int flags = 0;

            //    // Check if the character p and the moving object o are around.
            //    bool wereInRange = pold.inRangeOf(oold, visualRange) &&
            //                       !((pflags | oflags) & UPDATEFLAG_NEW_ON_MAP);
            //    bool willBeInRange = ppos.inRangeOf(opos, visualRange);

            //    if (!wereInRange && !willBeInRange)
            //    {
            //        // Nothing to report: o and p are far away from each other.
            //        continue;
            //    }


            //    if (wereInRange && willBeInRange)
            //    {
            //        // Send attack messages.
            //        if ((oflags & UPDATEFLAG_ATTACK) && oid != pid)
            //        {
            //            MessageOut AttackMsg(GPMSG_BEING_ATTACK);
            //            AttackMsg.writeInt16(oid);
            //            AttackMsg.writeInt8(o.getDirection());
            //            AttackMsg.writeInt8(static_cast< Being * >(o).getAttackId());
            //            gameHandler.sendTo(p, AttackMsg);
            //        }

            //        // Send action change messages.
            //        if ((oflags & UPDATEFLAG_ACTIONCHANGE))
            //        {
            //            MessageOut ActionMsg(GPMSG_BEING_ACTION_CHANGE);
            //            ActionMsg.writeInt16(oid);
            //            ActionMsg.writeInt8(static_cast< Being * >(o).getAction());
            //            gameHandler.sendTo(p, ActionMsg);
            //        }

            //        // Send looks change messages.
            //        if (oflags & UPDATEFLAG_LOOKSCHANGE)
            //        {
            //            MessageOut LooksMsg(GPMSG_BEING_LOOKS_CHANGE);
            //            LooksMsg.writeInt16(oid);
            //            Character * c = static_cast<Character * >(o);
            //            serializeLooks(c, LooksMsg, false);
            //            LooksMsg.writeInt16(c.getHairStyle());
            //            LooksMsg.writeInt16(c.getHairColor());
            //            LooksMsg.writeInt16(c.getGender());
            //            gameHandler.sendTo(p, LooksMsg);
            //        }

            //        // Send direction change messages.
            //        if (oflags & UPDATEFLAG_DIRCHANGE)
            //        {
            //            MessageOut DirMsg(GPMSG_BEING_DIR_CHANGE);
            //            DirMsg.writeInt16(oid);
            //            DirMsg.writeInt8(o.getDirection());
            //            gameHandler.sendTo(p, DirMsg);
            //        }

            //        // Send damage messages.
            //        if (o.canFight())
            //        {
            //            Being *victim = static_cast< Being * >(o);
            //            const Hits &hits = victim.getHitsTaken();
            //            for (Hits::const_iterator j = hits.begin(),
            //                 j_end = hits.end(); j != j_end; ++j)
            //            {
            //                damageMsg.writeInt16(oid);
            //                damageMsg.writeInt16(*j);
            //            }
            //        }

            //        if (oold == opos)
            //        {
            //            // o does not move, nothing more to report.
            //            continue;
            //        }
            //    }

            //    if (!willBeInRange)
            //    {
            //        // o is no longer visible from p. Send leave message.
            //        MessageOut leaveMsg(GPMSG_BEING_LEAVE);
            //        leaveMsg.writeInt16(oid);
            //        gameHandler.sendTo(p, leaveMsg);
            //        continue;
            //    }

            //    if (!wereInRange)
            //    {
            //        // o is now visible by p. Send enter message.
            //        MessageOut enterMsg(GPMSG_BEING_ENTER);
            //        enterMsg.writeInt8(otype);
            //        enterMsg.writeInt16(oid);
            //        enterMsg.writeInt8(static_cast< Being *>(o).getAction());
            //        enterMsg.writeInt16(opos.x);
            //        enterMsg.writeInt16(opos.y);
            //        enterMsg.writeInt8(o.getDirection());
            //        switch (otype)
            //        {
            //            case OBJECT_CHARACTER:
            //            {
            //                Character *q = static_cast< Character * >(o);
            //                enterMsg.writeString(q.getName());
            //                enterMsg.writeInt8(q.getHairStyle());
            //                enterMsg.writeInt8(q.getHairColor());
            //                enterMsg.writeInt8(q.getGender());
            //                serializeLooks(q, enterMsg, true);
            //            } break;

            //            case OBJECT_MONSTER:
            //            {
            //                Monster *q = static_cast< Monster * >(o);
            //                enterMsg.writeInt16(q.getSpecy().getId());
            //                enterMsg.writeString(q.getName());
            //            } break;

            //            case OBJECT_NPC:
            //            {
            //                NPC *q = static_cast< NPC * >(o);
            //                enterMsg.writeInt16(q.getNPC());
            //                enterMsg.writeString(q.getName());
            //            } break;

            //            default:
            //                assert(false); // TODO
            //        }
            //        gameHandler.sendTo(p, enterMsg);
            //    }

            //    if (opos != oold)
            //    {
            //        // Add position check coords every 5 seconds.
            //        if (worldTime % 50 == 0)
            //            flags |= MOVING_POSITION;

            //        flags |= MOVING_DESTINATION;
            //    }

            //    // Send move messages.
            //    moveMsg.writeInt16(oid);
            //    moveMsg.writeInt8(flags);
            //    if (flags & MOVING_POSITION)
            //    {
            //        moveMsg.writeInt16(oold.x);
            //        moveMsg.writeInt16(oold.y);
            //    }

            //    if (flags & MOVING_DESTINATION)
            //    {
            //        moveMsg.writeInt16(opos.x);
            //        moveMsg.writeInt16(opos.y);
            //        // We multiply the sent speed (in tiles per second) by ten
            //        // to get it within a byte with decimal precision.
            //        // For instance, a value of 4.5 will be sent as 45.
            //        moveMsg.writeInt8((unsigned short)
            //            (o.getModifiedAttribute(ATTR_MOVE_SPEED_TPS) * 10));
            //    }
            //}

            //// Do not send a packet if nothing happened in p's range.
            //if (moveMsg.getLength() > 2)
            //    gameHandler.sendTo(p, moveMsg);

            //if (damageMsg.getLength() > 2)
            //    gameHandler.sendTo(p, damageMsg);

            //// Inform client about status change.
            //p.sendStatus();

            //// Inform client about health change of party members
            //for (CharacterIterator i(map.getWholeMapIterator()); i; ++i)
            //{
            //    Character *c = *i;

            //    // Make sure its not the same character
            //    if (c == p)
            //        continue;

            //    // make sure they are in the same party
            //    if (c.getParty() == p.getParty())
            //    {
            //        int cflags = c.getUpdateFlags();
            //        if (cflags & UPDATEFLAG_HEALTHCHANGE)
            //        {
            //            MessageOut healthMsg(GPMSG_BEING_HEALTH_CHANGE);
            //            healthMsg.writeInt16(c.getPublicID());
            //            healthMsg.writeInt16(c.getModifiedAttribute(ATTR_HP));
            //            healthMsg.writeInt16(c.getModifiedAttribute(ATTR_MAX_HP));
            //            gameHandler.sendTo(p, healthMsg);
            //        }
            //    }
            //}

            //// Inform client about items on the ground around its character
            //MessageOut itemMsg(GPMSG_ITEMS);
            //for (FixedActorIterator i(map.getAroundBeingIterator(p, visualRange)); i;
            //                                                                        ++i)
            //{
            //    assert((*i).getType() == OBJECT_ITEM ||
            //           (*i).getType() == OBJECT_EFFECT);

            //    Actor *o = *i;
            //    Point opos = o.getPosition();
            //    int oflags = o.getUpdateFlags();
            //    bool willBeInRange = ppos.inRangeOf(opos, visualRange);
            //    bool wereInRange = pold.inRangeOf(opos, visualRange) &&
            //                       !((pflags | oflags) & UPDATEFLAG_NEW_ON_MAP);

            //    if (willBeInRange ^ wereInRange)
            //    {
            //        switch (o.getType())
            //        {
            //            case OBJECT_ITEM:
            //            {
            //                Item *o = static_cast< Item * >(*i);
            //                if (oflags & UPDATEFLAG_NEW_ON_MAP)
            //                {
            //                    /* Send a specific message to the client when an item appears
            //                       out of nowhere, so that a sound/animation can be performed. */
            //                    MessageOut appearMsg(GPMSG_ITEM_APPEAR);
            //                    appearMsg.writeInt16(o.getItemClass().getDatabaseID());
            //                    appearMsg.writeInt16(opos.x);
            //                    appearMsg.writeInt16(opos.y);
            //                    gameHandler.sendTo(p, appearMsg);
            //                }
            //                else
            //                {
            //                    itemMsg.writeInt16(willBeInRange ? o.getItemClass().getDatabaseID() : 0);
            //                    itemMsg.writeInt16(opos.x);
            //                    itemMsg.writeInt16(opos.y);
            //                }
            //            }
            //            break;
            //            case OBJECT_EFFECT:
            //            {
            //                Effect *o = static_cast< Effect * >(*i);
            //                o.show();
            //                // Don't show old effects
            //                if (!(oflags & UPDATEFLAG_NEW_ON_MAP))
            //                    break;
            //                Being *b = o.getBeing();
            //                if (b)
            //                {
            //                    MessageOut effectMsg(GPMSG_CREATE_EFFECT_BEING);
            //                    effectMsg.writeInt16(o.getEffectId());
            //                    effectMsg.writeInt16(b.getPublicID());
            //                    gameHandler.sendTo(p, effectMsg);
            //                } else {
            //                    MessageOut effectMsg(GPMSG_CREATE_EFFECT_POS);
            //                    effectMsg.writeInt16(o.getEffectId());
            //                    effectMsg.writeInt16(opos.x);
            //                    effectMsg.writeInt16(opos.y);
            //                    gameHandler.sendTo(p, effectMsg);
            //                }
            //            }
            //            break;
            //            default: break;
            //        } // Switch
            //    }
            //}

            //// Do not send a packet if nothing happened in p's range.
            //if (itemMsg.getLength() > 2)
            //    gameHandler.sendTo(p, itemMsg);
        }

        //#ifndef NDEBUG
        //static bool dbgLockObjects;
        //#endif

        public static void update(int worldTime)
		{
//            #   ifndef NDEBUG
//                dbgLockObjects = true;
//            #   endif

			// Update game state (update AI, etc.)
			Dictionary<int, MapComposite> maps = MapManager.getMaps();

			foreach(KeyValuePair<int, MapComposite> pair in maps)
			{
				MapComposite map = pair.Value;

				if(!map.isActive())
				{
					continue;
				}

				updateMap(map);

//				for (CharacterIterator p(map.getWholeMapIterator()); p; ++p)
//				{
//					informPlayer(map, *p, worldTime);
//					/*
//                         sending the whole character is overhead for the database, it should
//                         be replaced by a syncbuffer. see: game-server/accountconnection:
//                         AccountConnection::syncChanges()
//
//                        if (worldTime % 2000 == 0)
//                        {
//                            accountHandler.sendCharacterData(*p);
//                        }
//                        */
//				}
//
//				for (ActorIterator i(map.getWholeMapIterator()); i; ++i)
//				{
//					Actor *a = *i;
//					a.clearUpdateFlags();
//					if (a.canFight())
//					{
//						static_cast< Being * >(a).clearHitsTaken();
//					}
//				}
			}


//            #   ifndef NDEBUG
//                dbgLockObjects = false;
//            #   endif

			// Take care of events that were delayed because of their side effects.
			foreach(KeyValuePair<Actor, DelayedEvent> pair in delayedEvents)
			{
				DelayedEvent e = pair.Value;
				Actor o = pair.Key;

				switch((Event)e.type)
				{
				case Event.EVENT_REMOVE:
					remove(o);
					if(o.getType() == ThingType.OBJECT_CHARACTER)
					{
						Character ch = (Character)o;
						ch.disconnected();
						Program.gameHandler.kill(ch);
					}
                            //delete o;
					break;

				case Event.EVENT_INSERT:
					insertOrDelete(o);
					break;

				case Event.EVENT_WARP:
					Debug.Assert(o.getType() == ThingType.OBJECT_CHARACTER);
					warp((Character)o, e.map, e.x, e.y);
					break;
				}
			}

			delayedEvents.Clear();
		}

        public static bool insert(Thing ptr)
        {
            //assert(!dbgLockObjects);

            MapComposite map=ptr.getMap();
            //assert(map && map.isActive());

            /* Non-visible objects have neither position nor public ID, so their
               insertion cannot fail. Take care of them first. */
            if(!ptr.isVisible())
            {
                map.insert(ptr);
                ptr.inserted();
                return true;
            }

            // Check that coordinates are actually valid.
            Actor obj=(Actor)ptr;
            Map mp=map.getMap();
           
            Point pos=obj.getPosition();
            if((int)pos.x/mp.getTileWidth()>=mp.getWidth()||
                (int)pos.y/mp.getTileHeight()>=mp.getHeight())
            {
                Logger.Write(LogLevel.Error, "Tried to insert an actor at position {0}, {1} outside map {2}.", pos.x, pos.y, map.getID());

                // Set an arbitrary small position.
                pos=new Point(100, 100);
                obj.setPosition(pos);
            }

            if(!map.insert(obj))
            {
                // The map is overloaded, no room to add a new actor
                Logger.Write(LogLevel.Error, "Too many actors on map {0}.", map.getID());
                return false;
            }

            obj.inserted();

            // DEBUG INFO //TODO Implementieren
//            switch(obj.getType())
//            {
//                case ThingType.OBJECT_ITEM:
//                    Logger.Write(LogLevel.Debug, "Item inserted: "
//                       (Item)(obj).getItemClass().getDatabaseID());
//                    break;
//
//                case ThingType.OBJECT_NPC:
//                    Logger.Write(LogLevel.Debug, "NPC inserted: "<<static_cast<NPC*>(obj).getNPC());
//                    break;
//
//                case ThingType.OBJECT_CHARACTER:
//                    Logger.Write(LogLevel.Debug, "Player inserted: "
//                        <<static_cast<Being*>(obj).getName());
//                    break;
//
//                case ThingType.OBJECT_EFFECT:
//                    Logger.Write(LogLevel.Debug, "Effect inserted: "
//                        <<static_cast<Effect*>(obj).getEffectId());
//                    break;
//
//                case ThingType.OBJECT_MONSTER:
//                    Logger.Write(LogLevel.Debug, "Monster inserted: "
//                        <<static_cast<Monster*>(obj).getSpecy().getId());
//                    break;
//
//                case ThingType.OBJECT_ACTOR:
//                case ThingType.OBJECT_OTHER:
//                default:
//                    Logger.Write(LogLevel.Debug, "Thing inserted: "<<obj.getType());
//            }

            obj.raiseUpdateFlags((byte)UpdateFlag.UPDATEFLAG_NEW_ON_MAP);
            if(obj.getType()!=ThingType.OBJECT_CHARACTER)
                return true;

            /* Since the player does not know yet where in the world its character is,
               we send a map-change message, even if it is the first time it
               connects to this server. */
            MessageOut mapChangeMessage=new MessageOut(Protocol.GPMSG_PLAYER_MAP_CHANGE);
            mapChangeMessage.writeString(map.getName());
            mapChangeMessage.writeInt16(pos.x);
            mapChangeMessage.writeInt16(pos.y);
            Program.gameHandler.sendTo((Character)(obj), mapChangeMessage);

            // update the online state of the character
            Program.accountHandler.updateOnlineStatus(((Character)obj).getDatabaseID(), true);

            return true;
        }

        public static bool insertOrDelete(Thing ptr)
        {
            //if (insert(ptr)) return true;
            //delete ptr;
            return false;
        }

        public static void remove(Thing ptr)
        {
            //assert(!dbgLockObjects);
            //MapComposite *map = ptr.getMap();
            //int visualRange = Configuration::getValue("game_visualRange", 448);

            //ptr.removed();

            //// DEBUG INFO
            //switch (ptr.getType())
            //{
            //    case OBJECT_ITEM:
            //        LOG_DEBUG("Item removed: "
            //               << static_cast<Item*>(ptr).getItemClass().getDatabaseID());
            //        break;

            //    case OBJECT_NPC:
            //        LOG_DEBUG("NPC removed: " << static_cast<NPC*>(ptr).getNPC());
            //        break;

            //    case OBJECT_CHARACTER:
            //        LOG_DEBUG("Player removed: "
            //                  << static_cast<Being*>(ptr).getName());
            //        break;

            //    case OBJECT_EFFECT:
            //        LOG_DEBUG("Effect removed: "
            //                  << static_cast<Effect*>(ptr).getEffectId());
            //        break;

            //    case OBJECT_MONSTER:
            //        LOG_DEBUG("Monster removed: "
            //                  << static_cast<Monster*>(ptr).getSpecy().getId());
            //        break;

            //    case OBJECT_ACTOR:
            //    case OBJECT_OTHER:
            //    default:
            //        LOG_DEBUG("Thing removed: " << ptr.getType());
            //}

            //if (ptr.canMove())
            //{
            //    if (ptr.getType() == OBJECT_CHARACTER)
            //    {
            //        static_cast< Character * >(ptr).cancelTransaction();

            //        // remove characters online status
            //        accountHandler.updateOnlineStatus(
            //            static_cast< Character * >(ptr).getDatabaseID(), false);
            //    }

            //    Actor *obj = static_cast< Actor * >(ptr);
            //    MessageOut msg(GPMSG_BEING_LEAVE);
            //    msg.writeInt16(obj.getPublicID());
            //    Point objectPos = obj.getPosition();

            //    for (CharacterIterator p(map.getAroundActorIterator(obj, visualRange));
            //         p; ++p)
            //    {
            //        if (*p != obj && objectPos.inRangeOf((*p).getPosition(),
            //            visualRange))
            //        {
            //            gameHandler.sendTo(*p, msg);
            //        }
            //    }
            //}
            //else if (ptr.getType() == OBJECT_ITEM)
            //{
            //    Item *obj = static_cast< Item * >(ptr);
            //    Point pos = obj.getPosition();
            //    MessageOut msg(GPMSG_ITEMS);
            //    msg.writeInt16(0);
            //    msg.writeInt16(pos.x);
            //    msg.writeInt16(pos.y);

            //    for (CharacterIterator p(map.getAroundActorIterator(obj, visualRange)); p; ++p)
            //    {
            //        if (pos.inRangeOf((*p).getPosition(), visualRange))
            //        {
            //            gameHandler.sendTo(*p, msg);
            //        }
            //    }
            //}

            //map.remove(ptr);
        }

        static void warp(Character ptr, MapComposite map, int x, int y)
        {
            //TODO DEBUG555 entfernen
            int debug=555;

            remove(ptr);
            ptr.setMap(map);
            ptr.setPosition(new Point(x, y));
            ptr.clearDestination();
            /* Force update of persistent data on map change, so that
               characters can respawn at the start of the map after a death or
               a disconnection. */
            Program.accountHandler.sendCharacterData(ptr);

            if(map.isActive())
            {
                if(!insert(ptr))
                {
                    ptr.disconnected();
                    Program.gameHandler.kill(ptr);
                }
            }
            else
            {
                MessageOut msg=new MessageOut(Protocol.GAMSG_REDIRECT);
                msg.writeInt32(ptr.getDatabaseID());
                Program.accountHandler.send(msg);
                Program.gameHandler.prepareServerChange(ptr);
            }
        }

        /**
		 * Enqueues an event. It will be executed at end of update.
		 */
        static void enqueueEvent(Actor ptr, DelayedEvent e)
        {
            delayedEvents.Add(ptr, e);

            //TODO Implementieren und in Originalimplementation schauen
//            // Delete events take precedence over other events.
//            if(e!=null&&e.type==Event.EVENT_REMOVE)
//            {
//                p.first.second.type=Event.EVENT_REMOVE;
//            }
        }

        static void enqueueInsert(Actor ptr)
        {
            //DelayedEvent e = { EVENT_INSERT, 0, 0, 0 };
            //enqueueEvent(ptr, e);
        }

        public static void enqueueRemove(Actor ptr)
        {
            //DelayedEvent e = { EVENT_REMOVE, 0, 0, 0 };
            //enqueueEvent(ptr, e);
        }

        public static void enqueueWarp(Character ptr, MapComposite m, int x, int y)
        {
            //TODO Implementieren
//            DelayedEvent e=new DelayedEvent(Event.EVENT_WARP, x, y, m);
//            enqueueEvent(ptr, e);
        }

        static void sayAround(Actor obj, string text)
        {
            //Point speakerPosition = obj.getPosition();
            //int visualRange = Configuration::getValue("game_visualRange", 448);

            //for (CharacterIterator i(obj.getMap().getAroundActorIterator(obj, visualRange)); i; ++i)
            //{
            //    if (speakerPosition.inRangeOf((*i).getPosition(), visualRange))
            //    {
            //        sayTo(*i, obj, text);
            //    }
            //}
        }

        static void sayTo(Actor destination, Actor source, string text)
        {
            //if (destination.getType() != OBJECT_CHARACTER)
            //    return; //only characters will read it anyway

            //MessageOut msg(GPMSG_SAY);
            //if (source == NULL)
            //{
            //    msg.writeInt16(0);
            //}
            //else if (!source.canMove())
            //{
            //    msg.writeInt16(65535);
            //}
            //else
            //{
            //    msg.writeInt16(static_cast< Actor * >(source).getPublicID());
            //}
            //msg.writeString(text);

            //gameHandler.sendTo(static_cast< Character * >(destination), msg);
        }

        static void sayToAll(string text)
        {
            //MessageOut msg(GPMSG_SAY);

            //// The message will be shown as if it was from the server
            //msg.writeInt16(0);
            //msg.writeString(text);

            //// Sends it to everyone connected to the game server
            //gameHandler.sendToEveryone(msg);
        }

        static string getVariable(string key)
        {
            //std::map<std::string, std::string>::iterator iValue =
            //                                                 mScriptVariables.find(key);
            //if (iValue != mScriptVariables.end())
            //    return iValue.second;
            //else
            //    return std::string();

            return ""; //ssk
        }

        static void setVariable(string key, string value)
        {
            //mScriptVariables[key] = value;
            //accountHandler.updateWorldVar(key, value);
        }

        public static void setVariableFromDbserver(string key, string value)
        {
            //mScriptVariables[key] = value ;
        }
    }
}
