using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public interface IRetrievalOrderbook: IEntryOrderbook
{
    IReadOnlyCollection<OrderbookEntry> GetBidEntries();
    IReadOnlyCollection<OrderbookEntry> GetAskEntries();
    public IList<PriceLevel> GetBidPriceLevel();
    public IList<PriceLevel> GetAskPriceLevel();
}
