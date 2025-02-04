using System.Security.Principal;

namespace TradingEngineServer.Orders;

public interface IOrderCore
{
    public string OrderId { get; }
    public string Username { get; }
}
