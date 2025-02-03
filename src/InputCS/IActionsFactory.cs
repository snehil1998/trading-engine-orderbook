using System;
using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Input;

public interface IActionsFactory
{
    void AddOrder(IMatchingOrderbook fifo, CancellationToken token);
}
