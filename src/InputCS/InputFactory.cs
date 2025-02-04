﻿using TradingEngineServer.Logging;

namespace TradingEngineServer.Input;

internal class InputFactory
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

    private readonly ITextLogger _logger;
}
