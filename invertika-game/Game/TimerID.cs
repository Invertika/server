using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public enum TimerID
	{
		T_M_STROLL, // time until monster strolls to new location
		T_M_KILLSTEAL_PROTECTED,  // killsteal protection time
		T_M_DECAY,  // time until dead monster is removed
		T_M_ATTACK_TIME,    // time until monster can attack again
		T_B_HP_REGEN,    // time until hp is regenerated again
		T_C_MUTE // time until the character can chat again
	}
}
