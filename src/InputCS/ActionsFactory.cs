using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Input;

public class ActionsFactory: IActionsFactory
{
    public ActionsFactory(ITextLogger textLogger)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _inputFactory = new InputFactory(textLogger);
    }

    public void AddOrder(IMatchingOrderbook fifo, CancellationToken token)
    {
        int? userIdInput = _inputFactory.EnterUserId(token);
        if (userIdInput is null) return;

        string? username = _inputFactory.EnterUsername(token);
        if (username is null) return;

        uint? quantity = _inputFactory.EnterQuantity(token);
        if (quantity is null) return;

        long? price = _inputFactory.EnterPrice(token);
        if (price is null) return;

        bool? isBuy = _inputFactory.EnterSide(token);
        if (isBuy is null) return;

        var order = new Order(new OrderCore(userIdInput.Value, username), quantity.Value, price.Value, isBuy.Value);
        var result = fifo.Match(order);

        Console.WriteLine($"Matched orders: {result.MatchedOrders.Count}");
        foreach(var matchedOrder in result.MatchedOrders)
        {          
            Console.WriteLine($"Price: {matchedOrder.Price}, Quantity: {matchedOrder.Quantity}");
        }

        _logger.Information(nameof(ActionsFactory), "Order successfully added!");   
    }

    private readonly ITextLogger _logger;
    private readonly InputFactory _inputFactory;
}
