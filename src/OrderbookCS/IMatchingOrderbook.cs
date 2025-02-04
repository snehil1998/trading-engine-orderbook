using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IMatchingOrderbook
{
    void ModifyOrder(ModifyOrder modifyOrder);
    MatchResult Match(Order order);
}
