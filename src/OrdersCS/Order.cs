using System;

namespace TradingEngineServer.Orders;

public class Order: IOrderCore
{
    public Order(IOrderCore orderCore, uint quantity, long price, bool isBuySide)
    {
        _orderCore = orderCore;
        InititalQuantity = quantity;
        CurrentQuantity = quantity;
        Price = price;
        IsBuySide = isBuySide;
    }

    public void IncreaseQuantity(uint quantity)
    {
        CurrentQuantity += quantity;
    }

    public void DecreaseQuantity(uint quantity)
    {
        if (quantity > CurrentQuantity)
        {
            throw new Exception($"Quantity {quantity} is greater than the Current Quantity {CurrentQuantity} "
                + $"for orderId: {OrderId}");
        }
        CurrentQuantity -= quantity;
    }

    public uint InititalQuantity { get; init; }
    public uint CurrentQuantity { get; private set; }
    public long Price { get; init; }
    public bool IsBuySide { get; init; }
    public long OrderId => _orderCore.OrderId;
    public string Username => _orderCore.Username;
    private readonly IOrderCore _orderCore;
}
