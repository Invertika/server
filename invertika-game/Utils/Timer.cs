//
//  Timer.cs
//
//  This file is part of Invertika (http://invertika.org)
// 
//  Based on The Mana Server (http://manasource.org)
//  Copyright (C) 2004-2012  The Mana World Development Team 
//
//  Author:
//       seeseekey <seeseekey@googlemail.com>
// 
//  Copyright (c) 2011, 2012 by Invertika Development Team
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace invertika_game.Utils
{
	public class Timer
	{
		/**
 * Interval between two pulses.
 */
		uint interval;

		/**
		 * The time the last pulse occured.
		 */
		UInt64 lastpulse;

		/**
		 * Activity status of the timer.
		 */
		bool active;

		public Timer(uint ms, bool createActive)
		{
			active=createActive;
			interval=ms;
			lastpulse=getTimeInMillisec();
		}

		public void sleep()
		{
			if(!active) return;
			UInt64 now=getTimeInMillisec();
			if(now-lastpulse>=interval) return;

			Thread.Sleep((int)(interval-(now-lastpulse)));
		}

		public int poll()
		{
			int elapsed=0;
			if(active)
			{
				UInt64 now=getTimeInMillisec();
				if(now>lastpulse)
				{
					elapsed=(int)((now-lastpulse)/interval);
					lastpulse+=(ulong)(interval*elapsed);
				}
				else
				{
					// Time has made a jump to the past. This should be a rare
					// occurence, so just reset lastpulse to prevent problems.
					lastpulse=now;
				}
			};
			return elapsed;
		}

		public void start()
		{
			active=true;
			lastpulse=getTimeInMillisec();
		}

		void stop()
		{
			active=false;
		}

		void changeInterval(uint newinterval)
		{
			interval=newinterval;
		}

		UInt64 getTimeInMillisec()
		{
			DateTime timeBase=new DateTime(1970, 1, 1);
			TimeSpan timeDiff=DateTime.Now.ToUniversalTime()-timeBase;
			return ((UInt64)timeDiff.TotalMilliseconds);
		}
	}
}
