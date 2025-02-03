using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public sealed class Fifo: IMatchingOrderbook
{
    public Fifo(IRetrievalOrderbook orderbook)
    {
        _orderbook = orderbook ?? throw new ArgumentNullException(nameof(orderbook));
    }

    public MatchResult Match(Order order)
    {
        if (order == null || order.CurrentQuantity <= 0)
        {
            throw new ArgumentException("Order cannot be null and must have a positive quantity.", nameof(order));
        }

        var matchedRecords = new List<MatchedRecord>();

        if (order.IsBuySide)
        {
            var askOrderPriceLevels = _orderbook.GetAskPriceLevel();
            foreach (var askPriceLevel in askOrderPriceLevels)
            {
                if (askPriceLevel.Price > order.Price || order.CurrentQuantity == 0) break;
                matchedRecords.AddRange(MatchPriceLevels(askPriceLevel, order));
            }
        }
        else
        {
            var bidOrderPriceLevels = _orderbook.GetBidPriceLevel();
            foreach (var bidPriceLevel in bidOrderPriceLevels)
            {
                if (bidPriceLevel.Price < order.Price || order.CurrentQuantity == 0) break;
                matchedRecords.AddRange(MatchPriceLevels(bidPriceLevel, order));
            }
        }
        
        if (order.CurrentQuantity > 0)
        {
            _orderbook.AddOrder(order);
            return new MatchResult(order, matchedRecords, false);
        }
        return new MatchResult(order, matchedRecords, true);
    }

    private List<MatchedRecord> MatchPriceLevels(PriceLevel priceLevel, Order order)
    {
        var matchedRecords = new List<MatchedRecord>();
        var head = priceLevel.Head;
        while(head != null && order.CurrentQuantity > 0)
        {
            if (head.Order.CurrentQuantity > order.CurrentQuantity)
            {
                var orderQuantity = order.CurrentQuantity;
                order.DecreaseQuantity(orderQuantity);
                head.Order.DecreaseQuantity(orderQuantity);
                matchedRecords.Add(new(head.Order, orderQuantity, priceLevel.Price));
                break;
            }
            var matchedQuantity = head.Order.CurrentQuantity;
            order.DecreaseQuantity(matchedQuantity);
            matchedRecords.Add(new(head.Order, matchedQuantity, priceLevel.Price));
            _orderbook.RemoveOrder(new CancelOrder(head.Order));
            head = head.Next;
        }

        return matchedRecords;
    }
    private readonly IRetrievalOrderbook _orderbook;
}
