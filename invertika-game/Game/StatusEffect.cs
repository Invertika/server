using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	public class StatusEffect
	{
		StatusEffect(int id)
		{
			//            mId(id),
			//mScript(0)
		}

		~StatusEffect()
		{
			//delete mScript;
		}

		void tick(Being target, int count)
		{
			//if (mScript)
			//{
			//    mScript->setMap(target->getMap());
			//    mScript->prepare("tick");
			//    mScript->push(target);
			//    mScript->push(count);
			//    mScript->execute();
			//}
		}
	}
}
