using System;
using ISL.Server.Common;

namespace invertika_game.Game
{
    public class Monster: Being
    {
        public Monster(MonsterClass specy):base(ThingType.OBJECT_MONSTER)
//            Being(OBJECT_MONSTER),
//            mSpecy(specy),
//            mScript(NULL),
//            mTargetListener(&monsterTargetEventDispatch),
//            mOwner(NULL),
//            mCurrentAttack(NULL)
        {
//            LOG_DEBUG("Monster spawned! (id: " << mSpecy->getId() << ").");
//            
//            /*
//     * Initialise the attribute structures.
//     */
//            const AttributeScope &mobAttr = attributeManager->getAttributeScope(
//                MonsterScope);
//            
//            for (AttributeScope::const_iterator it = mobAttr.begin(),
//                 it_end = mobAttr.end(); it != it_end; ++it)
//            {
//                mAttributes.insert(std::pair< unsigned int, Attribute >
//                                   (it->first, Attribute(*it->second)));
//            }
//            
//            /*
//     * Set the attributes to the values defined by the associated monster
//     * class with or without mutations as needed.
//     */
//            
//            int mutation = specy->getMutation();
//            
//            for (AttributeMap::iterator it2 = mAttributes.begin(),
//                 it2_end = mAttributes.end(); it2 != it2_end; ++it2)
//            {
//                double attr = 0.0f;
//                
//                if (specy->hasAttribute(it2->first))
//                {
//                    attr = specy->getAttribute(it2->first);
//                    
//                    setAttribute(it2->first,
//                                 mutation ?
//                                 attr * (100 + (rand()%(mutation << 1)) - mutation) / 100.0 :
//                                 attr);
//                }
//            }
//            
//            setSize(specy->getSize());
//            setGender(specy->getGender());
//            
//            // Set positions relative to target from which the monster can attack
//            int dist = specy->getAttackDistance();
//            mAttackPositions.push_back(AttackPosition(dist, 0, LEFT));
//            mAttackPositions.push_back(AttackPosition(-dist, 0, RIGHT));
//            mAttackPositions.push_back(AttackPosition(0, -dist, DOWN));
//            mAttackPositions.push_back(AttackPosition(0, dist, UP));
//            
//            // Load default script
//            loadScript(specy->getScript());
        }


        /**
         * Returns the way the actor is blocked by other things on the map.
         */
        public virtual byte getWalkMask()
        {
            // blocked walls, other monsters and players ( bin 1000 0011)
            return 0x83;
        }
    }
}

