using System;
using invertika_game.Game;
using invertika_game.Scripting;
using ISL.Server.Common;

namespace invertika_game.game
{
    public class NPC: Being
    {
        public NPC(string name, int id, Script s):base(ThingType.OBJECT_NPC)
//            mScript(s),
//            mID(id),
//            mEnabled(true)
        {
            //setName(name);
        }

        /**
         * Gets the way an NPC is blocked by other things on the map
         */
        public virtual byte getWalkMask()
        {
            return 0x83;
        } // blocked like a monster by walls, monsters and characters ( bin 1000 0011)
    }
}

