using System.Text;
using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Input;

public class ActionsFactory: IActionsFactory
{
    public ActionsFactory(ITextLogger textLogger, IRetrievalOrderbook orderbook, IMatchingOrderbook fifo, IModifyOrderbook modifyOrderbook)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _inputFactory = new InputFactory(textLogger);
        _orderbook = orderbook;
        _fifo = fifo;
        _modifyOrderbook = modifyOrderbook;
        _random = new Random();
    }

    public void AddOrder(string username, CancellationToken token)
    {
        string prefix = "ORD";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        int randomPart = _random.Next(1000, 9999);
        string orderId = $"{prefix}-{timestamp}-{randomPart}";

        uint? quantity = _inputFactory.EnterQuantity(token);
        if (quantity is null) return;

        long? price = _inputFactory.EnterPrice(token);
        if (price is null) return;

        bool? isBuy = _inputFactory.EnterSide(token);
        if (isBuy is null) return;

        var order = new Order(new OrderCore(orderId, username), quantity.Value, price.Value, isBuy.Value);
        var result = _fifo.Match(order);

        Console.WriteLine($"Matched orders: {result.MatchedOrders.Count}");
        foreach(var matchedOrder in result.MatchedOrders)
        {          
            Console.WriteLine($"Price: {matchedOrder.Price}, Quantity: {matchedOrder.Quantity}");
        }

        Console.WriteLine($"Order {orderId} successfully added!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully added.");   
    }

    public void CancelOrder(string username, CancellationToken token)
    {
        string? orderId = _inputFactory.EnterOrderID(token);
        if (orderId is null) return;

        if (!_orderbook.ContainsOrder(orderId))
        {
            _logger.Error(nameof(ActionsFactory), $"OrderID {orderId} does not exist in the orderbook");
            return;
        }

        var cancelOrder = new CancelOrder(new OrderCore(orderId, username));
        _orderbook.RemoveOrder(cancelOrder);

        Console.WriteLine($"Order {orderId} successfully cancelled!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully cancelled.");   
    }

    public void ModfifyOrder(string username, CancellationToken token)
    {
        string? orderId = _inputFactory.EnterOrderID(token);
        if (orderId is null) return;

        if (!_orderbook.ContainsOrder(orderId))
        {
            _logger.Error(nameof(ActionsFactory), $"OrderID {orderId} does not exist in the orderbook");
            return;
        }

        uint? quantity = _inputFactory.EnterQuantity(token);
        if (quantity is null) return;

        long? price = _inputFactory.EnterPrice(token);
        if (price is null) return;

        bool? isBuy = _inputFactory.EnterSide(token);
        if (isBuy is null) return;

        var modifyOrder = new ModifyOrder(new OrderCore(orderId, username), price.Value, quantity.Value, isBuy.Value);
        _modifyOrderbook.ModifyOrder(modifyOrder);

        Console.WriteLine($"Order {orderId} successfully modified!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully modified.");     
    }

    public void PrintOrderbook()
    {
        var bidPriceLevels = _orderbook.GetBidPriceLevel();
        var askPriceLevels = _orderbook.GetAskPriceLevel();

        Console.WriteLine($"Price : Quantity : Orders_count");
        Console.WriteLine($"Sell price");
        foreach(var priceLevel in askPriceLevels)
        {
            Console.WriteLine($"{priceLevel.Price} : {priceLevel.GetLevelOrderQuantity()} : {priceLevel.GetLevelOrderNumber()}");
        }
        Console.WriteLine("--------");
        Console.WriteLine($"Buy price");
        foreach(var priceLevel in bidPriceLevels)
        {
            Console.WriteLine($"{priceLevel.Price} : {priceLevel.GetLevelOrderQuantity()} : {priceLevel.GetLevelOrderNumber()}");
        }
    }

    public void PrintOrders()
    {
        var bidPriceLevels = _orderbook.GetBidPriceLevel();
        var askPriceLevels = _orderbook.GetAskPriceLevel();
        
        Console.WriteLine("Price = [(OrderID, Quantity)]");
        Console.WriteLine($"Sell orders");
        foreach(var priceLevel in askPriceLevels)
        {
            var orders = priceLevel.GetLevelOrders().Select(x => $"({x.OrderId}, {x.CurrentQuantity})");
            Console.WriteLine($"{priceLevel.Price} = [{string.Join( ", ", orders)}]");
        }
        Console.WriteLine("--------");
        Console.WriteLine($"Buy orders");
        foreach(var priceLevel in bidPriceLevels)
        {
            var orders = priceLevel.GetLevelOrders().Select(x => $"({x.OrderId}, {x.CurrentQuantity})");
            Console.WriteLine($"{priceLevel.Price} = [{string.Join( ", ", orders)}]");
        }
    }

    private readonly ITextLogger _logger;
    private readonly InputFactory _inputFactory;
    private readonly IRetrievalOrderbook _orderbook;
    private readonly IMatchingOrderbook _fifo;
    private readonly IModifyOrderbook _modifyOrderbook;
    private readonly Random _random;

}
