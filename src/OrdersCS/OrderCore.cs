using System;

namespace TradingEngineServer.Orders;

public sealed class OrderCore: IOrderCore
{
    public OrderCore(long orderId, string username)
    {
        OrderId = orderId;
        Username = username;
    }
    public long OrderId { get; init; }
    public string Username { get; init; }
}
