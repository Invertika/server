using System;
using ISL.Server.Common;
using invertika_game.Common;

namespace invertika_game
{
    public class AttributeModifiersEffect
    {
        public AttributeModifiersEffect(StackableType stackableType, ModifierEffectType effectType) 
//            mCacheVal(0),
//            mMod(effectType == Multiplicative ? 1 : 0),
//            mStackableType(stackableType),
//            mEffectType(effectType)
        {
//            assert(effectType == Multiplicative
//                   || effectType == Additive);
//            assert(stackableType == Stackable
//                   || stackableType == NonStackable
//                   || stackableType == NonStackableBonus);
//            
//            LOG_DEBUG("Layer created with effectType " << effectType
//                      << " and stackableType " << stackableType << ".");
        }
        
        ~AttributeModifiersEffect()
        {
            // // ?
            // /*mStates.clear();*/
            // LOG_WARN("DELETION of attribute effect!");
        }
        
        bool add(ushort duration, double value, double prevLayerValue, int level)
        {
//            LOG_DEBUG("Adding modifier with value " << value <<
//                      " with a previous layer value of " << prevLayerValue << ". "
//                      "Current mod at this layer: " << mMod << ".");
//            bool ret = false;
//            mStates.push_back(new AttributeModifierState(duration, value, level));
//            switch (mStackableType) {
//                case Stackable:
//                    switch (mEffectType) {
//                        case Additive:
//                            if (value)
//                            {
//                                ret = true;
//                                mMod += value;
//                                mCacheVal = prevLayerValue + mMod;
//                            }
//                            break;
//                        case Multiplicative:
//                            if (value != 1)
//                            {
//                                ret = true;
//                                mMod *= value;
//                                mCacheVal = prevLayerValue * mMod;
//                            }
//                            break;
//                        default:
//                            LOG_FATAL("Attribute modifiers effect: unhandled type '"
//                                      << mEffectType << "' as a stackable!");
//                            assert(0);
//                            break;
//                    }
//                    break;
//                case NonStackable:
//                    switch (mEffectType) {
//                        case Additive:
//                            if (value > mMod)
//                            {
//                                ret = true;
//                                mMod = value;
//                                if (mMod > prevLayerValue)
//                                    mCacheVal = mMod;
//                            }
//                            break;
//                        default:
//                            LOG_FATAL("Attribute modifiers effect: unhandled type '"
//                                      << mEffectType << "' as a non-stackable!");
//                            assert(0);
//                    }
//                    // A multiplicative type would also be nonsensical
//                    break;
//                case NonStackableBonus:
//                    switch (mEffectType) {
//                        case Additive:
//                        case Multiplicative:
//                            if (value > mMod)
//                            {
//                                ret = true;
//                                mMod = value;
//                                mCacheVal = mEffectType == Additive ? prevLayerValue + mMod
//                                    : prevLayerValue * mMod;
//                            }
//                            break;
//                        default:
//                            LOG_FATAL("Attribute modifiers effect: unhandled type '"
//                                      << mEffectType << "' as a non-stackable bonus!");
//                            assert(0);
//                    }
//                    break;
//                default:
//                    LOG_FATAL("Attribute modifiers effect: unknown stackable type '"
//                              << mStackableType << "'!");
//                    assert(0);
//            }
//            return ret;

            return false; //ssk
        }

        bool remove(double value, uint id, bool fullCheck)
        {
//            /* We need to find and check this entry exists, and erase the entry
//       from the list too. */
//            if (!fullCheck)
//                mStates.sort(durationCompare); /* Search only through those with a duration of 0. */
//            bool ret = false;
//            
//            for (std::list< AttributeModifierState * >::iterator it = mStates.begin();
//                 it != mStates.end() && (fullCheck || !(*it)->mDuration);)
//            {
//                /* Check for a match */
//                if ((*it)->mValue != value || (*it)->mId != id)
//                {
//                    ++it;
//                    continue;
//                }
//                
//                delete *it;
//                mStates.erase(it++);
//                
//                /* If this is stackable, we need to update for every modifier affected */
//                if (mStackableType == Stackable)
//                    updateMod();
//                
//                ret = true;
//                if (!id)
//                    break;
//            }
//            /*
//     * Non stackables only need to be updated once, since this is recomputed
//     * from scratch. This is done at the end after modifications have been
//     * made as necessary.
//     */
//            if (ret && mStackableType != Stackable)
//                updateMod();
//            return ret;

            return false; //ssk
        }
        
        void updateMod(double value)
        {
//            if (mStackableType == Stackable)
//            {
//                if (mEffectType == Additive)
//                {
//                    mMod -= value;
//                }
//                else if (mEffectType == Multiplicative)
//                {
//                    if (value)
//                        mMod /= value;
//                    else
//                    {
//                        mMod = 1;
//                        for (std::list< AttributeModifierState * >::const_iterator
//                             it = mStates.begin(),
//                             it_end = mStates.end();
//                             it != it_end;
//                             ++it)
//                            mMod *= (*it)->mValue;
//                    }
//                }
//                else LOG_ERROR("Attribute modifiers effect: unhandled type '"
//                               << mEffectType << "' as a stackable in cache update!");
//            }
//            else if (mStackableType == NonStackable || mStackableType == NonStackableBonus)
//            {
//                if (mMod == value)
//                {
//                    mMod = 0;
//                    for (std::list< AttributeModifierState * >::const_iterator
//                         it = mStates.begin(),
//                         it_end = mStates.end();
//                         it != it_end;
//                         ++it)
//                        if ((*it)->mValue > mMod)
//                            mMod = (*it)->mValue;
//                }
//            }
//            else
//            {
//                LOG_ERROR("Attribute modifiers effect: unknown stackable type '"
//                          << mStackableType << "' in cache update!");
//            }
        }
        
        bool recalculateModifiedValue(double newPrevLayerValue)
        {
//            double oldValue = mCacheVal;
//            switch (mEffectType) {
//                case Additive:
//                    switch (mStackableType) {
//                        case Stackable:
//                        case NonStackableBonus:
//                            mCacheVal = newPrevLayerValue + mMod;
//                            break;
//                        case NonStackable:
//                            mCacheVal = newPrevLayerValue < mMod ? mMod : newPrevLayerValue;
//                            break;
//                        default:
//                            LOG_FATAL("Unknown effect type '" << mEffectType << "'!");
//                            assert(0);
//                    } break;
//                case Multiplicative:
//                    mCacheVal = mStackableType == Stackable ? newPrevLayerValue * mMod : newPrevLayerValue * mMod;
//                    break;
//                default:
//                    LOG_FATAL("Unknown effect type '" << mEffectType << "'!");
//                    assert(0);
//            }
//            return oldValue != mCacheVal;

            return false; //ssk
        }

        
        bool tick()
        {
//            bool ret = false;
//            std::list<AttributeModifierState *>::iterator it = mStates.begin();
//            while (it != mStates.end())
//            {
//                if ((*it)->tick())
//                {
//                    double value = (*it)->mValue;
//                    LOG_DEBUG("Modifier of value " << value << " expiring!");
//                    delete *it;
//                    mStates.erase(it++);
//                    updateMod(value);
//                    ret = true;
//                }
//                ++it;
//            }
//            return ret;

            return false;//ssk
        }
        
        void clearMods(double baseValue)
        {
//            mStates.clear();
//            mCacheVal = baseValue;
//            mMod = mEffectType == Additive ? 0 : 1;
        }
    }
}

