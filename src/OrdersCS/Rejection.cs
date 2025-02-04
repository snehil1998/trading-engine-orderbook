using TradingEngineServer.Orders;

namespace TradingEngineServer.Rejects;

public class Rejection: IOrderCore
{
    public Rejection(IOrderCore rejectedOrder, RejectionReason rejectionReason)
    {
        _rejectedOrder = rejectedOrder;
        RejectionReason = rejectionReason;
    }

    public string OrderId => _rejectedOrder.OrderId;
    public string Username => _rejectedOrder.Username;
    public RejectionReason RejectionReason { get; init; }


    private readonly IOrderCore _rejectedOrder;
}
