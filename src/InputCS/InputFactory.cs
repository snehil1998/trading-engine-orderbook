using TradingEngineServer.Logging;

namespace TradingEngineServer.Input;

internal class InputFactory
{
    public InputFactory(ITextLogger textLogger)
    {
        _logger = textLogger;
    }
    public int? EnterUserId(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            Console.Write("Enter userID or enter 'exit' to exit: ");
            string? userIdInput = Console.ReadLine();
            if (userIdInput?.Trim().ToLower() == "exit") break;
            if (userIdInput is null || !int.TryParse(userIdInput, out int userId))
            {
                Console.WriteLine("Invalid userId. Try again.");
                continue;
            }
            return userId;
        }
        _logger.Information(nameof(InputFactory), "Exited from the program while entering userID");
        return null;
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
            Console.Write("Enter quantity or enter 'exit' to exit");
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

    private readonly ITextLogger _logger;
}
