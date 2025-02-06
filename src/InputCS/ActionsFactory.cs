using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;
using TradingEngineServer.Security;

namespace TradingEngineServer.Input;

public sealed class ActionsFactory: IActionsFactory
{
    public ActionsFactory(ITextLogger textLogger, ISecurityManager securityManager)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _inputFactory = new InputFactory(textLogger);
        _securityManager = securityManager;
        _random = new Random();
    }

    public void AddSecurity(CancellationToken token)
    {
        string? symbol = _inputFactory.EnterSecuritySymbol(token);
        if (symbol is null) return;

        string? name = _inputFactory.EnterSecurityName(token);
        if (name is null) return;

        string? assetType = _inputFactory.EnterSecurityAssetType(token);
        if (assetType is null) return;

        try
        {
            _securityManager.AddSecurity(symbol, name, assetType);
        }
        catch(Exception e)
        {
            Console.WriteLine($"Failed to add security {symbol}!");
            _logger.Error(nameof(ActionsFactory), $"Could not add security ({symbol}, {name}, {assetType}): {e.Message}");
            return;
        }

        Console.WriteLine($"Security {symbol} successfully added!");
        _logger.Information(nameof(ActionsFactory), $"Security {symbol} successfully added.");
    }

    public void AddOrder(string username, CancellationToken token)
    {
        string prefix = "ORD";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        int randomPart = _random.Next(1000, 9999);
        string orderId = $"{prefix}-{timestamp}-{randomPart}";

        string? symbol = _inputFactory.EnterSecuritySymbol(token);
        if (symbol is null) return;

        uint? quantity = _inputFactory.EnterQuantity(token);
        if (quantity is null) return;

        long? price = _inputFactory.EnterPrice(token);
        if (price is null) return;

        bool? isBuy = _inputFactory.EnterSide(token);
        if (isBuy is null) return;

        try
        {
            var orderbook = _securityManager.GetOrderbookForSymbol(symbol);
            var matchingOrderbook = new Fifo(orderbook, _logger);
            var order = new Order(new OrderCore(orderId, username), quantity.Value, price.Value, isBuy.Value);
            var result = matchingOrderbook.Match(order);

            Console.WriteLine($"Matched orders: {result.MatchedOrders.Count}");
            foreach(var matchedOrder in result.MatchedOrders)
            {          
                Console.WriteLine($"Price: {matchedOrder.Price}, Quantity: {matchedOrder.Quantity}");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Failed to add order {orderId}!");
            _logger.Error(nameof(ActionsFactory), $"Failed to add order {orderId}: {e.Message}");
            return;
        }

        Console.WriteLine($"Order {orderId} successfully added!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully added."); 
    }

    public void CancelOrder(string username, CancellationToken token)
    {
        string? orderId = _inputFactory.EnterOrderID(token);
        if (orderId is null) return;

        try
        {
            var orderbook = _securityManager.GetOrderbookForOrderId(orderId);
            var cancelOrder = new CancelOrder(new OrderCore(orderId, username));
            orderbook.RemoveOrder(cancelOrder);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to cancel order {orderId}!");
            _logger.Error(nameof(ActionsFactory), $"Failed to cancel order {orderId}: {e.Message}");
            return;
        }

        Console.WriteLine($"Order {orderId} successfully cancelled!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully cancelled.");
    }

    public void ModifyOrder(string username, CancellationToken token)
    {
        string? orderId = _inputFactory.EnterOrderID(token);
        if (orderId is null) return;

        uint? quantity = _inputFactory.EnterQuantity(token);
        if (quantity is null) return;

        long? price = _inputFactory.EnterPrice(token);
        if (price is null) return;

        bool? isBuy = _inputFactory.EnterSide(token);
        if (isBuy is null) return;

        try
        {
            var orderbook = _securityManager.GetOrderbookForOrderId(orderId);
            var matchingOrderbook = new Fifo(orderbook, _logger);
            var modifyOrderbook = new ModifyOrderbook(orderbook, matchingOrderbook, _logger);
            var modifyOrder = new ModifyOrder(new OrderCore(orderId, username), price.Value, quantity.Value, isBuy.Value);
            modifyOrderbook.ModifyOrder(modifyOrder);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to modify order {orderId}!");
            _logger.Error(nameof(ActionsFactory), $"Failed to modify order: {e.Message}");
            return;
        }

        Console.WriteLine($"Order {orderId} successfully modified!");
        _logger.Information(nameof(ActionsFactory), $"Order {orderId} successfully modified.");     
    }

    public void PrintOrderbook(CancellationToken token)
    {
        string? symbol = _inputFactory.EnterSecuritySymbol(token);
        if (symbol is null) return;

        try
        {
            var orderbook = _securityManager.GetOrderbookForSymbol(symbol);
            var bidPriceLevels = orderbook.GetBidPriceLevel();
            var askPriceLevels = orderbook.GetAskPriceLevel();

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
        catch(Exception e)
        {
            Console.WriteLine($"Could not print orderbook!");
            _logger.Error(nameof(ActionsFactory), $"Could not print orderbook: {e.Message}");
        }
    }

    public void PrintOrders(CancellationToken token)
    {
        string? symbol = _inputFactory.EnterSecuritySymbol(token);
        if (symbol is null) return;

        try
        {
            var orderbook = _securityManager.GetOrderbookForSymbol(symbol);
            var bidPriceLevels = orderbook.GetBidPriceLevel();
            var askPriceLevels = orderbook.GetAskPriceLevel();
            
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
        catch(Exception e)
        {
            Console.WriteLine($"Could not print orders!");
            _logger.Error(nameof(ActionsFactory), $"Could not print orders: {e.Message}");
        }
    }

    private readonly ITextLogger _logger;
    private readonly InputFactory _inputFactory;
    private readonly ISecurityManager _securityManager;
    private readonly Random _random;

}
