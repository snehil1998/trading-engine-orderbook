using System;

namespace TradingEngineServer.Orders;

public sealed class OrderCore: IOrderCore
{
    public OrderCore(string orderId, string username)
    {
        OrderId = orderId;
        Username = username;
    }
    public string OrderId { get; init; }
    public string Username { get; init; }
}
