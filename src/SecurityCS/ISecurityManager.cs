using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Security;

public interface ISecurityManager
{
    void AddSecurity(string symbol, string name, string assetType);
    IRetrievalOrderbook GetOrderbookForSymbol(string symbol);
    IRetrievalOrderbook GetOrderbookForOrderId(string orderId);
    bool ContainsSecurity(string symbol);
    Security GetSecurity(string symbol);
}
