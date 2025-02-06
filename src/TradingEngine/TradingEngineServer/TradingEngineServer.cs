using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Input;

namespace TradingEngineServer.Core;

internal class TradingEngineServer: BackgroundService, ITradingEngineServer
{
    public TradingEngineServer(ITextLogger textLogger, IOptions<TradingEngineServerConfiguration> config, IActionsFactory actionsFactory)
    {
        _logger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));
        _tradingEngineServerConfiguration = config.Value ?? throw new ArgumentNullException(nameof(config));
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
        
            Console.WriteLine("Enter your username or 'exit' to exit: ");
            string username = Console.ReadLine() ?? throw new Exception("Invalid username");
            if (username.ToLower() == "exit") break;

            while(!token.IsCancellationRequested)
            {
                Console.WriteLine($"Hi {username}! What action do you wish to perform: Add order(1), Remove order(2), Modify order(3), " +
                "Print orderbook(4), Print orders(5), Add security(6), Exit(7): ");
                if (!int.TryParse(Console.ReadLine(), out int input))
                {
                    Console.WriteLine("Invalid action input. Try again.");
                    continue;
                }

                if(input == 1)
                {
                    _actionsFactory.AddOrder(username, token);
                }
                else if(input == 2) {
                    _actionsFactory.CancelOrder(username, token);
                }
                else if(input == 3)
                {
                    _actionsFactory.ModifyOrder(username, token);
                }
                else if(input == 4)
                {
                    _actionsFactory.PrintOrderbook(token); 
                }
                else if(input == 5)
                {
                    _actionsFactory.PrintOrders(token);
                }
                else if(input == 6)
                {
                    _actionsFactory.AddSecurity(token);
                }
                else if(input == 7)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Action {input} does not exist. Try again.");
                    _logger.Warning(nameof(TradingEngineServer), $"Action {input} does not exist. Try again.");
                }                    
            }
        }

        Console.WriteLine("Stopped trading engine");
        _logger.Information(nameof(TradingEngineServer), "Stopped trading engine");
        return Task.CompletedTask;
    }

    private readonly ITextLogger _logger;
    private readonly TradingEngineServerConfiguration _tradingEngineServerConfiguration;
    private readonly IActionsFactory _actionsFactory;
}
