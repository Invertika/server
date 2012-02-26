using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISL.Server.Common;

namespace invertika_game.Common
{
	public class AttributeInfoType
	{
		StackableType stackableType;
		ModifierEffectType effectType;

		AttributeInfoType(StackableType s, ModifierEffectType effect)
		{
			stackableType=s;
			effectType=effect;
		}
	}
}
