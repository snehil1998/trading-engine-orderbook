using TradingEngineServer.Logging;

namespace TradingEngineServer.Input;

internal sealed class InputFactory
{
    public InputFactory(ITextLogger textLogger)
    {
        _logger = textLogger;
    }

    public string? EnterUsername(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter username or enter 'exit' to exit: ");
            string? username = Console.ReadLine();
            if (username?.Trim().ToLower() == "exit") break;
            if (username is null)
            {
                Console.WriteLine("Invalid username. Try again.");
                continue;
            }
            return username;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering username");
        return null;
    }

    public uint? EnterQuantity(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter quantity or enter 'exit' to exit: ");
            string? quantityInput = Console.ReadLine();
            if (quantityInput?.Trim().ToLower() == "exit") break;
            if (!uint.TryParse(quantityInput, out uint quantity))
            {
                Console.WriteLine("Invalid quantity. Try again.");
                continue;
            }
            return quantity;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering quantity");
        return null;
    }

    public long? EnterPrice(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter price or enter 'exit' to exit: ");
            string? priceInput = Console.ReadLine();
            if (priceInput?.Trim().ToLower() == "exit") break;
            if (!long.TryParse(priceInput, out long price))
            {
                Console.WriteLine("Invalid price. Try again.");
                continue;
            }
            return price;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering price");
        return null;
    }

    public bool? EnterSide(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter side (buy/sell) or enter 'exit' to exit: ");
            string? sideInput = Console.ReadLine()?.Trim().ToLower();
            if (sideInput == "exit") break;
            if (sideInput != "buy" && sideInput != "sell")
            {
                Console.WriteLine("Invalid side. Try again.");
                continue;
            }
            return sideInput == "buy";
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering side");
        return null;
    }

    public string? EnterOrderID(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter orderID or enter 'exit' to exit: ");
            string? orderId = Console.ReadLine();
            if (orderId?.Trim().ToLower() == "exit") break;
            if (orderId is null)
            {
                Console.WriteLine("Invalid orderID. Try again.");
                continue;
            }
            return orderId;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering orderID");
        return null;
    }

    public string? EnterSecuritySymbol(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter security symbol or enter 'exit' to exit: ");
            string? symbol = Console.ReadLine();
            if (symbol?.Trim().ToLower() == "exit") break;
            if (symbol is null)
            {
                Console.WriteLine("Invalid security symbol. Try again.");
                continue;
            }
            return symbol;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering security symbol");
        return null;
    }

    public string? EnterSecurityName(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter security name or enter 'exit' to exit: ");
            string? name = Console.ReadLine();
            if (name?.Trim().ToLower() == "exit") break;
            if (name is null)
            {
                Console.WriteLine("Invalid security name. Try again.");
                continue;
            }
            return name;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering security name");
        return null;
    }

    public string? EnterSecurityAssetType(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter security asset type or enter 'exit' to exit: ");
            string? assetType = Console.ReadLine();
            if (assetType?.Trim().ToLower() == "exit") break;
            if (assetType is null)
            {
                Console.WriteLine("Invalid security asset type. Try again.");
                continue;
            }
            return assetType;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering security asset type");
        return null;
    }

    private readonly ITextLogger _logger;
}
