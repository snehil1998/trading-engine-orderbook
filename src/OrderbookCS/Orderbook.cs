using TradingEngineServer.Logging;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public class Orderbook : IRetrievalOrderbook
{
    public Orderbook(ITextLogger logger)
    {
        _logger = logger;
    }

    public int Count => _orderbook.Count;

    public void AddOrder(Order order)
    {
        AddOrder(order, order.Price, order.IsBuySide ? _bidPriceLevel : _askPriceLevel, _orderbook);
    }

    private static void AddOrder(Order order, long orderPrice, SortedSet<PriceLevel> sidePriceLevels, IDictionary<long, OrderbookEntry> orderbook)
    {
        var existingPriceLevel = sidePriceLevels.FirstOrDefault(x => x.Price == orderPrice);
        if (existingPriceLevel is not null)
        {
            var orderbookEntry = new OrderbookEntry(order);
            var tailOrderbookEntry = existingPriceLevel.Tail;
            tailOrderbookEntry.Next = orderbookEntry;
            orderbookEntry.Previous = tailOrderbookEntry;
            existingPriceLevel.Tail = orderbookEntry; 
            orderbook.Add(order.OrderId, orderbookEntry);
        }
        else
        {
            var orderbookEntry = new OrderbookEntry(order);
            var newOrderPriceLevel = new PriceLevel(orderPrice, orderbookEntry);
            sidePriceLevels.Add(newOrderPriceLevel);
            orderbook.Add(order.OrderId, orderbookEntry);
        }
    }

    public bool ContainsOrder(long OrderId)
    {
        return _orderbook.ContainsKey(OrderId);
    }

    public IReadOnlyCollection<OrderbookEntry> GetAskEntries()
    {
        var orderbookEntries = new List<OrderbookEntry>();
        foreach(var priceLevel in _askPriceLevel)
        {
            var head = priceLevel.Head;
            while(head is not null)
            {
                orderbookEntries.Add(head);
                head = head.Next;
            }
        }
        return orderbookEntries;
    }

    public IReadOnlyCollection<OrderbookEntry> GetBidEntries()
    {
        var orderbookEntries = new List<OrderbookEntry>();
        foreach(var priceLevel in _bidPriceLevel)
        {
            var head = priceLevel.Head;
            while(head != null)
            {
                orderbookEntries.Add(head);
                head = head.Next;
            }
        }
        return orderbookEntries;
    }

    public OrderbookSpread GetSpread()
    {
        long? bestBid = null, bestAsk = null;
        if (_askPriceLevel.Count != 0)
        {
            bestAsk = _askPriceLevel.Min!.Price;
        }
        if (_bidPriceLevel.Count != 0 )
        {
            bestBid = _bidPriceLevel.Max!.Price;
        }
        return new OrderbookSpread(bestBid, bestAsk);
    }

    public void ModifyOrder(ModifyOrder modifyOrder)
    {
        if (_orderbook.ContainsKey(modifyOrder.OrderId))
        {
            RemoveOrder(modifyOrder.ToCancelOrder());
            AddOrder(modifyOrder.ToNewOrder());
        }
        else
        {
            _logger.Error($"{nameof(Orderbook)}", $"OrderID {modifyOrder.OrderId} does not exist in orderbook and cannot be modified");
        }

    }

    public void RemoveOrder(CancelOrder cancelOrder)
    {
        if(_orderbook.TryGetValue(cancelOrder.OrderId, out var orderbookEntry))
        {
            RemoveOrder(cancelOrder.OrderId, orderbookEntry, _orderbook,
                orderbookEntry.Order.IsBuySide ? _bidPriceLevel : _askPriceLevel);
        }
        else
        {
            _logger.Error($"{nameof(Orderbook)}", $"OrderID {cancelOrder.OrderId} does not exist in orderbook and cannot be removed");
        }
    }

    private static void RemoveOrder(long orderId, OrderbookEntry orderbookEntry, IDictionary<long,
    OrderbookEntry> orderbook, SortedSet<PriceLevel> priceLevels)
    {
        // remove the orderbook entry within the linkedlist
        if (orderbookEntry.Previous != null && orderbookEntry.Next != null)
        {
            orderbookEntry.Next.Previous = orderbookEntry.Previous;
            orderbookEntry.Previous.Next = orderbookEntry.Next;
        }
        else if (orderbookEntry.Next != null)
        {
            orderbookEntry.Next.Previous = null;
        }
        else if (orderbookEntry.Previous != null)
        {
            orderbookEntry.Previous.Next = null;
        }

        var priceLevel = priceLevels.FirstOrDefault(pl => pl.Head == orderbookEntry || pl.Tail == orderbookEntry);

        if (priceLevel is not null)
        {
            if (priceLevel.Head == orderbookEntry && priceLevel.Tail == orderbookEntry)
            {
                // only 1 order on the level, then remove the price level.
                priceLevels.Remove(priceLevel);
            }
            else if (priceLevel.Head == orderbookEntry && priceLevel.Head.Next is not null)
            {
                // more than 1 order but OBE is the first order on level 
                priceLevel.Head = priceLevel.Head.Next;
            }
            else if (priceLevel.Tail == orderbookEntry  && priceLevel.Tail.Previous is not null)
            {
                // more than 1 order but OBE is the last order on level 
                priceLevel.Tail = priceLevel.Tail.Previous;
            }
        }

        orderbook.Remove(orderId);
    }

    public SortedSet<PriceLevel> GetBidPriceLevel()
    {
        return _bidPriceLevel;
    }

    public SortedSet<PriceLevel> GetAskPriceLevel()
    {
        return _askPriceLevel;
    }

    // private readonly Security _instrument;
    private readonly ITextLogger _logger;
    private SortedSet<PriceLevel> _bidPriceLevel = new SortedSet<PriceLevel>(BidPriceLevelComparer.Comparer);
    private SortedSet<PriceLevel> _askPriceLevel = new SortedSet<PriceLevel>(AskPriceLevelComparer.Comparer);
    private Dictionary<long, OrderbookEntry> _orderbook = new Dictionary<long, OrderbookEntry>();
}
