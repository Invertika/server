using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Common
{
	/**
 * Beings and actors directions
 * WARNING: Has to be in sync with the same enum in the Being class
 * of the client!
 */
	public enum BeingDirection
	{
		DOWN=1,
		LEFT=2,
		UP=4,
		RIGHT=8
	}
}
