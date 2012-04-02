using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_account.Common
{
	/**
	 * Enumeration type for account levels.
	 * A normal player would have permissions of 1
	 * A tester would have permissions of 3 (AL_PLAYER | AL_TESTER)
	 * A dev would have permissions of 7 (AL_PLAYER | AL_TESTER | AL_DEV)
	 * A gm would have permissions of 11 (AL_PLAYER | AL_TESTER | AL_GM)
	 * A admin would have permissions of 255 (*)
	 */
	public enum AccessLevel
	{
		AL_BANNED=0,     /**< This user is currently banned. */
		AL_PLAYER=1,     /**< User has regular rights. */
		AL_TESTER=2,     /**< User can perform testing tasks. */
		AL_DEV=4,     /**< User is a developer and can perform dev tasks */
		AL_GM=8,     /**< User is a moderator and can perform mod tasks */
		AL_ADMIN=128     /**< User can perform administrator tasks. */
	}
}
