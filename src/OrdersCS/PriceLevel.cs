using System;

namespace TradingEngineServer.Orders;

// this class stores the orderbook entries at a price level
public class PriceLevel
{
    public PriceLevel(long price, OrderbookEntry orderbookEntry)
    {
        Price = price;
        Head = orderbookEntry;
        Tail = orderbookEntry;
    }

    public long Price { get; init; }
    public OrderbookEntry? Head { get; set; }
    public OrderbookEntry? Tail { get; set; }

    public uint GetLevelOrderNumber()
    {
        var head = Head;
        uint orders = 0;
        while (head != null)
        {
            if (head.Order.CurrentQuantity != 0)
            {
                orders++;
            }
            head = head.Next;
        }
        return orders;
    }

    public uint GetLevelOrderQuantity()
    {
        var head = Head;
        uint quantity = 0;
        while (head != null)
        {
            quantity += head.Order.CurrentQuantity;
            head = head.Next;
        }
        return quantity;
    }

    public IReadOnlyCollection<Order> GetLevelOrders()
    {
        var head = Head;
        var orders = new List<Order>();
        while(head != null)
        {
            if(head.Order.CurrentQuantity != 0)
            {
                orders.Add(head.Order);
            }
            head = head.Next;
        }
        return orders.AsReadOnly();
    }

    public bool IsEmpty {
        get 
        {
            return Head == null && Tail == null;
        }
    }

    public Side Side => Head == null ? Side.Unknown : Head.Order.IsBuySide ? Side.Bid : Side.Ask;
}
