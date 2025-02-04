using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Input;
using TradingEngineServer.Logging;
using TradingEngineServer.Logging.LoggerConfiguration;
using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Core;

public sealed class TradingEngineServerHostBuilder
{
    public static IHost BuildTradingEngineServer() =>
    Host.CreateDefaultBuilder().ConfigureServices((context, services) => {
        // trading engine server configuration
        services.AddOptions();
        services.Configure<TradingEngineServerConfiguration>(
            context.Configuration.GetSection(nameof(TradingEngineServerConfiguration)));
        services.Configure<LoggingConfiguration>(
            context.Configuration.GetSection(nameof(LoggingConfiguration)));

        // add singleton objects
        services.AddSingleton<ITradingEngineServer, TradingEngineServer>();
        services.AddSingleton<ITextLogger, TextLogger>();
        services.AddSingleton<IRetrievalOrderbook, Orderbook.Orderbook>();
        services.AddSingleton<IMatchingOrderbook, Fifo>();
        services.AddSingleton<IModifyOrderbook, ModifyOrderbook>();
        services.AddSingleton<IActionsFactory, ActionsFactory>();

        // Add controllers and MVC
        services.AddControllers();

        // adding hosted service
        services.AddHostedService<TradingEngineServer>();
    }).Build();
}

