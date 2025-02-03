namespace TradingEngineServer.Orderbook;

public interface IReadOnlyOrderbook
{
    public bool ContainsOrder(long OrderId);
    public OrderbookSpread GetSpread();

    public int Count { get; }
}
