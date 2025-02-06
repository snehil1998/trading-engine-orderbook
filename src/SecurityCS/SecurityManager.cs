using TradingEngineServer.Orderbook;
using TradingEngineServer.Logging;

namespace TradingEngineServer.Security;

public class SecurityManager: ISecurityManager
{
    public SecurityManager(ITextLogger logger)
    {
        _orderbookForSecurity = new Dictionary<string, IRetrievalOrderbook>();
        _securities = new List<Security>();
        _logger = logger;
    }

    public void AddSecurity(string symbol, string name, string assetType)
    {
        if (ContainsSecurity(symbol))
        {
            throw new Exception($"Symbol {symbol} already exists");
        }
        _securities.Add(new Security(symbol, name, assetType));
        _orderbookForSecurity.Add(symbol, new OrderBook(_logger));
    }

    public IRetrievalOrderbook GetOrderbookForSymbol(string symbol)
    {
        if (!ContainsSecurity(symbol))
        {
            throw new Exception($"Symbol {symbol} does not exist");
        }
        return _orderbookForSecurity[symbol];
    }

    public IRetrievalOrderbook GetOrderbookForOrderId(string orderId)
    {
        if (!_orderbookForSecurity.Values.Any(x => x.ContainsOrder(orderId)))
        {
            throw new Exception($"OrderID {orderId} does not exist");
        }
        return _orderbookForSecurity.Values.First(x => x.ContainsOrder(orderId));
    }

    public bool ContainsSecurity(string symbol) => _securities.Any(x => x.Symbol == symbol);

    public Security GetSecurity(string symbol) {
        if (!ContainsSecurity(symbol))
        {
            throw new Exception($"Symbol {symbol} does not exist");
        }
        return _securities.First(x => x.Symbol == symbol);
    }

    private IDictionary<string, IRetrievalOrderbook> _orderbookForSecurity;
    private IList<Security> _securities;
    private ITextLogger _logger;
}
