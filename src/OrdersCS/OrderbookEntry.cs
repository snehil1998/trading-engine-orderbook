using System;
using System.Runtime.InteropServices;

namespace TradingEngineServer.Orders;

public class OrderbookEntry
{
    public OrderbookEntry(Order currentOrder)
    {
        CreationTime = DateTime.UtcNow;
        Order = currentOrder;
    }

    public Order Order { get; init; }
    public OrderbookEntry? Next { get; set; }
    public OrderbookEntry? Previous { get; set; }
    public DateTime CreationTime { get;  init; }

}
