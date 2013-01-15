using System;

namespace invertika_game
{
    /**
 * Flags that are raised as necessary. They trigger messages that are sent to
 * the clients.
 */
    public enum UpdateFlag
    {
        UPDATEFLAG_NEW_ON_MAP = 1,
        UPDATEFLAG_NEW_DESTINATION = 2,
        UPDATEFLAG_ATTACK = 4,
        UPDATEFLAG_ACTIONCHANGE = 8,
        UPDATEFLAG_LOOKSCHANGE = 16,
        UPDATEFLAG_DIRCHANGE = 32,
        UPDATEFLAG_HEALTHCHANGE = 64
    }
}

