using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Common
{
	/**
 * Damage type, used to know how to compute them.
 */
	public enum DamageType
	{
		DAMAGE_PHYSICAL=0,
		DAMAGE_MAGICAL,
		DAMAGE_DIRECT,
		DAMAGE_OTHER=-1
	}
}
