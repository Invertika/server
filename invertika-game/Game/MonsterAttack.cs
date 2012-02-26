using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using invertika_game.Common;

namespace invertika_game.Game
{
	/**
 * Structure containing different attack types of a monster type
 */
	public class MonsterAttack
	{
		uint id;
		int priority;
		float damageFactor;
		int element;
		DamageType type;
		int preDelay;
		int aftDelay;
		int range;
		string scriptFunction;
	}
}
