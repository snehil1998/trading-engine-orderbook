using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IMatchingOrderbook
{
    MatchResult Match(Order order);
}
