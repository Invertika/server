using System;
using invertika_game.Game;

namespace invertika_game
{
    public abstract class TriggerAction
    {
        public abstract void process(Actor obj);

        public TriggerAction()
        {
        }
    }
}

