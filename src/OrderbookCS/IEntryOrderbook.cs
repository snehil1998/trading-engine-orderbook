using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IEntryOrderbook: IReadOnlyOrderbook
{
    void AddOrder(Order order);
    void RemoveOrder(CancelOrder cancelOrder);
}
