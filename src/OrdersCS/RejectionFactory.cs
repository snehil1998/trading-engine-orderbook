using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects;

public sealed class RejectionFactory
{
    public static Rejection CreateRejection(IOrderCore rejectedOrder, RejectionReason reason)
    {
        return new Rejection(rejectedOrder, reason);
    }
}
