using System;

namespace TradingEngineServer.Orderbook;

public sealed class OrderbookSpread
{
    public OrderbookSpread(long? bid, long? ask)
    {
        Bid = bid;
        Ask = ask;
    }

    public long? GetSpread()
    {
        if (Bid.HasValue && Ask.HasValue)
        {
            return  Ask.Value - Bid.Value;
        }
        return null;
    }

    public long? Bid { get; }
    public long? Ask { get; }
}
