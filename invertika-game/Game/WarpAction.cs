using System;
using invertika_game.Game;
using ISL.Server.Common;

namespace invertika_game
{
    public class WarpAction: TriggerAction
    {
        MapComposite mMap;
        ushort mX, mY;

        public WarpAction(MapComposite m, int x, int y)
        {
            mMap=m;
            mX=(ushort)x;
            mY=(ushort)y;
        }

        public override void process(Actor obj)
        {
            if(obj.getType()==ThingType.OBJECT_CHARACTER)
            {
                GameState.enqueueWarp((Character)(obj), mMap, mX, mY);
            }
        }
    }
}

