using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invertika_game.Game
{
	/*
 * See trade.cpp for doc on TradeStates
 */
	public enum TradeState
	{
		TRADE_INIT=0,
		TRADE_RUN,
		TRADE_CONFIRM_WAIT,
		TRADE_CONFIRMED,
		TRADE_AGREE_WAIT
	}
}
