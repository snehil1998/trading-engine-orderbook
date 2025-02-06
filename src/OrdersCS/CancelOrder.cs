using System;

namespace TradingEngineServer.Orders;

public sealed class CancelOrder : IOrderCore
{
    public CancelOrder(IOrderCore orderCore)
    {
        _orderCore = orderCore;
    }
    public string OrderId => _orderCore.OrderId;
    public string Username => _orderCore.Username;

    private readonly IOrderCore _orderCore;
}
