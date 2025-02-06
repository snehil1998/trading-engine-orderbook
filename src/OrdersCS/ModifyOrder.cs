namespace TradingEngineServer.Orders;

public sealed class ModifyOrder: IOrderCore
{
    public ModifyOrder(IOrderCore orderCore, long modifiedPrice, uint modifiedQuantity, bool isBuySide)
    {
        _orderCore = orderCore;

        Price = modifiedPrice;
        Quantity = modifiedQuantity;
        IsBuySide = isBuySide;
    }

    public CancelOrder ToCancelOrder()
    {
        return new CancelOrder(this);
    }

    public Order ToNewOrder()
    {
        return new Order(this, Quantity, Price, IsBuySide);
    }

    public uint Quantity { get; init; }
    public long Price { get; init; }
    public bool IsBuySide { get; init; }
    public string OrderId => _orderCore.OrderId;
    public string Username => _orderCore.Username;

    private readonly IOrderCore _orderCore;
}
