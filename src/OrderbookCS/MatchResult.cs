using System;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public record MatchResult(Order UserOrder, IReadOnlyCollection<MatchedRecord> MatchedOrders, bool IsCompletelyFilled);
public record MatchedRecord(Order MatchedOrder, uint Quantity, long Price);
