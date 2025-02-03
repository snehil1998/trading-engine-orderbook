using System;

namespace TradingEngineServer.Orders;

public sealed class OrderStatusFactory
{
    public static NewOrderStatus CreateNewOrderStatus(Order order)
    {
        return new NewOrderStatus();
    }

    public static CancelOrderStatus CreateCancelOrderStatus(CancelOrder cancelOrder)
    {
        return new CancelOrderStatus();
    }

    public static ModifyOrderStatus CreateModifyOrderStatus(ModifyOrder modifyOrder)
    {
        return new ModifyOrderStatus();
    }
}
