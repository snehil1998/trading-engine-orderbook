using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IModifyOrderbook
{
    void ModifyOrder(ModifyOrder modifyOrder);
}
