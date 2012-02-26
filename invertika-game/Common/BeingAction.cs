using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Common
{
	/**
  * Moves enum for beings and actors for others players vision.
  * WARNING: Has to be in sync with the same enum in the Being class
  * of the client!
  */
	public enum BeingAction
	{
		STAND,
		WALK,
		ATTACK,
		SIT,
		DEAD,
		HURT
	}
}
