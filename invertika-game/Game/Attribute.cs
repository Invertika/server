using System;
using System.Collections.Generic;
using invertika_game.Common;

namespace invertika_game
{
    public class Attribute
    {
        double mBase;
        List<AttributeModifiersEffect> mMods;

        public double getBase()
        {
            return mBase;
        }

        public double getModifiedAttribute()
        { 
//            return mMods.empty() ? mBase :
//                (*mMods.rbegin())->getCachedModifiedValue();

            return 0; //ssk
        }

        bool add(ushort duration, double value, uint layer, int level)
        {
//            assert(mMods.size() > layer);
//            LOG_DEBUG("Adding modifier to attribute with duration " << duration <<
//                      ", value " << value << ", at layer " << layer << " with id "
//                      << level);
//            if (mMods.at(layer)->add(duration, value,
//                                     (layer ? mMods.at(layer - 1)->getCachedModifiedValue()
//             : mBase)
//                                     , level))
//            {
//                while (++layer < mMods.size())
//                {
//                    if (!mMods.at(layer)->recalculateModifiedValue(
//                        mMods.at(layer - 1)->getCachedModifiedValue()))
//                    {
//                        LOG_DEBUG("Modifier added, but modified value not changed.");
//                        return false;
//                    }
//                }
//                LOG_DEBUG("Modifier added. Base value: " << mBase << ", new modified "
//                          "value: " << getModifiedAttribute() << ".");
//                return true;
//            }
//            LOG_DEBUG("Failed to add modifier!");
            return false;
        }
        
        bool remove(double value, uint layer, int lvl, bool fullcheck)
        {
//            assert(mMods.size() > layer);
//            if (mMods.at(layer)->remove(value, lvl, fullcheck))
//            {
//                while (++layer < mMods.size())
//                    if (!mMods.at(layer)->recalculateModifiedValue(
//                        mMods.at(layer - 1)->getCachedModifiedValue()))
//                        return false;
//                return true;
//            }
            return false;
        }

        public Attribute(List<AttributeInfoType> type)
        {
//            LOG_DEBUG("Construction of new attribute with '" << type.size() << "' layers.");
//            for (unsigned int i = 0; i < type.size(); ++i)
//            {
//                LOG_DEBUG("Adding layer with stackable type " << type[i].stackableType
//                          << " and effect type " << type[i].effectType << ".");
//                mMods.push_back(new AttributeModifiersEffect(type[i].stackableType,
//                                                             type[i].effectType));
//                LOG_DEBUG("Layer added.");
//            }
        }
        
        ~Attribute()
        {
//            for (std::vector<AttributeModifiersEffect *>::iterator it = mMods.begin(),
//                 it_end = mMods.end(); it != it_end; ++it)
//            {
//                // ?
//                //delete *it;
//            }
        }
        
        bool tick()
        {
//            bool ret = false;
//            double prev = mBase;
//            for (std::vector<AttributeModifiersEffect *>::iterator it = mMods.begin(),
//                 it_end = mMods.end(); it != it_end; ++it)
//            {
//                if ((*it)->tick())
//                {
//                    LOG_DEBUG("Attribute layer " << mMods.begin() - it
//                              << " has expiring modifiers.");
//                    ret = true;
//                }
//                if (ret)
//                    if (!(*it)->recalculateModifiedValue(prev)) ret = false;
//                prev = (*it)->getCachedModifiedValue();
//            }
//            return ret;

            return false; //ssk
        }
        
        void clearMods()
        {
//            for (std::vector<AttributeModifiersEffect *>::iterator it = mMods.begin(),
//                 it_end = mMods.end(); it != it_end; ++it)
//                (*it)->clearMods(mBase);
        }
        
        void setBase(double @base)
        {
//            LOG_DEBUG("Setting base attribute from " << mBase << " to " << base << ".");
//            double prev = mBase = base;
//            std::vector<AttributeModifiersEffect *>::iterator it = mMods.begin();
//            while (it != mMods.end())
//            {
//                if ((*it)->recalculateModifiedValue(prev))
//                    prev = (*it++)->getCachedModifiedValue();
//                else
//                    break;
//            }
        }
    }
}

