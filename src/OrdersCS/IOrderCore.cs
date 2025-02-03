using System.Security.Principal;

namespace TradingEngineServer.Orders;

public interface IOrderCore
{
    public long OrderId { get; }
    public string Username { get; }
}
