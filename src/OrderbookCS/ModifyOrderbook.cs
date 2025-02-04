using System;
using TradingEngineServer.Logging;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Orderbook;

public class ModifyOrderbook: IModifyOrderbook
{
    public ModifyOrderbook(IRetrievalOrderbook orderbook, IMatchingOrderbook matchingOrderbook, ITextLogger logger)
    {
        _orderbook = orderbook;
        _matchingOrderbook = matchingOrderbook;
        _logger = logger;
    }

    public void ModifyOrder(ModifyOrder modifyOrder)
    {
        if (_orderbook.ContainsOrder(modifyOrder.OrderId))
        {
            _orderbook.RemoveOrder(modifyOrder.ToCancelOrder());
            
            _matchingOrderbook.Match(modifyOrder.ToNewOrder());
        }
        else
        {
            _logger.Error($"{nameof(Orderbook)}", $"OrderID {modifyOrder.OrderId} does not exist in orderbook and cannot be modified");
        }
    }

    private readonly IMatchingOrderbook _matchingOrderbook;
    private readonly IRetrievalOrderbook _orderbook;
    private readonly ITextLogger _logger;
}
