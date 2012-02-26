using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/**
 * Identifies a modifier by the attribute id that it applies to and its layer
 * index in the stack of modifiers for that attribute.
 */
	public class ModifierLocation
	{
		int attributeId;
		int layer;

		ModifierLocation(int attributeId, int layer)
		{
			//: attributeId(attributeId)
			//   , layer(layer)
		}

		//bool operator==(const ModifierLocation &other) const
		//{ return attributeId == other.attributeId && layer == other.layer; }
	}
}
