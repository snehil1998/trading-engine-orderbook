namespace TradingEngineServer.Orderbook;

public interface IReadOnlyOrderbook
{
    public bool ContainsOrder(string OrderId);
    public OrderbookSpread GetSpread();

    public int Count { get; }
}
