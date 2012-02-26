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

		Timer(uint ms, bool createActive)
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
