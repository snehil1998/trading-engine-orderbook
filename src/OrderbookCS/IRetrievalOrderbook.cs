using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IRetrievalOrderbook: IEntryOrderbook
{
    IReadOnlyCollection<OrderbookEntry> GetBidEntries();
    IReadOnlyCollection<OrderbookEntry> GetAskEntries();
    public SortedSet<PriceLevel> GetBidPriceLevel();
    public SortedSet<PriceLevel> GetAskPriceLevel();
}
