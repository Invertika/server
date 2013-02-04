using System;
using invertika_game.Game;
using ISL.Server.Utilities;
using System.Collections.Generic;
using ISL.Server.Common;

namespace invertika_game
{
    public class TriggerArea: Thing
    {
        Rectangle mZone;
        TriggerAction mAction;
        bool mOnce;
        List<Actor> mInside;

        /**
                 * Creates a rectangular trigger for a given map.
                 */
        public TriggerArea(MapComposite m, Rectangle r, TriggerAction ptr, bool once): base(ThingType.OBJECT_OTHER, m)
        {
            mZone=r;
            mAction=ptr;
            mOnce=once;
        }

        public void update()
        {
            //TODO Implementieren
//            std::set<Actor*> insideNow;
//            for (BeingIterator i(getMap()->getInsideRectangleIterator(mZone)); i; ++i)
//            {
//                // Don't deal with unitialized actors.
//                if (!(*i) || !(*i)->isPublicIdValid())
//                    continue;
//                
//                // The BeingIterator returns the mapZones in touch with the rectangle
//                // area. On the other hand, the beings contained in the map zones
//                // may not be within the rectangle area. Hence, this additional
//                // contains() condition.
//                if (mZone.contains((*i)->getPosition()))
//                {
//                    insideNow.insert(*i);
//                    
//                    if (!mOnce || mInside.find(*i) == mInside.end())
//                    {
//                        mAction->process(*i);
//                    }
//                }
//            }
//
//            mInside.swap(insideNow); //swapping is faster than assigning
        }
    }
}

