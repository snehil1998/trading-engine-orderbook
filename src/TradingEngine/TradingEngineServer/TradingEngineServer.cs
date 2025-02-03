using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Input;

namespace TradingEngineServer.Core;

internal class TradingEngineServer: BackgroundService, ITradingEngineServer
{
    public TradingEngineServer(ITextLogger textLogger, IOptions<TradingEngineServerConfiguration> config,
    IMatchingOrderbook fifo, IActionsFactory actionsFactory)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _tradingEngineServerConfiguration = config.Value ?? throw new ArgumentNullException(nameof(config));
        _fifo = fifo;
        _actionsFactory = actionsFactory;
    }

    public Task Run(CancellationToken token)
    {
        return ExecuteAsync(token);
    }

    protected override Task ExecuteAsync(CancellationToken token)
    {
        _logger.Information(nameof(TradingEngineServer), $"Starting Trading Engine");
        while(!token.IsCancellationRequested)
        {
            Console.WriteLine("\nWhat action do you wish to perform: Add order(1), Remove order(2), Modify order(3), Exit(4): ");
            if (!int.TryParse(Console.ReadLine(), out int input) || !validInputs.Contains(input))
            {
                Console.WriteLine("Invalid action input. Try again.");
                continue;
            }

            if (input == 4) {
                break;
            }
            else if (input == 1)
            {
                _actionsFactory.AddOrder(_fifo, token);
            }
            else
            {
                Console.WriteLine("Action not implemented yet");
                break;
            } 
        }
        Console.WriteLine("Stopped trading engine");
        _logger.Information(nameof(TradingEngineServer), "Stopped trading engine");
        return Task.CompletedTask;
    }

    private readonly ITextLogger _logger;
    private readonly TradingEngineServerConfiguration _tradingEngineServerConfiguration;
    private readonly IMatchingOrderbook _fifo;
    private readonly IActionsFactory _actionsFactory;
    private readonly int[] validInputs = [1, 2, 3, 4];
}
