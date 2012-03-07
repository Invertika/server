using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class BuySell
	{
		BuySell(Character c, bool sell)
		//mCurrencyId(ATTR_GP), mChar(c), mSell(sell)
		{
			//c->setBuySell(this);
		}

		~BuySell()
		{
			//mChar->setBuySell(NULL);
		}

		void cancel()
		{
			//delete this;
		}

		bool registerItem(int id, int amount, int cost)
		{
			//if (mSell)
			//{
			//    int nb = Inventory(mChar).count(id);
			//    if (nb == 0)
			//        return false;
			//    if (!amount || nb < amount)
			//        amount = nb;
			//}

			//TradedItem it = { id, amount, cost };
			//mItems.push_back(it);
			return true;
		}


		int registerPlayerItems()
		{
			//if (!mSell)
			//    return 0;

			//int nbItemsToSell = 0;

			//// We parse the player inventory and add all item
			//// in a sell list.
			//const InventoryData &inventoryData = mChar->getPossessions().getInventory();
			//for (InventoryData::const_iterator it = inventoryData.begin(),
			//    it_end = inventoryData.end(); it != it_end; ++it)
			//{
			//    unsigned int nb = it->second.amount;
			//    if (!nb)
			//        continue;

			//    int id = it->second.itemId;
			//    int cost = -1;
			//    if (itemManager->getItem(id))
			//    {
			//        cost = itemManager->getItem(id)->getCost();
			//    }
			//    else
			//    {
			//        LOG_WARN("registerPlayersItems(): The character Id: "
			//            << mChar->getPublicID() << " has unknown items (Id: " << id
			//            << "). They have been ignored.");
			//        continue;
			//    }

			//    if (cost < 1)
			//        continue;

			//    // We check if the item Id has been already
			//    // added. If so, we cumulate the amounts.
			//    bool itemAlreadyAdded = false;
			//    for (TradedItems::iterator i = mItems.begin(),
			//        i_end = mItems.end(); i != i_end; ++i)
			//    {
			//        if (i->itemId == id)
			//        {
			//            itemAlreadyAdded = true;
			//            i->amount += nb;
			//            break;
			//        }
			//    }

			//    if (!itemAlreadyAdded)
			//    {
			//        TradedItem itTrade = { id, nb, cost };
			//        mItems.push_back(itTrade);
			//        nbItemsToSell++;
			//    }
			//}
			//return nbItemsToSell;

			return 0; //ssk
		}

		bool start(Actor actor)
		{
			//if (mItems.empty())
			//{
			//    cancel();
			//    return false;
			//}

			//MessageOut msg(mSell ? GPMSG_NPC_SELL : GPMSG_NPC_BUY);
			//msg.writeInt16(actor->getPublicID());
			//for (TradedItems::const_iterator i = mItems.begin(),
			//     i_end = mItems.end(); i != i_end; ++i)
			//{
			//    msg.writeInt16(i->itemId);
			//    msg.writeInt16(i->amount);
			//    msg.writeInt16(i->cost);
			//}
			//mChar->getClient()->send(msg);
			return true;
		}

		void perform(int id, int amount)
		{
			//Inventory inv(mChar);
			//for (TradedItems::iterator i = mItems.begin(),
			//     i_end = mItems.end(); i != i_end; ++i)
			//{
			//    if (i->itemId != id) continue;
			//    if (i->amount && i->amount <= amount) amount = i->amount;
			//    if (mSell)
			//    {
			//        amount -= inv.remove(id, amount);
			//        mChar->setAttribute(mCurrencyId,
			//                            mChar->getAttribute(mCurrencyId) +
			//                            amount * i->cost);
			//    }
			//    else
			//    {
			//        amount = std::min(amount, ((int) mChar->getAttribute(mCurrencyId)) / i->cost);
			//        amount -= inv.insert(id, amount);
			//        mChar->setAttribute(mCurrencyId,
			//                            mChar->getAttribute(mCurrencyId) -
			//                            amount * i->cost);
			//    }
			//    if (i->amount)
			//    {
			//        i->amount -= amount;
			//        if (!i->amount)
			//        {
			//            mItems.erase(i);
			//        }
			//    }
			return;
		}
	}
}
