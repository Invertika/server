using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
 * Event expected to happen at next update.
 */
	public class DelayedEvent
	{
		ushort type, x, y;
		MapComposite map;
	}
}
