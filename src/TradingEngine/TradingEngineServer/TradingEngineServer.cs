using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Core;

internal class TradingEngineServer: BackgroundService, ITradingEngineServer
{
    public TradingEngineServer(ITextLogger textLogger, IOptions<TradingEngineServerConfiguration> config,
    IRetrievalOrderbook orderbook, IMatchingOrderbook fifo)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _tradingEngineServerConfiguration = config.Value ?? throw new ArgumentNullException(nameof(config));
        _orderbook = orderbook;
        _fifo = fifo;
    }

    public Task Run(CancellationToken token)
    {
        return ExecuteAsync(token);
    }

    protected override Task ExecuteAsync(CancellationToken token)
    {
        _logger.Information(nameof(TradingEngineServer), $"Starting Trading Engine");
        string? input = Console.ReadLine();
        while(!token.IsCancellationRequested && input != "")
        {
            if (input == "init")
            {
                // inititalize buy orders in orderbook
                var order = new Order(new OrderCore(1, "username1"), 5, 12, true);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(2, "username2"), 10, 10, true);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(3, "username3"), 10, 8, true);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(4, "username4"), 15, 6, true);
                _orderbook.AddOrder(order);

                // inititalize sell orders in orderbook
                order = new Order(new OrderCore(5, "username5"), 10, 14, false);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(6, "username6"), 10, 16, false);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(7, "username7"), 5, 18, false);
                _orderbook.AddOrder(order);
                order = new Order(new OrderCore(8, "username8"), 15, 20, false);
                _orderbook.AddOrder(order);

                // test orders
                order = new Order(new OrderCore(9, "username9"), 45, 5, false);
                var result = _fifo.Match(order);
                order = new Order(new OrderCore(10, "username10"), 5, 21, true);
                var result2 = _fifo.Match(order);
                break;
            }
        }
        _logger.Information(nameof(TradingEngineServer), $"Stopped Trading Engine");
        return Task.CompletedTask;
    }

    private readonly ITextLogger _logger;
    private readonly TradingEngineServerConfiguration _tradingEngineServerConfiguration;
    private readonly IRetrievalOrderbook _orderbook;
    private readonly IMatchingOrderbook _fifo;

}
