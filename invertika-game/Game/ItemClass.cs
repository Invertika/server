using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class ItemClass
	{
		//    public:
		//    ItemClass(int id, unsigned int maxperslot):
		//        mDatabaseID(id),
		//        mName("unnamed"),
		//        mSpriteID(0),
		//        mCost(0),
		//        mMaxPerSlot(maxperslot)
		//    {}

		//    ~ItemClass()
		//    { resetEffects(); }

		//    /**
		//     * Returns the name of the item type
		//     */
		//    const std::string &getName() const
		//    { return mName; }

		//    /**
		//     * Sets the name of the item type
		//     */
		//    void setName(const std::string &name)
		//    { mName = name; }

		//    /**
		//     * Applies the modifiers of an item to a given user.
		//     * @return true if item should be removed.
		//     */
		//    bool useTrigger(Being *itemUser, ItemTriggerType trigger);

		//    /**
		//     * Gets unit cost of these items.
		//     */
		//    int getCost() const
		//    { return mCost; }

		//    /**
		//     * Gets max item per slot.
		//     */
		//    unsigned int getMaxPerSlot() const
		//    { return mMaxPerSlot; }

		//    bool hasTrigger(ItemTriggerType id)
		//    { return mEffects.count(id); }

		//    /**
		//     * Gets database ID.
		//     */
		//    int getDatabaseID() const
		//    { return mDatabaseID; }

		//    /**
		//     * Gets the sprite ID.
		//     * @note At present this is only a stub, and will always return zero.
		//     *       When you would want to extend serializeLooks to be more
		//     *       efficient, keep track of a sprite id here.
		//     */
		//    int getSpriteID() const
		//    { return mSpriteID; }

		//    /**
		//     * Returns equip requirement.
		//     */
		//    const ItemEquipRequirement &getItemEquipRequirement() const
		//    { return mEquipReq; }

		//private:
		//    /**
		//     * Add an effect to a trigger
		//     * @param effect  The effect to be run when the trigger is hit.
		//     * @param id      The trigger type.
		//     * @param dispell The trigger that the effect should be dispelled on.
		//     * @note  FIXME:  Should be more than one trigger that an effect
		//     *                can be dispelled from.
		//     */
		//    void addEffect(ItemEffectInfo *effect,
		//                   ItemTriggerType id,
		//                   ItemTriggerType dispell = ITT_NULL)
		//    {
		//        mEffects.insert(std::make_pair(id, effect));
		//        if (dispell)
		//            mDispells.insert(std::make_pair(dispell, effect));
		//    }

		//    void resetEffects()
		//    {
		//        while (mEffects.begin() != mEffects.end())
		//        {
		//            delete mEffects.begin()->second;
		//            mEffects.erase(mEffects.begin());
		//        }
		//        while (mDispells.begin() != mDispells.end())
		//        {
		//            delete mDispells.begin()->second;
		//            mDispells.erase(mDispells.begin());
		//        }
		//    }

		//    unsigned short mDatabaseID; /**< Item reference information */
		//    std::string mName; /**< name used to identify the item class */
		//    /** The sprite that should be shown to the character */
		//    unsigned short mSpriteID;
		//    unsigned short mCost;     /**< Unit cost the item. */
		//    /** Max item amount per slot in inventory. */
		//    unsigned int mMaxPerSlot;

		//    std::multimap< ItemTriggerType, ItemEffectInfo * > mEffects;
		//    std::multimap< ItemTriggerType, ItemEffectInfo * > mDispells;

		//    /**
		//     * Requirement for equipping.
		//     */
		//    ItemEquipRequirement mEquipReq;

		//    friend class ItemManager;
	}
}