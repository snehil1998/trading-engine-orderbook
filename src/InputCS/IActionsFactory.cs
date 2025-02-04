using System;
using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Input;

public interface IActionsFactory
{
    void AddOrder(string username, CancellationToken token);
    void CancelOrder(string username, CancellationToken token);
    void ModfifyOrder(string username, CancellationToken token);
    void PrintOrderbook();
    void PrintOrders();
}
